using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyByTime : MonoBehaviour {

	public float lifetime;
	public Text shieldTimer;

	private ShieldController shieldController;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, lifetime);
		if (gameObject.CompareTag ("Shield")) {
			GameObject shieldTimerObject = GameObject.FindWithTag ("ShieldTimer");
			if (shieldTimerObject != null) {
				shieldTimer = shieldTimerObject.GetComponent <Text> ();
			} else {
				Debug.Log ("Cannot find 'shieldTimerObject' script");
			}
		}
	}

	void Update () {
		if (gameObject.CompareTag ("Shield") && shieldTimer != null) {
			lifetime -= Time.deltaTime;
			shieldController = gameObject.GetComponent <ShieldController> ();
			shieldTimer.text = "Shield : " + (int)lifetime + " seconds or " + shieldController.countUntillDestroy + " times";	
		}
	}

	void OnDestroy() {
		if (gameObject.CompareTag ("Shield") && shieldTimer != null) {
			shieldTimer.text = "";	
		}
	}
}
