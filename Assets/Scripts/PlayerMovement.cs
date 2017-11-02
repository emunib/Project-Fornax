using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

   
	float speed = 50.0f;
    float jumpspeed = 2.0f;

	Rigidbody2D player;


    // Use this for initialization
    void Start () {

		player = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {

        
	}



	void FixedUpdate(){


        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalMovement, 0.0f, verticalMovement);

        player.AddForce(direction * speed);

        if (Input.GetButton("Jump"))
        {
            player.AddForce(Vector2.up*jumpspeed, ForceMode2D.Impulse);

        }
       






	}

}
