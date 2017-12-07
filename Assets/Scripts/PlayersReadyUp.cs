using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Timers;
using InControl;

public class PlayersReadyUp : MonoBehaviour {

    string[] controllers;
    List<int> playersReady = new List<int>();

    public GameObject nextPanel;
    public Text Instructions;

	public InputDevice pInput;

    float timer;
    float countdown = 10f;

    // Use this for initialization
    void Start () {
        Instructions = this.transform.Find("Instruction").GetComponent<Text>();
        timer = countdown;
        Time.timeScale = 1.6f;  // For some reason, default runs too fast.



    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // Get list of names of controllers.
        controllers = Input.GetJoystickNames();

        // Get Fire1 input from each controller.
		for (int i = 0; i < InputManager.Devices.Count; i++)
        {
			if (InputManager.Devices[i].GetControl(InputControlType.RightTrigger))
            {

                timer = countdown;
                // Add players to ready list if they are not in it, otherwise remove it.
                if (!playersReady.Contains(i))
                {
                    playersReady.Add(i);
                    this.transform.Find("Player" + i).gameObject.SetActive(true);
                }
                else
                {
                    Instructions.text = "Press R1 to ready up";
                    playersReady.Remove(i);
                    this.transform.Find("Player" + i).gameObject.SetActive(false);
                }
            }
        }
        // Check if everyone is ready, if true: save number of players, and change panels to mode selection.
        if (playersReady.Count == controllers.Length)
        {
            timer = timer - Time.deltaTime;
            //StartCoroutine("Countdown");
            Instructions.text = timer.ToString("F2"); // Changes text to timer with 2 decimal places.
            if (timer < 0)
            {
                ChangeToModeSelect();
            }
        }
    }

    // Function is called when object becomes enabled or active.
    private void OnEnable()
    {
        // Reset timer.
        timer = countdown;
    }

    void ChangeToModeSelect()
    {
        ModeSettings.numPlayers = controllers.Length;
        nextPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
