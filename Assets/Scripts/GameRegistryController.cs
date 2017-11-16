using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Online;
public class GameRegistryController : MonoBehaviour
{
    public Dictionary<GamePrx,Text> Gameset;
    public Text Username;
    public Button LogoutButton;
    public Button CreateButton;
    public Button JoinButton;

    public GameObject text;

    // Use this for initialization
    void Start()
    {
        text = Resources.Load("Text") as GameObject;
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
        Dictionary<GamePrx, Text> NewGameSet = new Dictionary<GamePrx, Text>();
        OnlineManager.LobbyLstnrImpl.mutex.WaitOne();
        foreach(GamePrx game in OnlineManager.LobbyLstnrImpl.AvailableGames){
            if (Gameset.ContainsKey(game)){
                NewGameSet.Add(game, Gameset[game]);
            } else {
                Text newText = Instantiate(text, new UnityEngine.Vector3(0,0,0), Quaternion.identity).GetComponent<Text>();
                newText.transform.SetParent(parent);
                newText.rectTransform.anchorMax = new Vector2(0.5f, 1);
                newText.rectTransform.anchorMin = new Vector2(0.5f, 1);
                newText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                newText.rectTransform.anchoredPosition = new UnityEngine.Vector3(0, 0, 0);
                newText.text = game.ice_getIdentity().name;
                NewGameSet.Add(game, newText);
            }
        }
        OnlineManager.LobbyLstnrImpl.mutex.ReleaseMutex();
        Gameset = NewGameSet;
        Dictionary<GamePrx, Text>.Enumerator enumerator =  Gameset.GetEnumerator();
        int i = 0;
        foreach (KeyValuePair<GamePrx,Text>  pair in Gameset){
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
