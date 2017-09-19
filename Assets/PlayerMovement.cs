using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {


	float speed = 2000.0f;

	Rigidbody2D player;

	// Use this for initialization
	void Start () {

		player = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {
	

		//var move = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0);
		//transform.position += move * speed * Time.deltaTime;

	}


	void FixedUpdate(){


		player.velocity = new Vector2 (Input.GetAxis ("Horizontal") * speed * Time.deltaTime, player.velocity.y);



	}

}
