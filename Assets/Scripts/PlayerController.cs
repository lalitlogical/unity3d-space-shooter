using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;

	public float speed;
	public float tilt;
	public Boundary boundary;

	public GameObject[] shots;
	public GameObject[] shotspawns2;
	public GameObject[] shotspawns3;
	public GameObject cleaner;
	public GameObject cleanerButton;

	public Transform shotspawn1;
	public float fireDelta = 0.5F;

	private float nextFire = 0.5F;
	private float myTime = 0.0F;
	private AudioSource audioSource ;
	private Vector3 dirInit = Vector3.zero;
	private string GameMode;
	private int stickToXAxis;
	private Touch touch;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource >();

		dirInit.z = Input.acceleration.z;
		dirInit.x = Input.acceleration.x;

		GameMode = PlayerPrefs.GetString ("GameMode");
		stickToXAxis = PlayerPrefs.GetInt ("StickToXAxis");
	}

	void Update () {
		myTime = myTime + Time.deltaTime;

		 if (Input.GetButton ("Fire1"))
			 FireAction ();
	}

	void FixedUpdate () {
		Vector3 movement = Vector3.zero;
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.touchCount > 0) {
				touch = Input.GetTouch (0);
				if (touch.phase == TouchPhase.Moved) {
					movement.x = touch.deltaPosition.x;
					movement.z = touch.deltaPosition.y;
					rb.velocity = movement;
					rb.position = new Vector3 
						(
							Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
							0.0f, 
							Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
						);
				}
			} else {
				movement.x = Input.acceleration.x - dirInit.x;
				if (GameMode != "ChildMode" && stickToXAxis == 0) {
					movement.z = - (Input.acceleration.z - dirInit.z);
				}

				rb.velocity = movement * speed;
				rb.position = new Vector3 
					(
						Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
						0.0f, 
						Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
					);
			}
		} else {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			if (GameMode == "ChildMode" || stickToXAxis == 1) {
				movement = new Vector3 (moveHorizontal, 0.0f, 0.0f);
			} else {
				movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			}

			rb.velocity = movement * speed;
			rb.position = new Vector3 
				(
					Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
					0.0f, 
					Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
				);
		}

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}

	public void FireAction () {
		if (myTime > nextFire)
		{
			nextFire = myTime + fireDelta;
			if (GameMode == "ChildMode") {
				for (int i = 0; i < shotspawns3.Length; i++) {
					Transform shotspawn = shotspawns3 [i].transform;
					Instantiate (shots [1], shotspawn.position, shotspawn.rotation); 
				}
			} else {
				int bulletCount = PlayerPrefs.GetInt ("BulletCount");
				if (bulletCount <= 1) {
					Instantiate (shots[0], shotspawn1.position, shotspawn1.rotation); 
				} else if (bulletCount == 2) {
					for (int i = 0; i < shotspawns2.Length; i++) {
						Transform shotspawn = shotspawns2 [i].transform;
						Instantiate (shots[0], shotspawn.position, shotspawn.rotation); 
					}
				} else if (bulletCount >= 3) {
					for (int i = 0; i < shotspawns3.Length; i++) {
						Transform shotspawn = shotspawns3 [i].transform;
						Instantiate (shots[1], shotspawn.position, shotspawn.rotation); 
					}
				}
			}

			nextFire = nextFire - myTime;
			myTime = 0.0F;
			int mode = PlayerPrefs.GetInt ("PlayerMusic");
			if (mode == 1) audioSource.Play ();
		}
	}

	public void FireCleaner () {
		Vector3 cVector = Vector3.zero;
		cVector.z = shotspawn1.position.z;
		Instantiate (cleaner, cVector, shotspawn1.rotation); 
		cleanerButton.SetActive (false);
	}
}
