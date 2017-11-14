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

    public UnityEngine.UI.InputField CreateNewUsername;
    public UnityEngine.UI.InputField CreateNewPassword;
    public UnityEngine.UI.InputField CreateNewConfirmPassword;

    public GameObject GameLobby;
    public GameObject CreateNew;
    public GameObject Login;

    private GameRegisterPrx gameRegister;
    private LobbyListenerImpl lobbyListener;
    private LobbyListenerPrx lobbyProxy;
    private ObjectAdapter Adapater;
    private string[] args = new string[0];
    private PlayerPrx Player;
	// Use this for initialization

	void Start () {
        lobbyListener = new LobbyListenerImpl();
        try
        {
            Ice.Communicator communicator = Ice.Util.initialize(ref args);
            Adapater = communicator.createObjectAdapterWithEndpoints("Test", "tcp -h 127.0.0.1 -p 10001");
            Ice.ObjectPrx obj = communicator.stringToProxy("SimplePrinter:tcp -h 127.0.0.1 -p 10000");
            gameRegister = GameRegisterPrxHelper.checkedCast(obj);
            if (gameRegister == null)
            {
                throw new ApplicationException("Invalid proxy");
            }
            lobbyProxy = LobbyListenerPrxHelper.checkedCast(Adapater.addWithUUID(lobbyListener));
            Adapater.activate();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoginClick(){
        Debug.Log(LoginUsername.text + " " + LoginPassword.text);
        Player = gameRegister.Login(LoginUsername.text,
                           LoginPassword.text,
                           lobbyProxy,
                           null);
        if (Player == null){
            LoginUsername.text = "Login failed";
        } else {
            Login.SetActive(false);
            GameLobby.SetActive(true);
            GameLobby.GetComponent<GameRegistryController>().Player = Player;
        }
    }


    public void CreateNewClick()
    {
        if (CreateNewPassword.text == CreateNewConfirmPassword.text)
        {
            Player = gameRegister.CreateNew(CreateNewUsername.text, CreateNewPassword.text,
                               lobbyProxy,
                               null);
            if (Player == null)
            {
                CreateNewUsername.text = "Login failed";
            }
            else
            {
                CreateNew.SetActive(false);
                GameLobby.SetActive(true);
                GameLobby.GetComponent<GameRegistryController>().Player = Player;
            }
        }
    }




}
