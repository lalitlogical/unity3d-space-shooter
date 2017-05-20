using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildMode : MonoBehaviour {

	public Toggle activateChildeMode;

	// Use this for initialization
	void Start () {
		int childMode = PlayerPrefs.GetInt ("ChildMode");
		if (childMode == 1) {
			activateChildeMode.isOn = true;
		} else {
			activateChildeMode.isOn = false;
		}
	}

	public void ActivateChildMode () {		
		if (activateChildeMode.isOn) {
			PlayerPrefs.SetInt("ChildMode", 1);
		} else {
			PlayerPrefs.SetInt("ChildMode", 0);
		}
	}
}
