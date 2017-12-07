using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Schemas;

public class GameLobbyController : MonoBehaviour {
    Dictionary<userPublic, Text> Gameset;
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
        Gameset = new Dictionary<userPublic, Text>();
        LeaveButton = GameObject.Find("Canvas/Online/GameLobby/LeaveGameButton").GetComponent<Button>();
        LeaveButton.onClick.AddListener(LeaveClick);
        InvokeRepeating("UpdateList", 0.0f, 1f);
    }

    void LeaveClick()
    {
        OnlineManager.Game = null;
        CancelInvoke();
        //OnlineManager.Player.LeaveGame();
        gameObject.SetActive(false);
        GameObject.Find("Canvas/Online/GameRegistry").SetActive(true);
    }

    // Update is called once per frame
    void UpdateList()
    {
        LobbyInfo info = OnlineManager.Game.GetLobbyInfo();
        UnityEngine.Transform parent = GameObject.Find("/Canvas/Online/GameLobby/Scroll View/Viewport/Content").GetComponent<UnityEngine.Transform>();
        Dictionary<userPublic, Text> NewGameSet = new Dictionary<userPublic, Text>();
        if (Gameset.ContainsKey(info.Host)){
            NewGameSet.Add(info.Host, Gameset[info.Host]);
            Gameset.Remove(info.Host);
        } else {
            Text newText = Instantiate(text, new UnityEngine.Vector3(0, 0, 0), Quaternion.identity).GetComponent<Text>();
            newText.transform.SetParent(parent);
            newText.rectTransform.anchorMax = new Vector2(0.5f, 1);
            newText.rectTransform.anchorMin = new Vector2(0.5f, 1);
            newText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            newText.rectTransform.anchoredPosition = new UnityEngine.Vector3(0, 0, 0);
            newText.text = "Host: " + info.Host.username;
            NewGameSet.Add(info.Host, newText);
        }
        foreach (userPublic stat in info.Players)
        {
            if (Gameset.ContainsKey(stat))
            {
                NewGameSet.Add(stat, Gameset[stat]);
                Gameset.Remove(stat);
            }
            else
            {
                Text newText = Instantiate(text, new UnityEngine.Vector3(0, 0, 0), Quaternion.identity).GetComponent<Text>();
                newText.transform.SetParent(parent);
                newText.rectTransform.anchorMax = new Vector2(0.5f, 1);
                newText.rectTransform.anchorMin = new Vector2(0.5f, 1);
                newText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                newText.rectTransform.anchoredPosition = new UnityEngine.Vector3(0, 0, 0);
                newText.text = stat.username;
                NewGameSet.Add(stat, newText);
            }
        }
        foreach (KeyValuePair<userPublic, Text> pair in Gameset)
        {
            Destroy(pair.Value.gameObject);
        }
        Gameset = NewGameSet;
        int i = 0;
        foreach (KeyValuePair<userPublic, Text> pair in Gameset)
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
