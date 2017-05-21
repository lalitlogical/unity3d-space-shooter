using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour {

	public Text highestScore;

	void Start() {
		if (highestScore != null) {
			int bestScore = PlayerPrefs.GetInt ("BestScore");
			highestScore.text = "Best Score : " + bestScore;
		}
	}

	public void StartGame () {
		SceneManager.LoadScene ("Game");
	}
		
	public void QuitGame () {
		#if UNITY_STANDALONE
		//Quit the application
		Application.Quit(); 
		#endif

		if (Application.platform == RuntimePlatform.Android) {
			Application.Quit(); 
		}

		//If we are running in the editor
		#if UNITY_EDITOR
		//Stop playing the scene
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}
