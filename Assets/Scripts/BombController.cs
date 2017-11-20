using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.rigidbody.AddForce((other.contacts[0].point-new Vector2(transform.position.x, transform.position.y)).normalized * 50, ForceMode2D.Impulse);
			Destroy(gameObject);
		}
	}
}
