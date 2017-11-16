using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Online;
using Ice;
using System;


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

    private GameRegisterPrx gameRegister;
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
            Ice.Communicator communicator = Ice.Util.initialize(ref args);
            OnlineManager.Adapater = communicator.createObjectAdapterWithEndpoints("Test", "tcp -h 127.0.0.1 -p 10001");
            Ice.ObjectPrx obj = communicator.stringToProxy("SimplePrinter:tcp -h 127.0.0.1 -p 10000");
            gameRegister = GameRegisterPrxHelper.checkedCast(obj);
            if (gameRegister == null)
            {
                throw new ApplicationException("Invalid proxy");
            }
            OnlineManager.LobbyLstnrImpl = lobbyListener;
            OnlineManager.LobbyLstnrProxy = LobbyListenerPrxHelper.checkedCast(OnlineManager.Adapater.addWithUUID(lobbyListener));
            OnlineManager.Adapater.activate();
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
        Debug.Log(LoginUsername.text + " " + LoginPassword.text);
        OnlineManager.Player = gameRegister.Login(LoginUsername.text,
                                    LoginPassword.text,
                                                  OnlineManager.LobbyLstnrProxy,
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
            OnlineManager.Player = gameRegister.CreateNew(CreateNewUsername.text, CreateNewPassword.text,
                                                          OnlineManager.LobbyLstnrProxy,
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
