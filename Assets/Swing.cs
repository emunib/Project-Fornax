using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour {
	class VectorD2 {
		public VectorD2(double x, double y){
			this.x = x;
			this.y = y;
		}
		public double x;
		public double y;
	}
	float accel;
	VectorD2 pivot;
	Rigidbody2D pendulum;
	double radius = 5;
	// Use this for initialization
	void Start () {
		pivot = new VectorD2(0,0);
		accel = 0;
		pendulum = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow))
			accel += 3;
		else if (Input.GetKeyUp (KeyCode.RightArrow))
			accel -= 3;
	}

	void FixedUpdate () {
		VectorD2 tension = new VectorD2(0,0);
		double centrifugal = 0.00000;
		double x;
		double y;
		double hyp;
		double normalForce = Physics2D.gravity.y * pendulum.mass;
		x = pendulum.position.x - pivot.x;
		y = pendulum.position.y - pivot.y;
		hyp = Math.Sqrt ((Math.Pow (x, 2) + Math.Pow (y, 2)));
		double angle = Math.Atan (y / x);
		if (x < 0) {
			angle += Math.PI;
		} else if (y < 0) {
			angle += 2 * Math.PI;
		}
		if ((hyp >= radius)) {
			centrifugal += normalForce * Math.Sin (angle);
			double tangentialV = -1 * (pendulum.velocity.x * Math.Sin (angle)) + (pendulum.velocity.y * Math.Cos (angle));
			double perpindicularV =  (pendulum.velocity.x * Math.Cos (angle)) + (pendulum.velocity.y * Math.Sin (angle));
			double angularV = tangentialV / ((radius * 1.1 + hyp * .9)/2);
			centrifugal += perpindicularV * 10;
			centrifugal += pendulum.mass * Math.Pow (angularV, 2) * ((radius * 1.1 + hyp * .9)/2);
		}
		tension.x = 0;
		tension.y = 0;
		if (angle > Math.PI) {
			tension.y = accel * Math.Cos ((Math.PI * 2) - angle);
			tension.x = accel * Math.Sin ((Math.PI * 2) - angle);
		}

		tension.y += centrifugal * -Math.Sin (angle);
		tension.x += centrifugal * -Math.Cos (angle);
		pendulum.AddForce (new Vector2((float)tension.x, (float)tension.y), ForceMode2D.Force);
	}
}
