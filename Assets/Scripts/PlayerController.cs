using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public delegate void InputUpdate();
public delegate void FixedUpdate();

public class PlayerController : C_WorldObjectController {
	Dictionary<E_PlayerInputState, InputUpdate> InputUpdates;
	Dictionary<E_PlayerInputState, FixedUpdate> FixedUpdates;
	Dictionary<E_GrapplingState, InputUpdate> GraplingUpdates;
	Rigidbody2D body;
	public Transform GrapplingHookBase;
	GameObject ActiveGrapplingHook;
	C_PendulumController PendulumController;
    LineRenderer RopeLine;

	PlayerController() {
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
		SetObject (new C_Player ());
		MainCameraController controller = Camera.main.GetComponent<MainCameraController> ();
		controller.player = this.gameObject;
		body = GetComponent<Rigidbody2D> ();
		Manager.ObjectLog.Add (gameObject, this);
		((C_Player)Object).GrapplingState = E_GrapplingState.Detached;
    }
	
	// Update is called once per frame
	void Update () {
		C_Player Player = Object as C_Player;
		InputUpdates [Player.PlayerInputState] ();
		if (Input.GetMouseButtonDown(0)) {
			if (ActiveGrapplingHook != null) {
				Player.GrapplingState = E_GrapplingState.Detached;
				if (Player.PlayerInputState == E_PlayerInputState.Swinging) {
					Player.PlayerInputState = E_PlayerInputState.Free;
				}
				GameObject.Destroy (ActiveGrapplingHook);
            }
            Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			float vx = v3.x - body.position.x;
			float vy = v3.y - body.position.y;
			double vangle = Trig.GetAngle (vx, vy);
			ActiveGrapplingHook = Instantiate (GrapplingHookBase, new Vector3 (body.position.x + Mathf.Cos ((float)vangle), body.position.y + Mathf.Sin ((float)vangle), 0), new Quaternion ()).gameObject;
			ActiveGrapplingHook.SetActive (true);
			ActiveGrapplingHook.GetComponent<Rigidbody2D>().velocity = new Vector2 (20 * Mathf.Cos((float)vangle), 20 * Mathf.Sin((float)vangle));
			ActiveGrapplingHook.GetComponent<C_WorldObjectController>().SetObject (new GrapplingHook (this));
		}

		if ((Input.GetMouseButtonDown(1)) && (Player.GrapplingState == E_GrapplingState.Attached)) {
			Player.GrapplingState = E_GrapplingState.Detached;
			GameObject.Destroy (ActiveGrapplingHook);
			if (Player.PlayerInputState == E_PlayerInputState.Swinging) {
				Player.PlayerInputState = E_PlayerInputState.Free;
			}
		} 
			
	}

	void GroundUpdate(){
		C_Player Player = Object as C_Player;
		if (Input.GetAxis("Vertical") > 0)
			body.AddForce (Vector2.up * -Physics.gravity.y, ForceMode2D.Impulse);
	}

	public void SwingingUpdate(){
		C_Player Player = Object as C_Player;
        if (PendulumController != null)
        {
            PendulumController.Update();
        }
	}

	void FreeUpdate() {
		
	}

	void FixedUpdate (){
		C_Player Player = Object as C_Player;
		RaycastHit2D[] lineGround = Physics2D.RaycastAll (new Vector2 (body.position.x, body.position.y), Vector2.down);
		RaycastHit2D nearestTile = lineGround[0];

        bool found = false;
		foreach (RaycastHit2D obj in lineGround) {
			if (Manager.ObjectLog [obj.collider.gameObject].Object.GetType () == typeof(Tile)) {
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
			if ((nearestTile.distance > 0.6) && (Player.PlayerInputState == E_PlayerInputState.Ground)) {
				Debug.Log ("Left Ground");
				if (Player.GrapplingState == E_GrapplingState.Attached)
					Player.PlayerInputState = E_PlayerInputState.Swinging;
				else if (Player.GrapplingState == E_GrapplingState.Detached) {
					Player.PlayerInputState = E_PlayerInputState.Free;
				}
			} else if ((nearestTile.distance <= 0.6) && (Player.PlayerInputState != E_PlayerInputState.Ground)) {
				Debug.Log ("Hit Ground");
				Player.PlayerInputState = E_PlayerInputState.Ground;
			}
		} else if (Player.PlayerInputState == E_PlayerInputState.Ground) {
			Debug.Log ("Left Ground");
			if (Player.GrapplingState == E_GrapplingState.Attached)
				Player.PlayerInputState = E_PlayerInputState.Swinging;
			else if (Player.GrapplingState == E_GrapplingState.Detached) {
				Player.PlayerInputState = E_PlayerInputState.Free;
			}
		}
		FixedUpdates [Player.PlayerInputState] ();
	}

	void SwitchMovState(E_PlayerMovState newState){

	}


	void GroundFixedUpdate() {
		C_Player Player = Object as C_Player;
		Vector3 direction = new Vector2 (Input.GetAxis ("Horizontal"), 0) * Player.Xaccel;
		body.AddForce(direction);
	}

	void FreeFixedUpdate() {

	}

	void SwingingFixedUpdate() {
            PendulumController.FixedUpdate();
	}

	public void CreateAnchor(float x, float y, LineRenderer rope){
		C_Player Player = Object as C_Player;
		PendulumController = new C_PendulumController (new Vector2 (x, y), Object as C_Player, body, ActiveGrapplingHook.GetComponent<GrapplingHookController>().Pivots);
        Player.GrapplingState = E_GrapplingState.Attached;
        if (Player.PlayerInputState == E_PlayerInputState.Free)
			Player.PlayerInputState = E_PlayerInputState.Swinging;
	}
}
