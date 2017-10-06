﻿using System;
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
        StartCoroutine(WaitForPlayer());
        offset.z = -10;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (playerSpawned) {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = player.transform.position + offset;
        }
    }

    IEnumerator WaitForPlayer()
    {
        yield return new WaitForSeconds(1);
        playerSpawned = true;
    }

}
