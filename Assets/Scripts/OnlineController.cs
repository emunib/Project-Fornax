using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections.Generic;
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

	public Button GuestSigninButton;
	
    public GameObject GameLobby;
    public GameObject CreateNew;
    public GameObject Login;

    private Poster<createUserResponse, userLogin> createNewUser;
    private Poster<loginResponse, userLogin> loginService;
    
    private LobbyListenerImpl lobbyListener;
    private string[] args = new string[0];
	// Use this for initialization

	void Start () {
        
        LoginUsername = GameObject.Find("/Canvas/Online/OnlineLogin/UsernameField").GetComponent<InputField>();
        LoginButton = GameObject.Find("/Canvas/Online/OnlineLogin/LoginButton").GetComponent<Button>();
        LoginButton.onClick.AddListener(LoginClick);

        CreateNewButton = GameObject.Find("/Canvas/Online/OnlineCreateUser/CreateNewButton").GetComponent<Button>();
        CreateNewButton.onClick.AddListener(CreateNewClick);
		
		GuestSigninButton = GameObject.Find("/Canvas/Online/OnlineLogin/GuestSigninButton").GetComponent<Button>();
		GuestSigninButton.onClick.AddListener(GuestSigninClick);

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
        createNewUser =  new Poster<createUserResponse, userLogin>(OnlineManager.ServiceUrl + OnlineManager.UsersUrl, type =>
        {
	        if (type.response.Equals(CreateUserCode.SUCCESS))
	        {
		        OnlineManager.Player = type.sessionInfo;
		        if (CreateNew.activeSelf)
		        {
			        CreateNew.SetActive(false);
		        }
		        else if (Login.activeSelf)
		        {
			        Login.SetActive(false);
		        }
		        GameLobby.SetActive(true);
	        }
	        else
	        {
		        CreateNewUsername.text = "Create User failed";
		        Debug.Log(type.response.ToString());
	        }
        });
	    
	    loginService = new Poster<loginResponse, userLogin>(OnlineManager.ServiceUrl + OnlineManager.UsersUrl + "/{userId}" + OnlineManager.SessionUrl, type =>
	    {
		    if (type.response.Equals(LoginCode.SUCCESS))
		    {
			    OnlineManager.Player = type.sessionInfo;
				Login.SetActive(false);
			    GameLobby.SetActive(true);   
		    }
		    else
		    {
			    LoginUsername.text = "Login failed";
				Debug.Log(type.response.ToString());
		    }
	    } );
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void GuestSigninClick()
	{
		var userLogin = new userLogin();
		userLogin.username = "guest";
		createNewUser.Run(userLogin);
	}

    public void LoginClick()
    {
	    var userLogin = new userLogin();
	    userLogin.password = LoginPassword.text;
	    userLogin.username = LoginUsername.text;
	    var dictionary = new Dictionary<string, string>();
	    dictionary.Add("userId", LoginUsername.text);
	    loginService.SetUrlVariables(dictionary);
	    loginService.Run(userLogin);
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
