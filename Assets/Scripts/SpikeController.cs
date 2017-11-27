using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : C_WorldObjectController
{
	void Start () {
		Manager.ObjectLog.Add (gameObject, this);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.SendMessage("Die");
		}
	}
}
