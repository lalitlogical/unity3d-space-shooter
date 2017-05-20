using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour {

	private Toggle toggleButton;
	private SoundManager soundManager;

	// Use this for initialization
	void Start () {
		toggleButton = gameObject.GetComponent <Toggle> ();
		int mode = PlayerPrefs.GetInt (gameObject.tag);
		if (mode == 1) {
			toggleButton.isOn = true;
		} else {
			toggleButton.isOn = false;
		}

	}
	
	// Update is called once per frame
	public void ApplyToggle () {
		if (toggleButton.isOn) {
			PlayerPrefs.SetInt(gameObject.tag, 1);
		} else {
			PlayerPrefs.SetInt(gameObject.tag, 0);
		}
		if (gameObject.CompareTag ("BackgroundMusic")) {
			HandleMusic (toggleButton.isOn);
		}
	}

	void HandleMusic (bool isPlayMusic) {
		GameObject soundManager = GameObject.FindWithTag ("SoundManager");
		if (soundManager != null) {
			AudioSource audioSource = soundManager.GetComponent<AudioSource> ();
			if (isPlayMusic) {
				audioSource.Play ();
			} else {
				audioSource.Pause ();
			}
		}
	}
}
