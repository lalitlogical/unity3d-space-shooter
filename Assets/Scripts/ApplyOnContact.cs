using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyOnContact : MonoBehaviour {

	public int scoreValue;
	public GameObject shield;

	private GameController gameController;

	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController> ();
		} else {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Boundary")) {
			return;
		}

		if (other.gameObject.CompareTag ("Player") && gameController != null) {
			if (gameObject.CompareTag ("BulletActivator")) {
				gameController.UpgradeWeapon ();
				gameController.AddScore (scoreValue);
			} else if (gameObject.CompareTag ("ShieldActivator")) {
				GameObject isShieldPresent = GameObject.FindWithTag("Shield");
				if (isShieldPresent != null) {
					Destroy (isShieldPresent);
				}
				GameObject shieldObject = Instantiate (shield, other.gameObject.transform.position, other.gameObject.transform.rotation) as GameObject;	
				shieldObject.transform.parent = other.gameObject.transform;
			}
			Destroy (gameObject);
		}
	}
}
