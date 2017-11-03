using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour {

    public GameObject player;       //Public variable to store a reference to the player game object
    bool playerSpawned = false;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(WaitForPlayer());
        offset.z = -10;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (playerSpawned) {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = player.transform.position + offset;
			if (transform.position.x < 0) {
				Vector3 temp = transform.position;
				temp.x = 0;
				transform.position = temp;
			}
			if (transform.position.x > LevelBuilder.width) {
				Vector3 temp = transform.position;
				temp.x = LevelBuilder.width;
				transform.position = temp;
			}
			if (transform.position.y < 0) {
				Vector3 temp = transform.position;
				temp.y = 0;
				transform.position = temp;
			}
			if (transform.position.y > LevelBuilder.height) {
				Vector3 temp = transform.position;
				temp.y = LevelBuilder.height;
				transform.position = temp;
			}
        }
    }

    /*
    IEnumerator WaitForPlayer()
    {
        yield return new WaitForSeconds(1);
        playerSpawned = true;
    }
    */

    // Later this MUST BE CHANGED so that the game manager sends this function a message when all players are spawned.
    void PlayerSpawned(GameObject obj)
    {
        Debug.Log("Player spawned");
        player = obj;
        playerSpawned = true;
    }

}
