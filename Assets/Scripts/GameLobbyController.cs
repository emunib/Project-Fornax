using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Schemas;

public class GameLobbyController : MonoBehaviour {
    Dictionary<userPublic, Text> Gameset;
    private Button LeaveButton;

    public GameObject text;
    
    private Deleter LeaveRoomService;
    private Getter<publicGameInfo> GetUsersService;
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

        LeaveRoomService = new Deleter(OnlineManager.ServiceUrl + "/games/{gameID}/users/{userID}", response =>
        {
            if (response.IsSuccessStatusCode)
            {
                gameObject.SetActive(false);
                GameObject.Find("Canvas/Online/GameRegistry").SetActive(true);
            }
        });

        GetUsersService = new Getter<publicGameInfo>(
            OnlineManager.ServiceUrl + "/games/{gameID}/lobbyinfo",
            type =>
            {
                UnityEngine.Transform parent = GameObject.FindGameObjectWithTag("GameLobbyScrollViewViewportContent")
                    .GetComponent<UnityEngine.Transform>();
                Dictionary<userPublic, Text> NewGameSet = new Dictionary<userPublic, Text>();
                bool isKicked = true;
                foreach (userPublic stat in type.players)
                {
                    if (stat.username.Equals(OnlineManager.Player.username))
                    {
                        isKicked = false;
                    }
                    if (Gameset.ContainsKey(stat))
                    {
                        NewGameSet.Add(stat, Gameset[stat]);
                        Gameset.Remove(stat);
                    }
                    else
                    {
                        Text newText = Instantiate(text, new UnityEngine.Vector3(0, 0, 0), Quaternion.identity)
                            .GetComponent<Text>();
                        newText.transform.SetParent(parent);
                        newText.rectTransform.anchorMax = new Vector2(0.5f, 1);
                        newText.rectTransform.anchorMin = new Vector2(0.5f, 1);
                        newText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                        if (type.host == stat)
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
                if (isKicked)
                {
                    gameObject.SetActive(false);
                    GameObject.Find("Canvas/Online/GameRegistry").SetActive(true);
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
                if (type.host.username.Equals(OnlineManager.Player.username))
                {
                    gameObject.SetActive(false);
                    GameObject.Find("/Canvas/Online/GameHostLobby").SetActive(true);
                }
            });
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
    void UpdateList()
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
}
