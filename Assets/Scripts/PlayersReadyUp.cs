using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersReadyUp : MonoBehaviour {

    string[] controllers;
    List<int> playersReady = new List<int>();
    public GameObject nextPanel;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update()
    {
        // Get list of names of controllers.
        controllers = Input.GetJoystickNames();

        // Get Fire1 input from each controller.
        for (int i = 0; i < controllers.Length; i++)
        {
            if (Input.GetButtonDown("Fire1" + "_" + i))
            {
                // Add players to ready list if they are not in it, otherwise remove it.
                if (!playersReady.Contains(i))
                {
                    playersReady.Add(i);
                    this.transform.Find("Player" + i).gameObject.SetActive(true);
                }
                else
                {
                    playersReady.Remove(i);
                    this.transform.Find("Player" + i).gameObject.SetActive(false);
                }

                // Check if everyone is ready, if true: save number of players, and change panels to mode selection.
                if (playersReady.Count == controllers.Length)
                {
                    ChangeToModeSelect();
                }
            }
        }
    }


    void ChangeToModeSelect()
    {
        ModeSettings.numPlayers = controllers.Length;
        nextPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
