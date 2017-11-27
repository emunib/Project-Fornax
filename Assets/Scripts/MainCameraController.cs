using System;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private Vector2 max = new Vector2(float.MinValue, float.MinValue);
    private Vector2 min = new Vector2(float.MaxValue, float.MaxValue);

    private void Start()
    {
        // find the extents of the level
        foreach (var obj in GameObject.FindGameObjectsWithTag("Ground"))
        {
            float x = obj.GetComponent<Transform>().position.x;
            float y = obj.GetComponent<Transform>().position.y;

            min.x = Mathf.Min(min.x, x);
            min.y = Mathf.Min(min.y, y);
            max.x = Mathf.Max(max.x, x);
            max.y = Mathf.Max(max.y, y);
        }
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        Vector2 playermax = new Vector2(float.MinValue, float.MinValue);
        Vector2 playermin = new Vector2(float.MaxValue, float.MaxValue);

        var allDead = true;
        // find the greatest and lowest player x and y coordinates
        foreach (GameObject player in PlayerManager.PlayerLog)
        {
            if (player.activeSelf) // If active and not dead.
            {
                allDead = false; // at least one player is live
                float x = player.GetComponent<C_PlayerController>().body.position.x;
                float y = player.GetComponent<C_PlayerController>().body.position.y;

                playermin.x = Mathf.Min(playermin.x, x);
                playermin.y = Mathf.Min(playermin.y, y);
                playermax.x = Mathf.Max(playermax.x, x);
                playermax.y = Mathf.Max(playermax.y, y);
            }
        }
        
        // if there are no players fit camera to the platforms
        if (PlayerManager.PlayerLog.Count == 0 || allDead)
        {
            playermax = max;
            playermin = min;
        }
        else
        {   // if the player is outside of the level area keep the camera fit to the platforms
            playermin.x = Mathf.Max(playermin.x, min.x);
            playermin.y = Mathf.Max(playermin.y, min.y);
            playermax.x = Mathf.Min(playermax.x, max.x);
            playermax.y = Mathf.Min(playermax.y, max.y);
        }

        // you can change playermin.y to the y coordinate of the lowest platform and the camera
        // will keep it view, the camera will zoom out 'upwards' with the bottom edge fixed to the platform

        // differnce between the two positions
        var diff = playermax - playermin;

        // the center between the two positions
        var pos = (playermax + playermin) / 2;

        // the distance the camera is off center of the two positions
        var offset = new Vector2(transform.position.x, transform.position.y) - pos;
        offset = new Vector2(Math.Abs(offset.y), Math.Abs(offset.y));

        // resize camera to fit all players with a bit to spare
        // offset is taken into account since players may cut off if the camera if off center
        // this if compensated for by zooming out more if required
        var spacing = 15f;
        var targetValue = Mathf.Max(diff.x / Camera.main.aspect, diff.y) / 2 +
                          Mathf.Max(offset.x / Camera.main.aspect, offset.y + spacing);
        var zoomSpeed = 0f;

        // zoom out quick, zoom in a bit more slowly
        var smoothTime = targetValue > Camera.main.orthographicSize ? .08f : .25f;

        // gradaully resize camera
        Camera.main.orthographicSize =
            Mathf.SmoothDamp(Camera.main.orthographicSize, targetValue, ref zoomSpeed, smoothTime);

        // gradually move camera to the center of players
        Vector3 speed = Vector3.zero;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, new Vector3(pos.x, pos.y, -10), ref speed, 0.1f);
        transform.position = newPos;
    }
}