using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Online;
public class GameRegistryController : MonoBehaviour
{
    public Dictionary<GamePrx,GameObject> Gameset;
    public Text Username;
    public Button LogoutButton;
    public Button CreateButton;
    public Button JoinButton;

    public GameObject text;

    // Use this for initialization
    void Start()
    {
        text = Resources.Load("GamePanel") as GameObject;
        if (text == null){
            throw new System.Exception();
        }
        LogoutButton = GameObject.Find("/Canvas/Online/GameRegistry/LogoutButton").GetComponent<Button>();
        LogoutButton.onClick.AddListener(LogoutClick);

        CreateButton = GameObject.Find("/Canvas/Online/GameRegistry/CreateGameButton").GetComponent<Button>();
        CreateButton.onClick.AddListener(CreateGameClick);

        JoinButton = GameObject.Find("/Canvas/Online/GameRegistry/JoinGameButton").GetComponent<Button>();
        JoinButton.onClick.AddListener(LogoutClick);

        Username.text = OnlineManager.Player.GetStats().Username;
        InvokeRepeating("UpdateList", 0f, 1f);
        Gameset = new Dictionary<GamePrx, GameObject>();
    }

    void LogoutClick(){
        OnlineManager.Player.LogOut(null);
        this.gameObject.SetActive(false);
        GameObject.Find("/Canvas/Online/OnlineLogin").SetActive(true);
    }

    void CreateGameClick() {
        ServerPrx server = ServerPrxHelper.checkedCast(OnlineManager.Adapater.addWithUUID(new ServerImpl()));
        OnlineManager.GameHost = OnlineManager.Player.CreateGame(server, null);
        if (OnlineManager.GameHost != null){
            GameObject.Find("/Canvas/Online/GameHostLobby").SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void UpdateList(){
        UnityEngine.Transform parent = GameObject.Find("/Canvas/Online/GameRegistry/Scroll View/Viewport/Content").GetComponent<UnityEngine.Transform>();
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
                childText.text = game.ice_getIdentity().name;
                childButton.onClick.AddListener(() => {
                    Debug.Log("clicked: " + childText.text);
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
