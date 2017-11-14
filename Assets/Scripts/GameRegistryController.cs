using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Online;
public class GameRegistryController : MonoBehaviour
{
    public Text Username;
    public PlayerPrx Player;

    // Use this for initialization
    void Start()
    {
        Username.text = Player.GetStats().Username;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
