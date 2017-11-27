using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : C_WorldObjectController
{
	void Start () {
		Manager.ObjectLog.Add (gameObject, this);
	}
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			GetComponent<Animator>().SetBool("explode", true);
			Expolode();
			Destroy(GetComponent<Collider2D>());
		}
	}

	private void Expolode()
	{
		var explosionRadius = 15;
		var explosionForce = 50;
		var players = GameObject.FindGameObjectsWithTag("Player");

		foreach (var player in players)
		{
			var dir = (player.transform.position - transform.position);
			float wearoff = 1 - dir.magnitude / explosionRadius;
			if (wearoff < 0) wearoff = 0;
			player.GetComponent<Rigidbody2D>().AddForce(dir.normalized * explosionForce * wearoff, ForceMode2D.Impulse);
		}
	}
	
	private void Die()
	{
		Destroy(gameObject);
	}
}
