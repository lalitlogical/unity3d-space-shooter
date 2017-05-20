using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;

	private GameController gameController;
	private ShieldController shieldController;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController> ();
		} else {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Boundary") || other.gameObject.CompareTag ("BulletActivator") || other.gameObject.CompareTag ("ShieldActivator")) {
			return;
		}

		if (other.gameObject.CompareTag ("Shield")	) {
			HandleExplosionWithSound (explosion, transform);
//			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);

			shieldController = other.gameObject.GetComponent <ShieldController> ();
			if (shieldController.countUntillDestroy == 0) {
				HandleExplosionWithSound (playerExplosion, other.transform);
//				Instantiate (playerExplosion, other.transform.position, other.transform.rotation);	
				Destroy (other.gameObject);
			} else {
				shieldController.countUntillDestroy -= 1;
			}
			return;
		} else if (other.gameObject.CompareTag ("Player")) {
			GameObject shield = GameObject.FindWithTag("Shield");
			if (shield != null) {
				shieldController = shield.GetComponent <ShieldController> ();
				if (shieldController.countUntillDestroy > 0) {
					return;
				}
			}
		}
			
//		Instantiate (explosion, transform.position, transform.rotation);
		HandleExplosionWithSound (explosion, transform);
		if (other.gameObject.CompareTag ("Player")) {
			HandleExplosionWithSound(playerExplosion, other.transform);
//			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);	
			gameController.GameOver ();
		} else {
			gameController.AddScore (scoreValue);	
		}
		Destroy (other.gameObject);
		Destroy (gameObject);
	}

	void HandleExplosionWithSound (GameObject explosion, Transform transform) {
		GameObject explosionObject = Instantiate (explosion, transform.position, transform.rotation) as GameObject;
		int mode = PlayerPrefs.GetInt ("OtherMusic");
		Debug.Log ("OtherMusic : " + mode);
		if (mode == 1) {
			AudioSource audioSource = explosionObject.GetComponent<AudioSource> ();
			audioSource.Play ();
		}
	}
}
