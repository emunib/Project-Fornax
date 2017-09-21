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
	enum PendulumStates { Free, Swinging };
	PendulumStates PendulumState;
	float accel;
	VectorD2 pivot;
	GameObject circle;
	Rigidbody2D pendulum;
	LineRenderer[] lines;
	GameObject collidee;
	GameObject prevcollidee;
	GameObject preFab;
	double radius;
	double radiusDelta;
	// Use this for initialization
	void Start () {
		preFab = GameObject.Find ("GrapplingHook");
		preFab.SetActive (false);
		PendulumState = PendulumStates.Swinging;
		circle = GameObject.Find("Dot");
		for (int j = 1; j <= 3 ; j++) {
			double angle = Math.PI * 2;
			for (int i = 0; i < 5; i++) {
				GameObject _circle = Instantiate (circle);
				double x = Math.Cos (angle) * ((5 * Convert.ToInt64(j > 1) * (j-1)) + Convert.ToInt64(j == 1));
				double y = Math.Sin (angle) * ((5 * Convert.ToInt64(j > 1) * (j-1)) + Convert.ToInt64(j == 1));	
				_circle.transform.position = new Vector2 ((float)x, (float)y);
				angle -= Math.PI / 4;
			}
		}
		radius = 5;
		radiusDelta = 0;
		pivot = new VectorD2(0,0);
		accel = 0;
		pendulum = GetComponent<Rigidbody2D> ();
		lines = this.GetComponentsInChildren<LineRenderer> ();
		lines [0].material.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			switch (PendulumState) {
			case PendulumStates.Free:
				PendulumState = PendulumStates.Swinging;
				radius = Math.Sqrt ((Math.Pow (pendulum.position.x, 2) + Math.Pow (pendulum.position.y, 2)));
				break;
			case PendulumStates.Swinging:
				PendulumState = PendulumStates.Free;
				break;
			}
		}
		if (Input.GetKeyDown (KeyCode.UpArrow))
			radiusDelta += -0.1;
		else if (Input.GetKeyUp (KeyCode.UpArrow))
			radiusDelta -= -0.1;
		if (Input.GetKeyDown (KeyCode.DownArrow))
			radiusDelta += 0.1;
		else if (Input.GetKeyUp (KeyCode.DownArrow))
			radiusDelta -= 0.1;
		if (Input.GetMouseButtonDown(0)) {
			preFab.SetActive (true);
			Instantiate (preFab).SetActive(true);
			preFab.SetActive(false);
		}
		if (radiusDelta < 0) {
			if ((radius > Math.Abs (radiusDelta)) && (radius + radiusDelta > 1)) {
				radius += radiusDelta;
			}
		} else {
			radius += radiusDelta;
		}

		if (Input.GetKeyDown (KeyCode.RightArrow))
			accel += 3;
		else if (Input.GetKeyUp (KeyCode.RightArrow))
			accel -= 3;
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			accel += -3;
		else if (Input.GetKeyUp (KeyCode.LeftArrow))
			accel -= -3;
		double x = pendulum.position.x;
		double y = pendulum.position.y;
		double angle = Math.Atan (y / x);
		if (x < 0) {
			angle += Math.PI;
		} else if (y < 0) {
			angle += 2 * Math.PI;
		}
		double layer;
		double hyp = Math.Sqrt ((Math.Pow (x, 2) + Math.Pow (y, 2)));
		if (hyp < radius) {
			layer = 1;
		} else {
			layer = -1;
		}
		lines[0].SetPosition (1, new Vector3 (pendulum.position.x, pendulum.position.y, 0));
		lines[1].SetPosition (1, new Vector3 ((float)(Math.Cos(angle) * radius), (float)(Math.Sin(angle) * radius), (float)layer));
	}

	void FixedUpdate (){
		switch (PendulumState) {
		case PendulumStates.Free:
				
			break;
		case PendulumStates.Swinging:
			SwingingUpdate ();
			break;
		}
	}

	void SwingingUpdate () {
		VectorD2 tension = new VectorD2(0,0);
		double centrifugal = 0.00000;
		double x;
		double y;
		double hyp;
		double normalForce = Physics2D.gravity.y * pendulum.mass;
		x = pendulum.position.x - pivot.x;
		y = pendulum.position.y - pivot.y;
		hyp = Math.Sqrt ((Math.Pow (x, 2) + Math.Pow (y, 2)));
		RaycastHit2D hit = Physics2D.Raycast (new Vector2 ((float)pivot.x, (float)pivot.y), new Vector2 ((float)x, (float)y));
		collidee = hit.collider.gameObject;
		if (collidee != pendulum.gameObject) collidee.GetComponent<SpriteRenderer>().material.color = Color.yellow;
		if ((collidee != null) && (prevcollidee != null)) {
			if (collidee != prevcollidee) {
				if (prevcollidee != pendulum.gameObject) {
					prevcollidee.GetComponent<SpriteRenderer> ().material.color = Color.white ;
				}
			}
		}
		prevcollidee = collidee;
		double angle = Math.Atan (y / x);
		if (x < 0) {
			angle += Math.PI;
		} else if (y < 0) {
			angle += 2 * Math.PI;
		}
		if ((hyp >= radius)) {
			if (angle > Math.PI) centrifugal += normalForce * Math.Sin (angle);
			double tangentialV = -1 * (pendulum.velocity.x * Math.Sin (angle)) + (pendulum.velocity.y * Math.Cos (angle));
			double perpindicularV =  (pendulum.velocity.x * Math.Cos (angle)) + (pendulum.velocity.y * Math.Sin (angle));
			//double angularV = tangentialV / ((radius)/2);
			double angularV = tangentialV / ((radius + hyp)/2);
			centrifugal += perpindicularV * 30;
			centrifugal += pendulum.mass * Math.Pow (angularV, 2) * ((radius + hyp)/2);
			//centrifugal += pendulum.mass * Math.Pow (angularV, 2) * ((radius)/2);
			if (radiusDelta < 0)
				centrifugal += (hyp - radius) * 100;//(hyp - radius) * normalForce * Math.Sin (angle) ;
		}
		tension.x = 0;
		tension.y = 0;
		if (angle > Math.PI) {
			tension.y += accel * Math.Cos ((Math.PI * 2) - angle);
			tension.x += accel * Math.Sin ((Math.PI * 2) - angle);
		}

		tension.y += centrifugal * -Math.Sin (angle);
		tension.x += centrifugal * -Math.Cos (angle);
		pendulum.AddForce (new Vector2((float)tension.x, (float)tension.y), ForceMode2D.Force);
	}
}
