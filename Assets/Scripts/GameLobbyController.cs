using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLobbyController : MonoBehaviour {
    Dictionary<Online.PlayerStats, Text> Gameset;
    private Button LeaveButton;

    public GameObject text;
    // Use this for initialization
    void Start()
    {
        text = Resources.Load("Text") as GameObject;
        if (text == null)
        {
            throw new System.Exception();
        }
        Gameset = new Dictionary<Online.PlayerStats, Text>();
        LeaveButton = GameObject.Find("Canvas/Online/GameHostLobby/LeaveGameButton").GetComponent<Button>();
        LeaveButton.onClick.AddListener(LeaveClick);
        InvokeRepeating("UpdateList", 0.0f, 1f);
    }

    void LeaveClick()
    {
        OnlineManager.Player.LeaveGame();
        gameObject.SetActive(false);
        GameObject.Find("Canvas/Online/GameRegistry").SetActive(true);
    }

    // Update is called once per frame
    void UpdateList()
    {
        Online.LobbyInfo info = OnlineManager.GameHost.GetLobbyInfo();
        UnityEngine.Transform parent = GameObject.Find("/Canvas/Online/GameHostLobby/Scroll View/Viewport/Content").GetComponent<UnityEngine.Transform>();
        Dictionary<Online.PlayerStats, Text> NewGameSet = new Dictionary<Online.PlayerStats, Text>();
        if (Gameset.ContainsKey(info.Host)){
            NewGameSet.Add(info.Host, Gameset[info.Host]);
        } else {
            Text newText = Instantiate(text, new UnityEngine.Vector3(0, 0, 0), Quaternion.identity).GetComponent<Text>();
            newText.transform.SetParent(parent);
            newText.rectTransform.anchorMax = new Vector2(0.5f, 1);
            newText.rectTransform.anchorMin = new Vector2(0.5f, 1);
            newText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            newText.rectTransform.anchoredPosition = new UnityEngine.Vector3(0, 0, 0);
            newText.text = "Host: " + info.Host.Username;
            NewGameSet.Add(info.Host, newText);
        }
        foreach (Online.PlayerStats stat in info.Players)
        {
            if (Gameset.ContainsKey(stat))
            {
                NewGameSet.Add(stat, Gameset[stat]);
            }
            else
            {
                Text newText = Instantiate(text, new UnityEngine.Vector3(0, 0, 0), Quaternion.identity).GetComponent<Text>();
                newText.transform.SetParent(parent);
                newText.rectTransform.anchorMax = new Vector2(0.5f, 1);
                newText.rectTransform.anchorMin = new Vector2(0.5f, 1);
                newText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                newText.rectTransform.anchoredPosition = new UnityEngine.Vector3(0, 0, 0);
                newText.text = stat.Username;
                NewGameSet.Add(stat, newText);
            }
        }
        Gameset = NewGameSet;
        int i = 0;
        foreach (KeyValuePair<Online.PlayerStats, Text> pair in Gameset)
        {
            UnityEngine.Vector3 pos = pair.Value.transform.localPosition;
            pos.y = (i++ * -30f) - 15f;
            pair.Value.transform.localPosition = pos;
        }
    }

    void Update()
    {
    }
}
