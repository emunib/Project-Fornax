using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public delegate void InputUpdate();
public delegate void FixedUpdate();
public enum E_PlayerInputState { Ground, Free, Swinging };
public enum E_GrapplingState { Attached, Detached };

public class C_PlayerController : C_WorldObjectController {
	Dictionary<E_PlayerInputState, InputUpdate> InputUpdates;
	Dictionary<E_PlayerInputState, FixedUpdate> FixedUpdates;
	Dictionary<E_GrapplingState, InputUpdate> GraplingUpdates;
	public Rigidbody2D body = null;
	public GameObject GrapplingHookBase;
	GameObject ActiveGrapplingHook;
	public C_PendulumController PendulumController;
    LineRenderer RopeLine;
    public Vector2 spawn; // I made this a Nullable type to get rid of the error in the OnEnable() method. https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/nullable-types/index
	public C_Controller PlayerInput;
	bool onSlope = false;
	public Animator anim;
    public int lives;
    public int kills;
    public GameObject lastHitBy;
    public GameObject gameManager;

	public float Xaccel = 25;
	public KeyCode XPos = KeyCode.RightArrow, XNeg = KeyCode.LeftArrow, YPos = KeyCode.UpArrow, YNeg = KeyCode.DownArrow;
	public float MaxSpeed;
	public float AngularAccel;
	public float RadiusDelta;
	public E_PlayerInputState PlayerInputState;
	public E_GrapplingState GrapplingState;

	//Need these to prevent infinite combos
	public bool ableToAttack = true;
	public bool ableToMove = true;


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
		body = GetComponent<Rigidbody2D> ();
		Manager.ObjectLog.Add (gameObject, this);
		GrapplingState = E_GrapplingState.Detached;
		InvokeRepeating ("MapCheck", 0f, 0.5f);

		anim = GetComponent<Animator> ();

        int playerID = PlayerManager.AddPlayer(this.gameObject);
        GetComponent<Renderer>().material = Resources.Load<Material>("PlayerMaterial_" + (playerID + 1)); // Loads appropriate material for player ID.
        PlayerInput = new C_Controller (playerID);  // Appropriate controls for player ID.

        gameManager = GameObject.FindWithTag("GameManager");
        lives = ModeSettings.numLives;
        kills = 0;
    }



	// Update is called once per frame
	void Update () {
		anim.SetFloat ("PlayerSpeed", body.velocity.magnitude);

		if (PlayerInput.GetButtonDown ("LightAttack1")) {

			if (ableToAttack == true && PlayerInputState == E_PlayerInputState.Ground) {

				anim.Play ("Punch");
				StartCoroutine(stopInput(0.5f));
		
			} 

		}

		if (PlayerInput.GetButtonDown ("LightAttack2")) {

			if (ableToAttack == true && PlayerInputState == E_PlayerInputState.Ground) {

				anim.Play ("Kick");
				StartCoroutine (stopInput (0.767f));
			}

		}


		if (PlayerInput.GetButtonDown("StrongAttack1")){


			if (ableToAttack == true && PlayerInputState == E_PlayerInputState.Ground) {

				anim.Play ("Palm Strike");
				StartCoroutine (stopInput (1.267f));
			}


			if (ableToAttack == true && PlayerInputState != E_PlayerInputState.Ground) {


				//Need a condition to stay in dive kick state until player hits ground 

				anim.Play ("Dive Kick");
				gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(0, -3000));
				StartCoroutine (stopInput (0.267f));

			}

		}

		if (PlayerInput.GetButtonDown("StrongAttack2")){


			//TODO, implement dash? it works but the direction doesnt seem to want to flip. Perhaps it is getting the wrong value for local scale?

			//if (ableToAttack == true && PlayerInputState == E_PlayerInputState.Ground) {

			//	anim.Play ("Dash");
			//	gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2((gameObject.transform.localScale.y*2500), 0));
			//	StartCoroutine (stopInput (0.667f));

			//}



			if (ableToAttack == true && PlayerInputState != E_PlayerInputState.Ground) {

				anim.Play ("Flying Knee");
				//StartCoroutine (stopInput (0.667f));
				anim.CrossFade ("Jumping Transition", 0.3F);
			}


		}

		if (PlayerInput.GetButtonDown ("Block")) {

			if(ableToAttack == true && PlayerInputState == E_PlayerInputState.Ground) {

				//Need a condition to stay in block state until player lets go

				anim.Play ("Block");
				StartCoroutine (stopInput (0.667f));
			}

		}


		InputUpdates [PlayerInputState] ();
		if (PlayerInput.GetButtonDown("Fire1")) {
			if (ActiveGrapplingHook != null) {
				GrapplingState = E_GrapplingState.Detached;
				if (PlayerInputState == E_PlayerInputState.Swinging) {
					PlayerInputState = E_PlayerInputState.Free;
				}
				GameObject.Destroy (ActiveGrapplingHook);
            }
			Vector2 dirVec = new Vector2 (PlayerInput.GetAxis ("Horizontal_r"), (PlayerInput.GetAxis ("Vertical_r")));
			dirVec.Normalize ();
			double vangle = Trig.GetAngle (dirVec);
			ActiveGrapplingHook = Instantiate (GrapplingHookBase, new Vector3 (body.position.x + Mathf.Cos ((float)vangle), body.position.y + Mathf.Sin ((float)vangle), 0), new Quaternion ()).gameObject;
			ActiveGrapplingHook.GetComponent<GrapplingHookController> ().SetPendulum (this);
			ActiveGrapplingHook.SetActive (true);
			ActiveGrapplingHook.GetComponent<Rigidbody2D>().velocity = new Vector2 (100 * dirVec.x, 100 * dirVec.y);
		}

		if ((PlayerInput.GetButtonDown("Fire2")) && (ActiveGrapplingHook != null)) {
			GrapplingState = E_GrapplingState.Detached;
			GameObject.Destroy (ActiveGrapplingHook);
			ActiveGrapplingHook = null;
			if (PlayerInputState == E_PlayerInputState.Swinging) {
				PlayerInputState = E_PlayerInputState.Free;
			}
		} 
			
	}

	void GroundUpdate(){
		if (PlayerInput.GetAxis ("Vertical") > 0) {
			body.AddForce ((Vector2.up * -Physics.gravity.y) / gameObject.transform.localScale.y, ForceMode2D.Impulse);

				
				anim.CrossFade ("Jumping Transition", 0.3F);
		



		}
	}

	public void SwingingUpdate(){
        if (PendulumController != null)
        {
            PendulumController.Update();
        }
	}

	void FreeUpdate() {
		
	}

	IEnumerator stopInput(float time){
		ableToAttack = false;
		ableToMove = false;
		yield return new WaitForSeconds (time);

		ableToAttack = true;
		ableToMove = true;
	}

	void FixedUpdate (){
        RaycastHit2D[] lineGroundDown = Physics2D.RaycastAll (new Vector2 (body.position.x, body.position.y), Vector2.down);
        var lineGroundRight = Physics2D.RaycastAll (new Vector2 (body.position.x, body.position.y), new Vector2(1, -3));
        var lineGroundLeft = Physics2D.RaycastAll (new Vector2 (body.position.x, body.position.y), new Vector2(-1, -3));
		var lineGround = lineGroundDown.Concat(lineGroundRight).Concat(lineGroundLeft).ToList();
		lineGround.Sort((x, y) => x.distance.CompareTo(y.distance));
		RaycastHit2D nearestTile = lineGround[0];

        bool found = false;
		foreach (RaycastHit2D obj in lineGround) {
			if (!Manager.ObjectLog.ContainsKey(obj.collider.gameObject)) {
				Debug.Log("Object not found");
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
			if ((nearestTile.distance > gameObject.transform.localScale.y * 0.6) && (PlayerInputState == E_PlayerInputState.Ground)) {
				LeftGround ();
			} else if ((nearestTile.distance <= gameObject.transform.localScale.y * 0.6) && (PlayerInputState != E_PlayerInputState.Ground)) {
				Debug.Log ("Hit Ground");
				PlayerInputState = E_PlayerInputState.Ground;
				anim.SetBool ("IsGrounded", true);
			}
		} else if (PlayerInputState == E_PlayerInputState.Ground) {
			LeftGround ();
		}
		FixedUpdates [PlayerInputState] ();
	}

	void LeftGround(){
		Debug.Log ("Left Ground");
		anim.CrossFade ("Jumping Transition", 0.3F);
		anim.SetBool ("IsGrounded", false);
		if (IsAttached())
			PlayerInputState = E_PlayerInputState.Swinging;
		else if (GrapplingState == E_GrapplingState.Detached) {
			PlayerInputState = E_PlayerInputState.Free;
		}
	}

	bool IsAttached(){
		return GrapplingState == E_GrapplingState.Attached;
	}


	void GroundFixedUpdate() {


		if (ableToMove == true) {

			if (onSlope) {
				body.AddForce (-Physics2D.gravity * Math.Abs (PlayerInput.GetAxis ("Horizontal")) * Xaccel);
			}

			// What if players could accelerate while in the air?
			Vector2 direction = new Vector2 (PlayerInput.GetAxis ("Horizontal"), 0) * Xaccel;
			if (PlayerInput.GetAxis ("Horizontal") > 0) {
				gameObject.transform.localScale = new Vector3 (-9, 9, 9);
			} else if (PlayerInput.GetAxis ("Horizontal") < 0) {
				gameObject.transform.localScale = new Vector3 (9, 9, 9);
			}
			body.AddForce (direction);

		}
	}

	void FreeFixedUpdate() {
		Vector2 direction = new Vector2 (PlayerInput.GetAxis ("Horizontal"), 0) * Xaccel/3;
		if (PlayerInput.GetAxis ("Horizontal") > 0) {
			gameObject.transform.localScale = new Vector3 (-9, 9, 9);
		} else if (PlayerInput.GetAxis ("Horizontal") < 0) {
			gameObject.transform.localScale = new Vector3 (9, 9, 9); 
		}
		body.AddForce(direction);


	}

	void SwingingFixedUpdate() {
    
	}

	public void CreateAnchor(){
        GrapplingState = E_GrapplingState.Attached;
        if (PlayerInputState == E_PlayerInputState.Free)
			PlayerInputState = E_PlayerInputState.Swinging;
	}

	public void MapCheck(){
		if ((body.position.x < - 50) || (body.position.x > LevelBuilder.width + 50) || (body.position.y < -50) || (body.position.y > LevelBuilder.height + 50)) {
			GameObject.Destroy (ActiveGrapplingHook);
			ActiveGrapplingHook = null;
            this.PlayerDied();
        }
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		
		if (other.gameObject.CompareTag("Slope"))
		{
			onSlope = true;
		}
		
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		
		if (other.gameObject.CompareTag("Slope"))
		{
			onSlope = false;
		}
		
	}

    private void PlayerDied()
    {
        if (lastHitBy != null)
        {
            lastHitBy.SendMessage("PlayerKilled");
        }
        gameManager.GetComponent<GameManager>().SendMessage("PlayerDied", this.gameObject);
    }

    private void PlayerKilled()
    {
        kills += 1;
    }

    // This will actually be called once before start
    private void OnEnable()
    {

    }
}