using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneOnClick : MonoBehaviour {
	public Text loadingText;

	// Use this for initialization
	void Start () {

    }

    public void LoadByIndex(int sceneIndex)
    {
        GameObject buttonClicked = this.gameObject;
        if (buttonClicked.name == "Practice Mode")
        {
            ModeSettings.modeType = ModeSettings.Modes.PRACTICE;
        }
        else if (buttonClicked.name == "FFA")
        {
            ModeSettings.modeType = ModeSettings.Modes.FFA;
        }
        else if (buttonClicked.name == "2v2")
        {
            ModeSettings.modeType = ModeSettings.Modes.VERSUS;
        }
        ModeSettings.numLives = 5;

        SceneManager.LoadScene(sceneIndex);
		loadingText.text = "Loading...";

		StartCoroutine(LoadNewScene(sceneIndex));
	}

	IEnumerator LoadNewScene(int sceneIndex)
	{
		//yield return new WaitForSeconds(5);
		AsyncOperation async = SceneManager.LoadSceneAsync (sceneIndex);

		while (!async.isDone) {
			yield return null;
		}
	}

}
