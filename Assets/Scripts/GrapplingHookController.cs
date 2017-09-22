using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookController : C_WorldObjectController {
	LineRenderer RopeLine;
	Rigidbody2D Body;
	Stack<Pivot> Pivots;

	// Use this for initialization
	void Start () {
		RopeLine = gameObject.GetComponent<LineRenderer>();
		Body = gameObject.GetComponent<Rigidbody2D> () ;
		Manager.ObjectLog.Add (gameObject, this);
		Pivots = new Stack<Pivot>();

	}

	void OnDestroy () {
		Manager.ObjectLog.Remove (gameObject);
	}

	// Update is called once per frame
	void Update () {
		GrapplingHook hook = Object as GrapplingHook;
		RopeLine.SetPosition (0, new Vector3 (Body.position.x, Body.position.y));
		RopeLine.SetPosition (1, new Vector3 (hook.PlayerBody.position.x, hook.PlayerBody.position.y));
		Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0));
		if ((screenPos.x < Camera.main.pixelRect.xMin) || (screenPos.x > Camera.main.pixelRect.xMax) || (screenPos.y < Camera.main.pixelRect.yMin) || (screenPos.y > Camera.main.pixelRect.yMax)) {
			GameObject.Destroy (this.gameObject);
		}

	}

	void OnCollisionEnter2D(Collision2D collision){
		if ( Manager.ObjectLog[collision.gameObject].Object.GetType() == typeof(Tile)) {
			GrapplingHook hook = Object as GrapplingHook;
			hook.PlayerObject.CreateAnchor (Body.position.x, Body.position.y);
            Body.bodyType = RigidbodyType2D.Static;
            //GameObject.Destroy (this.gameObject);
		}
	}
}
