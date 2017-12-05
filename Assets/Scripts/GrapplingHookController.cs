using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookController : C_WorldObjectController {
	public LineRenderer RopeLine;
	public Rigidbody2D Body;
	public List<Pivot> Pivots;
	public C_PlayerController PlayerObject;
	public Rigidbody2D PlayerBody;
	// Use this for initialization

	void Start () {
		RopeLine = gameObject.GetComponent<LineRenderer>();
		Body = gameObject.GetComponent<Rigidbody2D> () ;

		Manager.ObjectLog.Add (gameObject, this);
		Pivots = new List<Pivot>();
		Pivots.Add(new Pivot(PlayerBody.position));
		PlayerObject.PendulumController = new C_PendulumController (PlayerObject,this);
	}

	public void SetPendulum(C_PlayerController player){
		PlayerObject = player;
		PlayerBody = player.gameObject.GetComponent<Rigidbody2D> ();
	}

	void OnDestroy () {
		Manager.ObjectLog.Remove (gameObject);
	}

	void FixedUpdate (){
		PlayerObject.PendulumController.FixedUpdate ();
	}

	// Update is called once per frame
	void Update () {
		PlayerObject.PendulumController.Draw ();
	}

	void OnCollisionEnter2D(Collision2D collision){
		if ( Manager.ObjectLog[collision.gameObject].GetType() == typeof(TileController)) {
			Body.simulated = false;
			Collider2D collidee = collision.collider;
			Vector2 directionVec = (PlayerBody.position - new Vector2 (collidee.bounds.center.x, collidee.bounds.center.y));
			directionVec.Normalize ();
			Vector2 collisionPoint = collision.contacts[0].point + (0.1f * directionVec);
			while (collidee.bounds.Contains (collisionPoint)) {
				collisionPoint += 0.1f * directionVec;
			} 
				
			Body.position = collisionPoint;
			PlayerObject.PendulumController.Switch ();
			PlayerObject.CreateAnchor ();
            
		}
	}
}
