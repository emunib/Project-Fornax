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
