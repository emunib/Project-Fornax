using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{

	// LateUpdate is called after Update each frame
	void LateUpdate()
	{
		Vector2 playermin = new Vector2(float.MaxValue, float.MaxValue);
		Vector2 playermax = new Vector2(float.MinValue, float.MinValue);

		// find the greatest and lowest player x and y coordinates
		foreach (C_PlayerController player in PlayerManager.PlayerLog)
		{
			float x = player.body.position.x;
			float y = player.body.position.y;

			playermin.x = Mathf.Min(playermin.x, x);
			playermin.y = Mathf.Min(playermin.y, y);
			playermax.x = Mathf.Max(playermax.x, x);
			playermax.y = Mathf.Max(playermax.y, y);
		}
		// you can change playermin.y to the y coordinate of the lowest platform and the camera
		// will keep it view, the camera will zoom 'upwards' with the bottom fixed to the platform

		// differnce between the two positions
		var diff = playermax - playermin;
		
		// the center between the two positions
		var pos = (playermax + playermin) / 2;

		// resize camera to fit all players with a bit to spare
		Camera.main.orthographicSize = Mathf.Max(diff.x/Camera.main.aspect, diff.y)/2 + 6;
		
		// reposition the camera in the center of players
		transform.position = new Vector3(pos.x, pos.y, -10);
	}
}
	
