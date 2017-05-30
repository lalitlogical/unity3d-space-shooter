using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTouch : MonoBehaviour {

	private GameController gameController;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController> ();
		} else {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}


	void OnMouseDown () {
		string GameMode = PlayerPrefs.GetString ("GameMode");
		if (GameMode == "ChildMode") {
			DestroyByContact destroyByContact = gameObject.GetComponent<DestroyByContact> ();
			gameController.AddScore (destroyByContact.scoreValue);	
			Destroy (gameObject);
		}
	}
}
