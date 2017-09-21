using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : C_WorldObjectController {
	Rigidbody2D body;
	GameObject GraplingHookBase;
	GameObject ActiveGraplingHook;
	// Use this for initialization
	void Start () {
		SetObject (new Player ());
		body = GetComponent<Rigidbody2D> ();
		Manager.ObjectLog.Add (gameObject, this);
		GraplingHookBase = GameObject.Find ("GrapplingHook");
		GraplingHookBase.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			body.AddForce (Vector2.up * body.mass * -Physics2D.gravity.y, ForceMode2D.Impulse);
		}
		/*
		if (Input.GetKeyDown (KeyCode.Space)) {
			switch (PendulumState) {
			case PendulumStates.Free:
				PendulumState = PendulumStates.Swinging;
				radius = Trig.GetHyp (pendulum.position.x, pendulum.position.y);
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
			*/
		if (Input.GetMouseButtonDown(0)) {
			if (ActiveGraplingHook != null)
				GameObject.Destroy (ActiveGraplingHook);
			Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			float vx = v3.x - body.position.x;
			float vy = v3.y - body.position.y;
			double vangle = Trig.GetAngle (vx, vy);
			ActiveGraplingHook = Instantiate (GraplingHookBase, new Vector3(body.position.x + Mathf.Cos((float)vangle) , body.position.y + Mathf.Sin((float)vangle), 0), new Quaternion());
			ActiveGraplingHook.SetActive (true);
			ActiveGraplingHook.GetComponent<Rigidbody2D>().velocity = new Vector2 (10 * Mathf.Cos((float)vangle), 10 * Mathf.Sin((float)vangle));
			ActiveGraplingHook.GetComponent<C_WorldObjectController>().SetObject (new GrapplingHook (gameObject));
		}
		/*
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
		double angle = Trig.GetAngle (x, y);
		double layer;
		double hyp = Math.Sqrt ((Math.Pow (x, 2) + Math.Pow (y, 2)));
		if (hyp < radius) {
			layer = 1;
		} else {
			layer = -1;
		}
		lines[0].SetPosition (1, new Vector3 (pendulum.position.x, pendulum.position.y, 0));
		lines[1].SetPosition (1, new Vector3 ((float)(Math.Cos(angle) * radius), (float)(Math.Sin(angle) * radius), (float)layer));
		*/
	}

	void FixedUpdate (){
		
	}

	void SwitchMovState(PlayerMovState newState){

	}

	void GroundFixedUpdate() {

	}

	void FreeFixedUpdate() {

	}
		
	void OnCollisionEnter2D(Collision2D collision){
		if (Manager.ObjectLog[collision.gameObject].Object.GetType() == typeof(Tile)) {
			Debug.Log ("Hit Ground");
		}
	}


	void OnCollisionExit2D(Collision2D coll) {
		if (Manager.ObjectLog[coll.gameObject].Object.GetType() == typeof(Tile)) {
			Debug.Log ("Left Ground");
		}
	}
}
