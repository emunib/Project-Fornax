using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour {


    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start()
    {
        offset.z = -10;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {

		Vector3 min = Camera.main.ScreenToWorldPoint (Camera.main.pixelRect.min);
		Vector3 max = Camera.main.ScreenToWorldPoint (Camera.main.pixelRect.max); 
		Vector2 position = new Vector2(0,0);
		bool shouldIncrease = false;
		bool shouldDecrease = false;
		Vector2 playermin = new Vector2 (float.MaxValue, float.MaxValue);
		Vector2 playermax = new Vector2(float.MinValue , float.MinValue);
		foreach (C_PlayerController player in PlayerManager.PlayerLog) {
			float x = player.body.position.x;
			float y = player.body.position.y;
			position += player.body.position;
			playermin.x = Mathf.Min (playermin.x, x);
			playermin.y = Mathf.Min (playermin.y, y);
			playermax.x = Mathf.Max (playermax.x, x);
			playermax.y = Mathf.Max (playermax.y, y);
		}

		if ((playermin.x < (min.x * 1.1f))) {
			shouldIncrease = true;
		} else if ((playermin.x - min.x) > 0.4 * (max.x - min.x)) {
			shouldDecrease = true;
		}

		if (playermin.y < (min.y * 1.1f)) {
				shouldIncrease = true;
		} else if ((playermin.y - min.y) > 0.4 * (max.y - min.y)) {
				shouldDecrease = true;
		}

		if ((playermax.x > (max.x * 0.9f)) ) {
			shouldIncrease = true;
		} else if ((max.x - playermax.x) > 0.4 * (max.x - min.x)) {
			shouldDecrease = true;
		}

		if (playermax.y > (max.y * 0.9f)) {
			shouldIncrease = true;
		} else if ((max.y - playermax.y) > 0.4 * (max.y - min.y)) {
			shouldDecrease = true;
		}
	

		if (shouldIncrease) {
			Camera.main.orthographicSize *= 1.02f;
		} else if (shouldDecrease) {
			if (Camera.main.orthographicSize > 10) {
				Camera.main.orthographicSize *= 0.99f;
			}
		}

		position = position / PlayerManager.PlayerLog.Count;




			
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		transform.position = new Vector3(position.x, position.y) + offset;
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
	
