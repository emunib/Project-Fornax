using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class OnlineController : MonoBehaviour {
    public UnityEngine.UI.InputField LoginUsername;
    public UnityEngine.UI.InputField LoginPassword;
    public Button LoginButton;

    public UnityEngine.UI.InputField CreateNewUsername;
    public UnityEngine.UI.InputField CreateNewPassword;
    public UnityEngine.UI.InputField CreateNewConfirmPassword;
    public Button CreateNewButton;

    public GameObject GameLobby;
    public GameObject CreateNew;
    public GameObject Login;

    private PlayerRegisterPrx playerRegister;
    private LobbyListenerImpl lobbyListener;
    private string[] args = new string[0];
	// Use this for initialization

	void Start () {
        
        LoginUsername = GameObject.Find("/Canvas/Online/OnlineLogin/UsernameField").GetComponent<InputField>();
        LoginButton = GameObject.Find("/Canvas/Online/OnlineLogin/LoginButton").GetComponent<Button>();
        LoginButton.onClick.AddListener(LoginClick);

        CreateNewButton = GameObject.Find("/Canvas/Online/OnlineCreateUser/CreateNewButton").GetComponent<Button>();
        CreateNewButton.onClick.AddListener(CreateNewClick);

        lobbyListener = new LobbyListenerImpl();
        try
        {
            Debug.Log(IPAddress.Any.ToString());

            OnlineManager.LobbyLstnrImpl = lobbyListener;
            OnlineManager.Adapater.activate();
            //playerRegister = gameRegister.Connect(OnlineManager.LobbyLstnrProxy);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoginClick() {
        OnlineManager.Player = playerRegister.Login(LoginUsername.text,
                                    LoginPassword.text,
                                    null);
        if (OnlineManager.Player == null){
            LoginUsername.text = "Login failed";
        } else {
            Login.SetActive(false);
            GameLobby.SetActive(true);
        }
    }


    public void CreateNewClick()
    {
        if (CreateNewPassword.text == CreateNewConfirmPassword.text)
        {
            OnlineManager.Player = playerRegister.CreateNew(CreateNewUsername.text, CreateNewPassword.text,
                               null);
            if (OnlineManager.Player == null)
            {
                CreateNewUsername.text = "Login failed";
            }
            else
            {
                CreateNew.SetActive(false);
                GameLobby.SetActive(true);
            }
        }
    }




}
