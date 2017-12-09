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
        LeaveButton = GameObject.FindGameObjectWithTag("GameLobbyLeaveGameButton").GetComponent<Button>();
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
        publicGameInfo info = OnlineManager.Game;
        UnityEngine.Transform parent = GameObject.FindGameObjectWithTag("GameLobbyScrollViewViewportContent").GetComponent<UnityEngine.Transform>();
        Dictionary<userPublic, Text> NewGameSet = new Dictionary<userPublic, Text>();
        foreach (userPublic stat in info.players)
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
                if (info.host == stat)
                {
                    newText.text = "Host: " + stat.username;
                }
                else
                {
                    newText.text = stat.username;
                }
                newText.rectTransform.anchoredPosition = new UnityEngine.Vector3(0, 0, 0);
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
