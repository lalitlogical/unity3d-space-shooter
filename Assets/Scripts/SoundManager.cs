using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private AudioSource audioSource;
	private bool isMusicPlaying = true;

	void Awake () {
		if (PlayerPrefs.GetInt ("Toggles") == 0) {
			PlayerPrefs.SetInt ("ChildMode", 0);
			PlayerPrefs.SetInt ("BackgroundMusic", 1);
			PlayerPrefs.SetInt ("PlayerMusic", 1);
			PlayerPrefs.SetInt ("OtherMusic", 1);
			PlayerPrefs.SetInt ("Toggles", 1);
		}
	}

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
		int mode = PlayerPrefs.GetInt ("BackgroundMusic");
		if (mode == 1) {
			audioSource.Play ();
		} else {
			isMusicPlaying = false;
		}
	}

	public void HandleMusic () {
		if (isMusicPlaying) {
			audioSource.Pause ();
		} else {
			audioSource.Play ();
		}
	}
}
