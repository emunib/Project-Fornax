using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameHostLobbyController : MonoBehaviour {
    Dictionary<Online.PlayerStats, GameObject> Gameset;
    private Button LeaveButton;
    private Button StartButton;
    private Button LockRoomButton;
    private Online.LobbyInfo lobbyInfo;

    public GameObject Panel;
	// Use this for initialization
	void Start () {
        Panel = Resources.Load("GamePanel") as GameObject;
        if (Panel == null)
        {
            throw new System.Exception();
        }
        Gameset = new Dictionary<Online.PlayerStats, GameObject>();
        LeaveButton = GameObject.Find("Canvas/Online/GameHostLobby/LeaveGameButton").GetComponent<Button>();
        LeaveButton.onClick.AddListener(LeaveClick);

        LockRoomButton = GameObject.Find("Canvas/Online/GameHostLobby/Panel/LockRoomButton").GetComponent<Button>();
        LockRoomButton.onClick.AddListener(SwitchLock);
        StartButton = GameObject.Find("Canvas/Online/GameHostLobby/StartGameButton").GetComponent<Button>();


        InvokeRepeating("UpdateList", 0.0f, 1f);
	}

    void SwitchLock(){
        OnlineManager.GameHost.SwitchLock();
    }

    void LeaveClick(){
        OnlineManager.Player.LeaveGame();
        gameObject.SetActive(false);
        GameObject.Find("Canvas/Online/GameRegistry").SetActive(true);
    }
	
	// Update is called once per frame
	void UpdateList () {
        lobbyInfo = OnlineManager.GameHost.GetLobbyInfo();
        UnityEngine.Transform parent = GameObject.Find("/Canvas/Online/GameHostLobby/Scroll View/Viewport/Content").GetComponent<UnityEngine.Transform>();
        Dictionary<Online.PlayerStats, GameObject> NewGameSet = new Dictionary<Online.PlayerStats, GameObject>();
        if (Gameset.ContainsKey(lobbyInfo.Host))
        {
            NewGameSet.Add(lobbyInfo.Host, Gameset[lobbyInfo.Host]);
            Gameset.Remove(lobbyInfo.Host);
        }
        else
        {
            NewGameSet.Add(lobbyInfo.Host, SetupItem(parent, false, lobbyInfo.Host));
        }
        foreach (Online.PlayerStats stat in lobbyInfo.Players)
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
        foreach (KeyValuePair<Online.PlayerStats, GameObject> pair in Gameset)
        {
            Destroy(pair.Value);
        }

        Gameset = NewGameSet;
        int i = 0;

        foreach (KeyValuePair<Online.PlayerStats, GameObject> pair in Gameset)
        {
            UnityEngine.Vector3 pos = pair.Value.transform.localPosition;
            pos.y = (i++ * -30f) - 15f;
            pair.Value.transform.localPosition = pos;
        }

        Text bttnText = LockRoomButton.transform.Find("Text").GetComponent<Text>();
        if (lobbyInfo.IsLocked){
            bttnText.text = "Unlock Room";
        } else {
            bttnText.text = "Lock Room";
        }
	}

    void Update()
    {
        
    }

    GameObject SetupItem(Transform parent, bool isHost, Online.PlayerStats stat){
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
            newText.text = "Host: " + stat.Username;
            newButton.gameObject.SetActive(false);
        } else {
            newText.text = stat.Username;
            newButton.onClick.AddListener(() => {
                OnlineManager.GameHost.KickPlayer(stat.Username);
            });
        }

        return newPanel;
    }
}
