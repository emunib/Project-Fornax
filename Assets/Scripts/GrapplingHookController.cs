using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookController : C_WorldObjectController {
	LineRenderer RopeLine;
	Rigidbody2D Body;
	public List<Pivot> Pivots;
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

	void OnDestroy () {
		Manager.ObjectLog.Remove (gameObject);
	}

	// Update is called once per frame
	void Update () {
		GrapplingHook hook = Object as GrapplingHook;
		if (Active) {
			Pivots [0].Position = Body.position;
		}
		RopeLine.positionCount = Pivots.Count + 1;
		RopeLine.SetPosition (0, new Vector3(hook.PlayerBody.position.x, hook.PlayerBody.position.y));
		int i = Pivots.Count;
		foreach (Pivot pivot in Pivots){
			RopeLine.SetPosition (i--, new Vector3(pivot.Position.x, pivot.Position.y));
		}
		Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0));
		if ((screenPos.x < Camera.main.pixelRect.xMin) || (screenPos.x > Camera.main.pixelRect.xMax) || (screenPos.y < Camera.main.pixelRect.yMin) || (screenPos.y > Camera.main.pixelRect.yMax)) {
			GameObject.Destroy (this.gameObject);
		}

	}

	void OnCollisionEnter2D(Collision2D collision){
		if ( Manager.ObjectLog[collision.gameObject].Object.GetType() == typeof(Tile)) {
			GrapplingHook hook = Object as GrapplingHook;
			Vector2 direction = collision.contacts[0].point -  Body.position;
			direction = -direction/direction.magnitude;
			Pivots [0].Position = collision.contacts[0].point + (direction * 0.1f);
			hook.PlayerObject.CreateAnchor (Pivots [0].Position.x, Pivots [0].Position.y, RopeLine);
			Body.simulated = false;
			Active = false;
            //GameObject.Destroy (this.gameObject);
		}
	}
}
