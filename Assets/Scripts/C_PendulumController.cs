using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

class VectorD2 {
	public VectorD2(double x, double y){
		this.x = x;
		this.y = y;
	}
	public double x;
	public double y;
}

public class Pivot {
	public Pivot Parent;
	public Vector2 Position;

	public Pivot(Vector2 position){
		Position = position;
		Parent = null;
	}

	public Pivot(Vector2 position, Pivot parent){
		Position = position;
		Parent = parent;
	}
}

public class C_PendulumController {
	List<Pivot> Pivots;
	C_PlayerController Player;
	GrapplingHookController Hook;
	Rigidbody2D Pendulum;
	Rigidbody2D Anchor;
	C_WorldObjectController AnchorObject;
	C_WorldObjectController PendulumObject;
	public float Radius;

	public C_PendulumController(C_PlayerController player, GrapplingHookController hook){
		// The 'pendulum' is the grappling hook rigidbody to start.
		// The 'anchor' or the first pivot is the player rigidbody to start.
		Hook = hook;
		Pivots = hook.Pivots;
		Pendulum = hook.Body;
		Player = player;
		Radius = 30;
		AnchorObject = player;
		PendulumObject = hook;
		Anchor = Player.body;
	}

	public void Switch(){
		//Switch the pendulum and the anchor
		//Remove the first pivot, which is the player rigidbody
		//Reverse the order of the remaining pivots
		Pivots.RemoveAt (0);
		List < Pivot> nList = new List<Pivot> ();
		while (Pivots.Count > 0) {
			Pivot temp = Pivots [Pivots.Count - 1];
			nList.Add (temp);
			Pivots.Remove (temp);
		}
		Pivot pivot = new Pivot (Hook.Body.position);
		nList.Insert(0, pivot);
		Pivots = nList;

		AnchorObject = Hook;
		PendulumObject = Player;
		Pendulum = Player.body;
		Anchor = Hook.Body;
		Vector2 secondaryAnchor;

		//Set the position of the new pivot to the position of the new anchor
		//Adjust the position of the new pivot until it is not inside any colliders
		if (Pivots.Count > 1) {
			secondaryAnchor = Pivots [1].Position;

			while (((secondaryAnchor - pivot.Position).magnitude < 0.75) && (Pivots.Count > 1)) {
				//If the secondary anchor is too close to the new pivot, the raycast will not detect colliders properly
				//Too avoid this delete the offending pivot and set the secondary anchor to the next pivot inline
				Pivots.Remove (Pivots [1]);
				if (Pivots.Count > 1) {
					secondaryAnchor = Pivots [1].Position;
				} else {
					secondaryAnchor = Pendulum.position;
				}
			}
		} else {
			secondaryAnchor = Pendulum.position;
		}

		RaycastHit2D empty = new RaycastHit2D ();
		RaycastHit2D result = new RaycastHit2D ();

		Loop.WhileOrFor (() => ((result = GetHits (pivot.Position, secondaryAnchor, empty)) != empty),
			() => {
				Vector2 directionVec = (secondaryAnchor - new Vector2 (result.collider.bounds.center.x, result.collider.bounds.center.y));

				directionVec.Normalize ();
				pivot.Position = pivot.Position + (0.2f * directionVec);
				while (result.collider.bounds.Contains (pivot.Position)) {
					pivot.Position += 0.2f * directionVec;
				}
			}, 100);
				
		//Move the grappling hook to the modified position
		Hook.transform.position = pivot.Position;
		//Set the radius to the distance between the nearest pivot and the pendulum
		Radius = (Pivots[Pivots.Count -1].Position - Pendulum.position).magnitude;
	}

	RaycastHit2D GetHits(Vector2 one, Vector2 two, RaycastHit2D empty){
		RaycastHit2D[] hits = Physics2D.LinecastAll (one, two);
		RaycastHit2D temp = empty;
		foreach (RaycastHit2D hit in hits) {
			if (Manager.ObjectLog [hit.collider.gameObject] != Player) {
				if (temp == empty) {
					temp = hit;
				} else if (hit.distance < temp.distance) {
					temp = hit;
				}
			} 
		}
		return temp;
	}
		

	public void Update(){
		float y = Player.pInput.LeftStickY.Value;
		if (y > 0) {
			if (Radius - y > 0.1) {
				Radius -= y;
			} else {
				Radius = 0.1f;
			}
		} else {
			Radius -= y;
		}
		float x = Player.pInput.LeftStickX.Value;
		Player.AngularAccel = x;
	}

	public void Draw(){
		//If the anchor is an active physics object then update the position of the fatherest pivot, which should be the anchor
		if (Anchor.simulated == true) {
			Pivots [0].Position = Anchor.position;
		}

		//Draw a line with all the pivots as points in the line
		Hook.RopeLine.positionCount = Pivots.Count + 1;
		Hook.RopeLine.SetPosition (0, new Vector3(Pendulum.position.x, Pendulum.position.y));
		int i = Pivots.Count;
		foreach (Pivot pivot in Pivots){
			Hook.RopeLine.SetPosition (i--, new Vector3(pivot.Position.x, pivot.Position.y));
		}
	}

	public void FixedUpdate() {
		Pivot pivot = Pivots [Pivots.Count - 1];
		Vector2 line2pend = Pendulum.position - pivot.Position;
		double angle = Trig.GetAngle (line2pend);
		RaycastHit2D[] collidees = Physics2D.RaycastAll (pivot.Position, line2pend);
		RaycastHit2D collidee = new RaycastHit2D();
		bool notfound = true;

		//Draw a line from the nearest pivot to the pendulum
		//and get the closest hit to the pivot that is not the anchor itself
		foreach (RaycastHit2D hit in collidees){
			if (Manager.ObjectLog [hit.collider.gameObject] != AnchorObject) {
				if (notfound) {
					collidee = hit;
					notfound = false;
				} else if (hit.distance < collidee.distance) {
					collidee = hit;
				}
			}
		}


		//The nearest hit should be the pendulum. If it is not then a new pivot needs to be added
		if (Manager.ObjectLog [collidee.collider.gameObject] != PendulumObject) {	
			// If the nearest pivot is inside of the object that the raycast hit then the logic 
			// of the algorithm has failed, throw an error
			if (collidee.collider.bounds.Contains (Pivots[Pivots.Count - 1].Position)) {
				throw new Exception ("pivot inside of bounding box");

			}

			// This directionVec should lead away from object that the raycast hit
			Vector2 directionVec = (collidee.point - new Vector2 (collidee.collider.bounds.center.x, collidee.collider.bounds.center.y));
			directionVec.Normalize ();

			// Move the point of the hit away from the collider
			Vector2 collisionPoint = collidee.point + (0.1f * directionVec);
			// While the point is inside of the collider move it away from the center
			while (collidee.collider.bounds.Contains (collisionPoint)) {
				 collisionPoint += 0.1f * directionVec;
			}

			Vector2 line2collision =  collisionPoint - pivot.Position;
			Pivots.Add (new Pivot (collisionPoint, pivot));

			// If the new pivot is inside of the object that the raycast hit then the logic 
			// of the algorithm has failed, throw an error
			if (collidee.collider.bounds.Contains (Pivots[Pivots.Count - 1].Position)) {
				throw new Exception ("pivot inside of bounding box");

			}

			// Adjust the radius to account for the length that has been consumed by the new pivot
			Radius -= line2collision.magnitude;
		} else {
			if (Pivots.Count > 1) {
				// If the pendulum can 'see' the pivot behind the nearest pivot then the nearest pivot is unecessary and
				// can be removed
				RaycastHit2D[] hits = Physics2D.LinecastAll (Pendulum.position, Pivots [Pivots.Count - 2].Position);
				if (hits.Length == 1) {
					Radius += (Pivots [Pivots.Count - 2].Position - (Pivots [Pivots.Count - 1].Position)).magnitude;
					Pivots.Remove (pivot);
				}
			}
		}

		// Grab the nearest pivot after adding or deleting any pivots
		pivot = Pivots [Pivots.Count - 1];
		VectorD2 tension = new VectorD2(0,0);
		double centrifugal = 0.00000;
		double hyp;
		double normalForce = Physics2D.gravity.y * Pendulum.mass;

		line2pend = Pendulum.position - pivot.Position;
		hyp = line2pend.magnitude;
		angle = Trig.GetAngle (line2pend);

		//If the rope is 'taut' then the forces of the rope apply
		if ((hyp >= Radius)) {
			// Calculate tension due to the normal force
			if (angle > Math.PI) centrifugal += normalForce * Math.Sin (angle);
			double tangentialV = -1 * (Pendulum.velocity.x * Math.Sin (angle)) + (Pendulum.velocity.y * Math.Cos (angle));
			double perpindicularV =  (Pendulum.velocity.x * Math.Cos (angle)) + (Pendulum.velocity.y * Math.Sin (angle));
			//double angularV = tangentialV / ((radius)/2);
			double angularV = tangentialV / ((Radius + hyp)/2);
			centrifugal += perpindicularV * 30;
			// Calculate tension from centrifugal force
			centrifugal += Pendulum.mass * Math.Pow (angularV, 2) * ((Radius + hyp)/2);
			//centrifugal += pendulum.mass * Math.Pow (angularV, 2) * ((radius)/2);

			//Apply a corrective force to the tension
			//This force should keep the radius stable and prevent small variations in calculation
			// and errors in the physics system due to the granularity of simulated time from accumulating;
			centrifugal += (hyp - Radius) * 50;//(hyp - radius) * normalForce * Math.Sin (angle) ;
		}
		tension.x = 0;
		tension.y = 0;
		Vector2 perpenline = new Vector2 (Mathf.Abs(line2pend.x), -Mathf.Sign(line2pend.x)*(line2pend.y));
		perpenline.Normalize ();

		// Apply the force of the player swinging the rope
		if (angle > Math.PI) {
			tension.y += Player.AngularAccel * perpenline.y * 10;//Math.Cos ((Math.PI * 2) - angle);
			tension.x += Player.AngularAccel * perpenline.x * 10; //Math.Sin ((Math.PI * 2) - angle);
		}


		// Apply the tension of the rope
		tension.y += centrifugal * -Math.Sin (angle);
		tension.x += centrifugal * -Math.Cos (angle);

		// Apply the force to the pendulum
		Pendulum.AddForce (new Vector2((float)tension.x, (float)tension.y), ForceMode2D.Force);
	}

}
