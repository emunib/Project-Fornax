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
    private LobbyInfo lobbyInfo;

    public GameObject Panel;
	// Use this for initialization
	void Start () {
        Panel = Resources.Load("GamePanel") as GameObject;
        if (Panel == null)
        {
            throw new System.Exception();
        }
        Gameset = new Dictionary<userPublic, GameObject>();
        LeaveButton = GameObject.Find("Canvas/Online/GameHostLobby/LeaveGameButton").GetComponent<Button>();
        LeaveButton.onClick.AddListener(LeaveClick);

        LockRoomButton = GameObject.Find("Canvas/Online/GameHostLobby/Panel/LockRoomButton").GetComponent<Button>();
        LockRoomButton.onClick.AddListener(SwitchLock);
        StartButton = GameObject.Find("Canvas/Online/GameHostLobby/StartGameButton").GetComponent<Button>();


        InvokeRepeating("UpdateList", 0.0f, 1f);
	}

    void SwitchLock(){
        //OnlineManager.GameHost.SwitchLock();
        session ses = new session();
        ses.publicID = "Bob";
    }

    void LeaveClick(){
        //OnlineManager.GameHost = null;
        CancelInvoke();
        //OnlineManager.Player.LeaveGame();
        gameObject.SetActive(false);
        GameObject.Find("Canvas/Online/GameRegistry").SetActive(true);
    }
	
	// Update is called once per frame
	void UpdateList () {
        //lobbyInfo = OnlineManager.GameHost.GetLobbyInfo();
        UnityEngine.Transform parent = GameObject.Find("/Canvas/Online/GameHostLobby/Scroll View/Viewport/Content").GetComponent<UnityEngine.Transform>();
        Dictionary<userPublic, GameObject> NewGameSet = new Dictionary<userPublic, GameObject>();
        if (Gameset.ContainsKey(lobbyInfo.Host))
        {
            NewGameSet.Add(lobbyInfo.Host, Gameset[lobbyInfo.Host]);
            Gameset.Remove(lobbyInfo.Host);
        }
        else
        {
            NewGameSet.Add(lobbyInfo.Host, SetupItem(parent, true, lobbyInfo.Host));
        }
        foreach (userPublic stat in lobbyInfo.Players)
        {
            if (Gameset.ContainsKey(stat))
            {
                NewGameSet.Add(stat, Gameset[stat]);
                Gameset.Remove(stat);
            }
            else
            {
                NewGameSet.Add(stat, SetupItem(parent, false, stat));
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
                //OnlineManager.GameHost.KickPlayer(stat.Username);
            });
        }

        newPanel.SetActive(true);
        return newPanel;
    }
}
