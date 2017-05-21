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
		int childMode = PlayerPrefs.GetInt ("ChildMode");
		if (childMode == 1) {
			DestroyByContact destroyByContact = gameObject.GetComponent<DestroyByContact> ();
			gameController.AddScore (destroyByContact.scoreValue);	
			Destroy (gameObject);
		}
	}
}
