﻿using UnityEngine.UI;
using UnityEngine;
using System;
using System.Net;
using System.Threading.Tasks;
using Schemas;

public class OnlineController : MonoBehaviour {
    public UnityEngine.UI.InputField LoginUsername;
    public UnityEngine.UI.InputField LoginPassword;
    public Button LoginButton;

    public UnityEngine.UI.InputField CreateNewUsername;
    public UnityEngine.UI.InputField CreateNewPassword;
    public UnityEngine.UI.InputField CreateNewConfirmPassword;
    public Button CreateNewButton;
    private Task<sessionInfo> restcall;

    public GameObject GameLobby;
    public GameObject CreateNew;
    public GameObject Login;

    private GenericPoster<sessionInfo, userLogin> createNewUser;
    private GenericPoster<sessionInfo, userLogin> loginService;
    
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
            //playerRegister = gameRegister.Connect(OnlineManager.LobbyLstnrProxy);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
        createNewUser =  new GenericPoster<sessionInfo, userLogin>(OnlineManager.ServiceUrl + OnlineManager.UsersUrl, type =>
        {
            OnlineManager.Player = type;
            if (OnlineManager.Player.publicID == "")
            {
                CreateNewUsername.text = "Create User failed";
            }
            else
            {
                CreateNew.SetActive(false);
                GameLobby.SetActive(true);
            }
        });
	    
	    loginService = new GenericPoster<sessionInfo, userLogin>(OnlineManager.ServiceUrl + OnlineManager.UsersUrl, type =>
	    {
	        OnlineManager.Player = type;
	        if (OnlineManager.Player.publicID == "")
	        {
	            LoginUsername.text = "Login failed";
	        }
	        else
	        {
	            Login.SetActive(false);
	            GameLobby.SetActive(true);
	        }
	    } );
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void LoginClick()
    {
	    var userLogin = new userLogin();
	    userLogin.password = LoginPassword.text;
	    userLogin.username = LoginUsername.text;
	    loginService.Run(userLogin, "/" + LoginUsername.text + OnlineManager.SessionUrl);
    }


    public void CreateNewClick()
    {
        if (CreateNewPassword.text == CreateNewConfirmPassword.text)
        {
            var userLogin = new userLogin();
            userLogin.password = CreateNewPassword.text;
            userLogin.username = CreateNewUsername.text;
            createNewUser.Run(userLogin);
        }
    }
}
