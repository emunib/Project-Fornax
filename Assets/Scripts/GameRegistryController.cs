using System;
using System.Collections.Generic;
using System.Net;
using Schemas;
using UnityEngine;
using UnityEngine.UI;
public class GameRegistryController : MonoBehaviour
{
    public Dictionary<publicGameInfo,GameObject> Gameset;
    public Text Username;
    public Button LogoutButton;
    public Button CreateButton;

    public GameObject text;

    private Deleter LogoutService;
    private Poster<publicGameInfo, sessionInfo> CreateGameService;
    private Getter<publicGameInfoList> GameListService;
    private Poster<publicGameInfo, sessionInfo> JoinGameService;
    private String ServiceUrl = OnlineManager.ServiceUrl;
    private String UsersUrl = OnlineManager.UsersUrl;
    private String SessionUrl = OnlineManager.SessionUrl;

    // Use this for initialization
    void Awake()
    {
        text = Resources.Load("GamePanel") as GameObject;
        if (text == null){
            throw new System.Exception();
        }
        LogoutButton = GameObject.FindGameObjectWithTag("GameRegistryLogoutButton").GetComponent<Button>();
        LogoutButton.onClick.AddListener(LogoutClick);

        CreateButton = GameObject.FindGameObjectWithTag("GameRegistryCreateGameButton").GetComponent<Button>();
        CreateButton.onClick.AddListener(CreateGameClick);

        Gameset = new Dictionary<publicGameInfo, GameObject>();
        
        LogoutService = new Deleter(ServiceUrl + UsersUrl + "/{userID}" + SessionUrl + "/{sessionID}", response =>
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                gameObject.SetActive(false);
                OnlineManager.Player = null;
                GameObject.Find("/Canvas/Online/OnlineLogin").SetActive(true);
            }
            else
            {
                Debug.Log("Logout Failed Status Code: " + response.StatusCode);
                Debug.Log(response.Headers.ToString());
            }
        }) ;        
        
        CreateGameService = new Poster<publicGameInfo, sessionInfo>(ServiceUrl + "/games", type =>
        {
            OnlineManager.Game = type;
            if ((OnlineManager.Game.host != null) && (OnlineManager.Game.host.username == OnlineManager.Player.username)){
                GameObject.Find("/Canvas/Online/GameHostLobby").SetActive(true);
                gameObject.SetActive(false);
            }
        });
        
        
        GameListService = new Getter<publicGameInfoList>(ServiceUrl + "/games", type =>
        {
             UnityEngine.Transform parent = GameObject.FindGameObjectWithTag("GameRegistryScrollViewViewportContent").GetComponent<UnityEngine.Transform>();
            Dictionary<publicGameInfo, GameObject> NewGameSet = new Dictionary<publicGameInfo, GameObject>();
            if (type.publicGameInfos != null)
            {
                foreach (publicGameInfo game in type.publicGameInfos)
                {
                    if (Gameset.ContainsKey(game))
                    {
                        NewGameSet.Add(game, Gameset[game]);
                        Gameset.Remove(game);
                    }
                    else
                    {
                        GameObject newText = Instantiate(text, new UnityEngine.Vector3(0, 0, 0), Quaternion.identity);
                        Text childText = newText.transform.Find("Text").GetComponent<Text>();
                        RectTransform rectTransform = newText.GetComponent<RectTransform>();
                        Button childButton = newText.transform.Find("Button").GetComponent<Button>();
                        newText.transform.SetParent(parent);
                        rectTransform.anchorMax = new Vector2(0.5f, 1);
                        rectTransform.anchorMin = new Vector2(0.5f, 1);
                        rectTransform.pivot = new Vector2(0.5f, 0.5f);
                        rectTransform.anchoredPosition = new UnityEngine.Vector3(0, 0, 0);
                        childText.text = "Unset";
                        childButton.onClick.AddListener(() =>
                        {
                            var dictionary = new Dictionary<string, string>();
                            dictionary.Add("gameID", game.gameID);
                            JoinGameService.SetUrlVariables(dictionary);
                            JoinGameService.Run(OnlineManager.Player);
                        });
                        NewGameSet.Add(game, newText);
                    }
                }
            }
            foreach (KeyValuePair<publicGameInfo, GameObject> pair in Gameset){
            Destroy(pair.Value.gameObject);
        }
        Gameset = NewGameSet;
        int i = 0;
        foreach (KeyValuePair<publicGameInfo,GameObject>  pair in Gameset){
            Text childText = pair.Value.transform.Find("Text").GetComponent<Text>();
                publicGameInfo info = pair.Key;
                childText.text = "Game Id: " + info.gameID + " Host: " + info.host.username + " Players: " + (info.players.Length);
                if (!pair.Value.activeSelf){
                    pair.Value.SetActive(true);
                }
                UnityEngine.Vector3 pos = pair.Value.transform.localPosition;
                pos.y = (i++ * -30f) - 15f;
                pair.Value.transform.localPosition = pos;
            
        }
        });
        
        JoinGameService = new Poster<publicGameInfo, sessionInfo>(ServiceUrl + "/games/{gameID}/users", type =>
        {    
            OnlineManager.Game = type;
            if (type.players != null)
            {
                gameObject.SetActive(false);
                GameObject.Find("/Canvas/Online/GameLobby").SetActive(true);
            }
        });
    }

    void LogoutClick()
    {
        LogoutService.SetHeader("PrivateID", OnlineManager.Player.privateID);
        var dictionary = new Dictionary<string, string>();
        dictionary.Add("userID", OnlineManager.Player.username);
        dictionary.Add("sessionID",  OnlineManager.Player.publicID);
        LogoutService.SetUrlVariables(dictionary);
        LogoutService.Run();
    }

    private void OnEnable()
    {
        Username.text = OnlineManager.Player.username;
        InvokeRepeating("UpdateList", 0f, 1f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void CreateGameClick()
    {
        CreateGameService.Run(OnlineManager.Player);
    }

    void UpdateList(){
       GameListService.Run();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
