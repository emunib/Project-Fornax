using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookController : C_WorldObjectController {
	LineRenderer RopeLine;
	Rigidbody2D Body;
	public List<Pivot> Pivots;
	public C_PlayerController PlayerObject;
	public Rigidbody2D PlayerBody;
	bool Active;
	// Use this for initialization

	void Start () {
		RopeLine = gameObject.GetComponent<LineRenderer>();
		Body = gameObject.GetComponent<Rigidbody2D> () ;
		
		Manager.ObjectLog.Add (gameObject, this);
		Pivots = new List<Pivot>();
		Pivots.Add (new Pivot (Body.position));
		Active = true;
	}

	public void SetPendulum(C_PlayerController player){
		PlayerObject = player;
		PlayerBody = player.gameObject.GetComponent<Rigidbody2D> ();
	}

	void OnDestroy () {
		Manager.ObjectLog.Remove (gameObject);
	}

	// Update is called once per frame
	void Update () {
		if (Active) {
			Pivots [0].Position = Body.position;
		} 
		RopeLine.positionCount = Pivots.Count + 1;
		RopeLine.SetPosition (0, new Vector3(PlayerBody.position.x, PlayerBody.position.y));
		int i = Pivots.Count;
		foreach (Pivot pivot in Pivots){
			RopeLine.SetPosition (i--, new Vector3(pivot.Position.x, pivot.Position.y));
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if ( Manager.ObjectLog[collision.gameObject].GetType() == typeof(TileController)) {
			Vector2 direction = collision.contacts[0].point -  Body.position;
			direction = -direction/direction.magnitude;
			Pivots [0].Position = collision.contacts[0].point + (direction * 0.1f);
			PlayerObject.CreateAnchor (Pivots [0].Position.x, Pivots [0].Position.y, RopeLine);
			Body.simulated = false;
			Active = false;
            //GameObject.Destroy (this.gameObject);
		}
	}
}
