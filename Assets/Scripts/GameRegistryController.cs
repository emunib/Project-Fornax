using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameRegistryController : MonoBehaviour
{
    public Dictionary<GamePrx,GameObject> Gameset;
    public Text Username;
    public Button LogoutButton;
    public Button CreateButton;

    public GameObject text;

    // Use this for initialization
    void Start()
    {
        text = Resources.Load("GamePanel") as GameObject;
        if (text == null){
            throw new System.Exception();
        }
        LogoutButton = GameObject.FindGameObjectWithTag("GameRegistryLogoutButton").GetComponent<Button>();
        LogoutButton.onClick.AddListener(LogoutClick);

        CreateButton = GameObject.FindGameObjectWithTag("GameRegistryCreateGameButton").GetComponent<Button>();
        CreateButton.onClick.AddListener(CreateGameClick);

        Username.text = OnlineManager.Player.username;
        InvokeRepeating("UpdateList", 0f, 1f);
        Gameset = new Dictionary<GamePrx, GameObject>();
    }

    void LogoutClick(){
        //OnlineManager.Player.LogOut(null);
        this.gameObject.SetActive(false);
        GameObject.Find("/Canvas/Online/OnlineLogin").SetActive(true);
    }

    void CreateGameClick() {
        /*
        OnlineManager.GameHost = OnlineManager.Player.CreateGame(server, null);
        if (OnlineManager.GameHost != null){
            GameObject.Find("/Canvas/Online/GameHostLobby").SetActive(true);
            gameObject.SetActive(false);
        }
        */
    }

    void UpdateList(){
        UnityEngine.Transform parent = GameObject.FindGameObjectWithTag("GameRegistryScrollViewViewportContent").GetComponent<UnityEngine.Transform>();
        Dictionary<GamePrx, GameObject> NewGameSet = new Dictionary<GamePrx, GameObject>();
        OnlineManager.LobbyLstnrImpl.mutex.WaitOne();
        foreach(GamePrx game in OnlineManager.LobbyLstnrImpl.AvailableGames){
            if (Gameset.ContainsKey(game)){
                NewGameSet.Add(game, Gameset[game]);
                Gameset.Remove(game);
            } else {
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
                childButton.onClick.AddListener(() => {
                    /* if (OnlineManager.Player.JoinGame(client, game)){
                        OnlineManager.Game = game;
                        gameObject.SetActive(false);
                        GameObject.Find("/Canvas/Online/GameLobby").SetActive(true);
                    } */
                });
                NewGameSet.Add(game, newText);
            }
        }
        OnlineManager.LobbyLstnrImpl.mutex.ReleaseMutex();
        foreach (KeyValuePair<GamePrx, GameObject> pair in Gameset){
            Destroy(pair.Value.gameObject);
        }
        Gameset = NewGameSet;
        int i = 0;
        foreach (KeyValuePair<GamePrx,GameObject>  pair in Gameset){
            Text childText = pair.Value.transform.Find("Text").GetComponent<Text>();
                LobbyInfo info = pair.Key.GetLobbyInfo();
                childText.text = "Game Id: " + info.Id + " Host: " + info.Host.username + " Players: " + (info.Players.Count + 1);
                if (!pair.Value.activeSelf){
                    pair.Value.SetActive(true);
                }
                UnityEngine.Vector3 pos = pair.Value.transform.localPosition;
                pos.y = (i++ * -30f) - 15f;
                pair.Value.transform.localPosition = pos;
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
