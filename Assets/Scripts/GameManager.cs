using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;

public class PlayerManager {
	public static List<GameObject> PlayerLog = new List<GameObject> ();
	public static System.Threading.Mutex myMutex = new Mutex();

	public static int AddPlayer(GameObject player){
		int result = -1;
		myMutex.WaitOne ();
		if (PlayerLog.Contains(player)){
			throw new Exception("Player already in log");
		} else {
			result = PlayerLog.Count;
			PlayerLog.Add(player);
		}
		myMutex.ReleaseMutex ();
		return result;
	}
}

public class Manager {
	public static Dictionary<GameObject, C_WorldObjectController> ObjectLog = new Dictionary<GameObject, C_WorldObjectController> ();
}

public class C_WorldObjectController : MonoBehaviour {
	
}


public class GameManager : MonoBehaviour {

    public void PlayerDied(GameObject player)
    {
        // Deactivate player object.
        StartCoroutine(DespawnPlayer(player));

        // If they have lives left, decrement their lives and respawn them.
        if (player.GetComponent<C_PlayerController>().lives > 0)
        {
            player.GetComponent<C_PlayerController>().lives = player.GetComponent<C_PlayerController>().lives - 1;
            player.GetComponent<C_PlayerController>().body.position = player.GetComponent<C_PlayerController>().spawn;
            player.GetComponent<C_PlayerController>().body.velocity = new Vector2(0, 0);
            // Delayed respawn
            StartCoroutine(SpawnPlayer(player));
        }
        else
        {
            // Check for other player's lives in PlayerLog
            // Depending on mode (practice, FFA or 2v2), if only one team has lives left, then declare winner.
            // Also make sure to clear PlayerLog since static fields are persistent across games.
            List<GameObject> PlayersAlive = new List<GameObject>();

            switch (ModeSettings.modeType) {
                case ModeSettings.Modes.FFA:
                    for (int i = 0; i > PlayerManager.PlayerLog.Count; i++)
                    {
                        if (PlayerManager.PlayerLog[i].GetComponent<C_PlayerController>().lives > 0)
                        {
                            PlayersAlive.Add(PlayerManager.PlayerLog[i]);
                        }
                    }
                    if (PlayersAlive.Count == 1)
                    {
                        // Declare winner.
                    }
                    break;

                case ModeSettings.Modes.VERSUS:
                    int TeamsAlive = 0;

                    for (int i = 0; i > PlayerManager.PlayerLog.Count; i = i+2) {
                        if (PlayerManager.PlayerLog[i].GetComponent<C_PlayerController>().lives > 0 || PlayerManager.PlayerLog[i + 1].GetComponent<C_PlayerController>().lives > 0)
                        {
                            TeamsAlive += 1;
                        }
                    }
                    if (TeamsAlive == 1)
                    {
                        // Declare winner.
                    }
                    break;

                case ModeSettings.Modes.PRACTICE:
                    // Assume player just exits practice mode through pause menu.
                    /*
                    for (int i = 0; i > PlayerManager.PlayerLog.Count; i++)
                    {
                        if (PlayerManager.PlayerLog[i].GetComponent<C_PlayerController>().lives > 0)
                        {
                            PlayersAlive.Add(PlayerManager.PlayerLog[i]);
                        }
                    }
                    if (PlayersAlive.Count < 1)
                    {
                        // End game.
                    }
                    */
                    break;
            }
        }
    }

    IEnumerator DespawnPlayer(GameObject player)
    {
        yield return new WaitForSeconds(0.1f); // Necessary because setActive will occur before the physics step preventing some variables like rigidbody.position from updating.
        // Deactivate player object.
        player.SetActive(false);
    }

    IEnumerator SpawnPlayer(GameObject player)
    {
        yield return new WaitForSeconds(6);  // the "seconds" argument scales by time.TimeScale
        player.SetActive(true);
    }

}