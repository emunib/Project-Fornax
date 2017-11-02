using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public delegate void InputUpdate();
public delegate void FixedUpdate();
public enum E_PlayerInputState { Ground, Free, Swinging };
public enum E_GrapplingState { Attached, Detached };

public class C_PlayerController : C_WorldObjectController {
	Dictionary<E_PlayerInputState, InputUpdate> InputUpdates;
	Dictionary<E_PlayerInputState, FixedUpdate> FixedUpdates;
	Dictionary<E_GrapplingState, InputUpdate> GraplingUpdates;
	public Rigidbody2D body;
	public Transform GrapplingHookBase;
	GameObject ActiveGrapplingHook;
	C_PendulumController PendulumController;
    LineRenderer RopeLine;


	public float Xaccel = 10;
	public KeyCode XPos = KeyCode.RightArrow, XNeg = KeyCode.LeftArrow, YPos = KeyCode.UpArrow, YNeg = KeyCode.DownArrow;
	public float MaxSpeed;
	public float AngularAccel;
	public float RadiusDelta;
	public E_PlayerInputState PlayerInputState;
	public E_GrapplingState GrapplingState;

	C_PlayerController() {
		InputUpdates = new Dictionary<E_PlayerInputState, InputUpdate> ();
		InputUpdates.Add (E_PlayerInputState.Swinging, SwingingUpdate);
		InputUpdates.Add (E_PlayerInputState.Ground, GroundUpdate);
		InputUpdates.Add (E_PlayerInputState.Free, FreeUpdate);

		FixedUpdates = new Dictionary<E_PlayerInputState, FixedUpdate> ();
		FixedUpdates.Add (E_PlayerInputState.Swinging, SwingingFixedUpdate);
		FixedUpdates.Add (E_PlayerInputState.Ground, GroundFixedUpdate);
		FixedUpdates.Add (E_PlayerInputState.Free, FreeFixedUpdate);
	}
	// Use this for initialization
	void Start () {
		MainCameraController controller = Camera.main.GetComponent<MainCameraController> ();
		controller.player = this.gameObject;
		body = GetComponent<Rigidbody2D> ();
		Manager.ObjectLog.Add (gameObject, this);
		GrapplingState = E_GrapplingState.Detached;
    }
	
	// Update is called once per frame
	void Update () {
		InputUpdates [PlayerInputState] ();
		if (Input.GetMouseButtonDown(0)) {
			if (ActiveGrapplingHook != null) {
				GrapplingState = E_GrapplingState.Detached;
				if (PlayerInputState == E_PlayerInputState.Swinging) {
					PlayerInputState = E_PlayerInputState.Free;
				}
				GameObject.Destroy (ActiveGrapplingHook);
            }
            Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			float vx = v3.x - body.position.x;
			float vy = v3.y - body.position.y;
			double vangle = Trig.GetAngle (vx, vy);
			ActiveGrapplingHook = Instantiate (GrapplingHookBase, new Vector3 (body.position.x + Mathf.Cos ((float)vangle), body.position.y + Mathf.Sin ((float)vangle), 0), new Quaternion ()).gameObject;
			ActiveGrapplingHook.GetComponent<GrapplingHookController> ().SetPendulum (this);
			ActiveGrapplingHook.SetActive (true);
			ActiveGrapplingHook.GetComponent<Rigidbody2D>().velocity = new Vector2 (20 * Mathf.Cos((float)vangle), 20 * Mathf.Sin((float)vangle));
		}

		if ((Input.GetMouseButtonDown(1)) && (GrapplingState == E_GrapplingState.Attached)) {
			GrapplingState = E_GrapplingState.Detached;
			GameObject.Destroy (ActiveGrapplingHook);
			if (PlayerInputState == E_PlayerInputState.Swinging) {
				PlayerInputState = E_PlayerInputState.Free;
			}
		} 
			
	}

	void GroundUpdate(){
		if (Input.GetAxis("Vertical") > 0)
			body.AddForce (Vector2.up * -Physics.gravity.y, ForceMode2D.Impulse);
	}

	public void SwingingUpdate(){
        if (PendulumController != null)
        {
            PendulumController.Update();
        }
	}

	void FreeUpdate() {
		
	}

	void FixedUpdate (){
		RaycastHit2D[] lineGround = Physics2D.RaycastAll (new Vector2 (body.position.x, body.position.y), Vector2.down);
		RaycastHit2D nearestTile = lineGround[0];

        bool found = false;
		foreach (RaycastHit2D obj in lineGround) {
			if (!Manager.ObjectLog.ContainsKey(obj.collider.gameObject)) {
				print ("Object not found");
			} else if (Manager.ObjectLog [obj.collider.gameObject].GetType() == typeof(TileController)) {
				if (found == false) {
					nearestTile = obj;
					found = true;
				} else {
					if (obj.distance < nearestTile.distance) {
						nearestTile = obj;
					}
				}
			}
		}
		if (found) {
			if ((nearestTile.distance > 0.6) && (PlayerInputState == E_PlayerInputState.Ground)) {
				Debug.Log ("Left Ground");
				if (GrapplingState == E_GrapplingState.Attached)
					PlayerInputState = E_PlayerInputState.Swinging;
				else if (GrapplingState == E_GrapplingState.Detached) {
					PlayerInputState = E_PlayerInputState.Free;
				}
			} else if ((nearestTile.distance <= 0.6) && (PlayerInputState != E_PlayerInputState.Ground)) {
				Debug.Log ("Hit Ground");
				PlayerInputState = E_PlayerInputState.Ground;
			}
		} else if (PlayerInputState == E_PlayerInputState.Ground) {
			Debug.Log ("Left Ground");
			if (GrapplingState == E_GrapplingState.Attached)
				PlayerInputState = E_PlayerInputState.Swinging;
			else if (GrapplingState == E_GrapplingState.Detached) {
				PlayerInputState = E_PlayerInputState.Free;
			}
		}
		FixedUpdates [PlayerInputState] ();
	}


	void GroundFixedUpdate() {
		Vector3 direction = new Vector2 (Input.GetAxis ("Horizontal"), 0) * Xaccel;
		body.AddForce(direction);
	}

	void FreeFixedUpdate() {

	}

	void SwingingFixedUpdate() {
            PendulumController.FixedUpdate();
	}

	public void CreateAnchor(float x, float y, LineRenderer rope){
		PendulumController = new C_PendulumController (new Vector2 (x, y), this, body, ActiveGrapplingHook.GetComponent<GrapplingHookController>().Pivots);
        GrapplingState = E_GrapplingState.Attached;
        if (PlayerInputState == E_PlayerInputState.Free)
			PlayerInputState = E_PlayerInputState.Swinging;
	}
}