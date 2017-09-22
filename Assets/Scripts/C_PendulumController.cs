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
	public double X, Y;

	public Pivot(double x , double y){
		X = x;
		Y = y;
		isOrigin = true;
		Parent = null;
		ParentAngle = 0;
		ParentDir = Direction.UNDEF;
	}

	public Pivot(double x , double y, Pivot parent, float angle, Direction dir){
		X = x;
		Y = y;
		isOrigin = false;
		Parent = parent;
		ParentAngle = angle;
		ParentDir = dir;
	}
}

public class C_PendulumController {
	Stack<Pivot> Pivots;
	C_Player Player;
	Rigidbody2D Pendulum;
	LineRenderer[] lines;
	GameObject collidee;
	GameObject prevcollidee;
	public float Radius;
	public C_PendulumController(Vector2 origin, C_Player player, Rigidbody2D body){
		Pivots = new Stack<Pivot> ();
		Pendulum = body;
		Player = player;
		Radius = (float)Trig.GetHyp (origin.x - body.position.x, origin.y - body.position.y);
		Pivots.Push (new Pivot (origin.x, origin.y));
	}

	public void Update(){
		float y = Input.GetAxis ("Vertical");
		if (y > 0) {
			if ((Radius > y) && (Radius + y> 1)) {
				Radius += -y;
			}
		} else {
			Radius += -y;
		}
	}

	public void FixedUpdate() {
		Pivot pivot = Pivots.Peek ();
		VectorD2 tension = new VectorD2(0,0);
		double centrifugal = 0.00000;
		double x, y, hyp;
		double normalForce = Physics2D.gravity.y * Pendulum.mass;

		x = Pendulum.position.x - Pivots.Peek().X;
		y = Pendulum.position.y - Pivots.Peek().Y;

		hyp = Trig.GetHyp (x, y);
		RaycastHit2D hit = Physics2D.Raycast (new Vector2 ((float)pivot.X, (float)pivot.Y), new Vector2 ((float)x, (float)y));
		collidee = hit.collider.gameObject;
		if (collidee != Pendulum.gameObject) collidee.GetComponent<SpriteRenderer>().material.color = Color.yellow;
		if ((collidee != null) && (prevcollidee != null)) {
			if (collidee != prevcollidee) {
				if (prevcollidee != Pendulum.gameObject) {
					prevcollidee.GetComponent<SpriteRenderer> ().material.color = Color.white ;
				}
			}
		}
		prevcollidee = collidee;
		double angle = Trig.GetAngle (x, y);
		if ((hyp >= Radius)) {
			if (angle > Math.PI) centrifugal += normalForce * Math.Sin (angle);
			double tangentialV = -1 * (Pendulum.velocity.x * Math.Sin (angle)) + (Pendulum.velocity.y * Math.Cos (angle));
			double perpindicularV =  (Pendulum.velocity.x * Math.Cos (angle)) + (Pendulum.velocity.y * Math.Sin (angle));
			//double angularV = tangentialV / ((radius)/2);
			double angularV = tangentialV / ((Radius + hyp)/2);
			centrifugal += perpindicularV * 30;
			centrifugal += Pendulum.mass * Math.Pow (angularV, 2) * ((Radius + hyp)/2);
			//centrifugal += pendulum.mass * Math.Pow (angularV, 2) * ((radius)/2);
			if (Input.GetAxis ("Vertical") > 0)
				centrifugal += (hyp - Radius) * 100;//(hyp - radius) * normalForce * Math.Sin (angle) ;
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
