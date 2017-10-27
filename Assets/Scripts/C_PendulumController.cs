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
	public enum Direction { Neg, Pos, UNDEF };
	public bool isOrigin;
	public Pivot Parent;
	public float ParentAngle;
	public Direction ParentDir;
	public float ParentLen;
	public Vector2 Position;

	public Pivot(Vector2 position){
		Position = position;
		isOrigin = true;
		Parent = null;
		ParentAngle = 0;
		ParentDir = Direction.UNDEF;
	}

	public Pivot(Vector2 position, Pivot parent, float angle, Direction dir, float parentLen){
		Position = position;
		isOrigin = false;
		Parent = parent;
		ParentAngle = angle;
		ParentDir = dir;
		ParentLen = parentLen;
	}
}

public class C_PendulumController {
	List<Pivot> Pivots;
	C_Player Player;
	Rigidbody2D Pendulum;
	public float Radius;
	public C_PendulumController(Vector2 origin, C_Player player, Rigidbody2D body, List<Pivot> pivots){
		Pivots = pivots;
		Pendulum = body;
		Player = player;
		Radius = (float)Trig.GetHyp (origin.x - body.position.x, origin.y - body.position.y);
	}

	public void Update(){
		float y = Input.GetAxis ("Vertical");
		if (y > 0) {
			if (Radius - y > 0.1) {
				Radius -= y;
			} else {
				Radius = 0.1f;
			}
		} else {
			Radius -= y;
		}
		float x = Input.GetAxis ("Horizontal");
        Player.AngularAccel = x;
	}

	public void FixedUpdate() {
		Pivot pivot = Pivots [Pivots.Count - 1];
		Vector2 line2pend = Pendulum.position - pivot.Position;
		double angle = Trig.GetAngle (line2pend.x, line2pend.y);
		RaycastHit2D collidee = Physics2D.Raycast (pivot.Position, line2pend);
        if (Manager.ObjectLog[collidee.collider.gameObject].Object != Player) {
            Vector2 line2collision = collidee.point - pivot.Position;
            // ERROR HERE, x and y can be 0 and trig function throws exception.  This breaks the grappling hook when grappling a surface the player is touching.
            float angle2collision = (float)Trig.GetAngle(line2collision.x, line2collision.y);
			double tangentialV = -1 * (Pendulum.velocity.x * Math.Sin (angle)) + (Pendulum.velocity.y * Math.Cos (angle));
			Pivot.Direction dir;
			double perpindicularAngle;
			if (tangentialV < 0) {
				dir = Pivot.Direction.Neg;
				perpindicularAngle = angle2collision + (Math.PI / 2);
			} else {
				dir = Pivot.Direction.Pos;
				perpindicularAngle = angle2collision - (Math.PI / 2);
			}

			if (perpindicularAngle > (Math.PI * 2)) {
				perpindicularAngle -= Math.PI * 2;
			} else if (perpindicularAngle < 0) {
				perpindicularAngle += Math.PI * 2;
			}
			Vector2 collisionPoint = collidee.point;
			collisionPoint.x += (float)(0.1 * Math.Cos (perpindicularAngle));
			collisionPoint.y += (float)(0.1 * Math.Sin (perpindicularAngle));
			line2collision =  collisionPoint - pivot.Position;
			angle2collision = (float)Trig.GetAngle (line2collision.x, line2collision.y);
			if (line2collision.magnitude > Radius) {
				collisionPoint.x = (float)(Radius * Math.Cos (angle2collision) + pivot.Position.x);
				collisionPoint.y = (float)(Radius * Math.Sin (angle2collision) + pivot.Position.y);
			}
			line2collision =  collisionPoint - pivot.Position;
			Pivots.Add (new Pivot (collisionPoint, pivot, (float)angle2collision, dir, line2collision.magnitude));
			Radius -= line2collision.magnitude;
		} else {
			if (Pivots.Count > 1) {
				if ((pivot.ParentDir == Pivot.Direction.Pos) && (angle < pivot.ParentAngle)) {
					Pivots.Remove (pivot);
					Radius += pivot.ParentLen;
				} else if ((pivot.ParentDir == Pivot.Direction.Neg) && (angle > pivot.ParentAngle)) {
					Pivots.Remove (pivot);
					Radius += pivot.ParentLen;
				}
			}
		}
		pivot = Pivots [Pivots.Count - 1];
		VectorD2 tension = new VectorD2(0,0);
		double centrifugal = 0.00000;
		double hyp;
		double normalForce = Physics2D.gravity.y * Pendulum.mass;
		line2pend = Pendulum.position - pivot.Position;
		hyp = line2pend.magnitude;
		angle = Trig.GetAngle (line2pend.x, line2pend.y);
		if ((hyp >= Radius)) {
			if (angle > Math.PI) centrifugal += normalForce * Math.Sin (angle);
			double tangentialV = -1 * (Pendulum.velocity.x * Math.Sin (angle)) + (Pendulum.velocity.y * Math.Cos (angle));
			double perpindicularV =  (Pendulum.velocity.x * Math.Cos (angle)) + (Pendulum.velocity.y * Math.Sin (angle));
			//double angularV = tangentialV / ((radius)/2);
			double angularV = tangentialV / ((Radius + hyp)/2);
			centrifugal += perpindicularV * 30;
			centrifugal += Pendulum.mass * Math.Pow (angularV, 2) * ((Radius + hyp)/2);
			//centrifugal += pendulum.mass * Math.Pow (angularV, 2) * ((radius)/2);
			centrifugal += (hyp - Radius) * 50;//(hyp - radius) * normalForce * Math.Sin (angle) ;
		}
		tension.x = 0;
		tension.y = 0;
		if (angle > Math.PI) {
			tension.y += Player.AngularAccel * Math.Cos ((Math.PI * 2) - angle);
			tension.x += Player.AngularAccel * Math.Sin ((Math.PI * 2) - angle);
		}

		tension.y += centrifugal * -Math.Sin (angle);
		tension.x += centrifugal * -Math.Cos (angle);
		Pendulum.AddForce (new Vector2((float)tension.x, (float)tension.y), ForceMode2D.Force);
	}

}
