using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Schemas;
public class GameHostLobbyController : MonoBehaviour {
    Dictionary<userPublic, GameObject> Gameset;
    private Button LeaveButton;
    private Button StartButton;
    private Button LockRoomButton;
	
	public GameObject text;

    private Getter<publicGameInfo> GetUsersService;
	private Deleter LeaveRoomService;
	private Deleter KickPlayerService;

    public GameObject Panel;
	// Use this for initialization
	void Start () {
        Panel = Resources.Load("GamePanel") as GameObject;
        if (Panel == null)
        {
            throw new System.Exception();
        }
        Gameset = new Dictionary<userPublic, GameObject>();
        LeaveButton = GameObject.FindGameObjectWithTag("GameHostLobbyLeaveGameButton").GetComponent<Button>();
        LeaveButton.onClick.AddListener(LeaveClick);

        LockRoomButton = GameObject.FindGameObjectWithTag("GameHostLobbyPanelLockRoomButton").GetComponent<Button>();
        LockRoomButton.onClick.AddListener(SwitchLock);
        StartButton = GameObject.FindGameObjectWithTag("GameHostLobbyStartGameButton").GetComponent<Button>();

	    GetUsersService = new Getter<publicGameInfo>(
	        OnlineManager.ServiceUrl + "/games/{gameID}/lobbyinfo",
	        type =>
	        {
		        UnityEngine.Transform parent = GameObject.FindGameObjectWithTag("GameHostLobbyScroll ViewViewportContent").GetComponent<UnityEngine.Transform>();
		        Dictionary<userPublic, GameObject> NewGameSet = new Dictionary<userPublic, GameObject>();

		        foreach (userPublic stat in type.players)
		        {
			        if (Gameset.ContainsKey(stat))
			        {
				        NewGameSet.Add(stat, Gameset[stat]);
				        Gameset.Remove(stat);
			        }
			        else
			        {
				        NewGameSet.Add(stat, SetupItem(parent, type.host.username.Equals(stat.username), stat));
			        }
		        }
		        foreach (KeyValuePair<userPublic, GameObject> pair in Gameset)
		        {
			        Destroy(pair.Value);
		        }

		        Gameset = NewGameSet;
		        int i = 0;

		        foreach (KeyValuePair<userPublic, GameObject> pair in Gameset)
		        {
			        UnityEngine.Vector3 pos = pair.Value.transform.localPosition;
			        pos.y = (i++ * -30f) - 15f;
			        pair.Value.transform.localPosition = pos;
		        }


		        Text bttnText = LockRoomButton.transform.Find("Text").GetComponent<Text>();
		        /*if (lobbyInfo.IsLocked){
		            bttnText.text = "Unlock Room";
		        } else {
		            bttnText.text = "Lock Room";
		        } */
	        });
		
		LeaveRoomService = new Deleter(OnlineManager.ServiceUrl + "/games/{gameID}/users/{userID}", response => {
			if (response.IsSuccessStatusCode)
			{
				gameObject.SetActive(false);
				GameObject.Find("Canvas/Online/GameRegistry").SetActive(true);
			}
		});
		
		KickPlayerService = new Deleter(OnlineManager.ServiceUrl + "/games/{gameID}/users/{userID}", response =>
		{

		});
	}

    void SwitchLock(){
        //OnlineManager.GameHost.SwitchLock();
        sessionInfo ses = new sessionInfo();
        ses.publicID = "Bob";
    }

    void LeaveClick()
    {
	    LeaveRoomService.SetHeader("PrivateID", OnlineManager.Player.privateID);
	    var dictionary = new Dictionary<string, string>();
	    dictionary.Add("gameID", OnlineManager.Game.gameID);
	    dictionary.Add("userID", OnlineManager.Player.username);
	    LeaveRoomService.SetUrlVariables(dictionary);
	    LeaveRoomService.Run();
    }
	
	// Update is called once per frame
	void UpdateList ()
	{
		var dictionary = new Dictionary<string, string>();
		dictionary.Add("gameID", OnlineManager.Game.gameID);
		GetUsersService.SetUrlVariables(dictionary);
		GetUsersService.Run();
	}

	private void OnEnable()
	{
		InvokeRepeating("UpdateList", 0.0f, 1f);
	}

	private void OnDisable()
	{
		CancelInvoke();
	}

	void Update()
    {
        
    }

    GameObject SetupItem(Transform parent, bool isHost, userPublic stat){
        GameObject newPanel = Instantiate(Panel, new UnityEngine.Vector3(0, 0, 0), Quaternion.identity);
        RectTransform rectTransform = newPanel.GetComponent<RectTransform>();
        Text newText = newPanel.transform.Find("Text").GetComponent<Text>();
        Button newButton = newPanel.transform.Find("Button").GetComponent<Button>();

        newPanel.transform.SetParent(parent);

        rectTransform.anchorMax = new Vector2(0.5f, 1);
        rectTransform.anchorMin = new Vector2(0.5f, 1);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new UnityEngine.Vector3(0, 0, 0);

        if (isHost){
            newText.text = "Host: " + stat.username;
            newButton.gameObject.SetActive(false);
        } else {
            newText.text = stat.username;
            newButton.onClick.AddListener(() => {
	            KickPlayerService.SetHeader("PrivateID", OnlineManager.Player.privateID);
	            var dictionary = new Dictionary<string, string>();
	            dictionary.Add("gameID", OnlineManager.Game.gameID);
	            dictionary.Add("userID", stat.username);
	            KickPlayerService.SetUrlVariables(dictionary);
	            KickPlayerService.Run();
            });
        }

        newPanel.SetActive(true);
        return newPanel;
    }
}
