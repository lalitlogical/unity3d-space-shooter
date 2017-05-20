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
	public Transform shotspawn1;

	public float fireDelta = 0.5F;

	private float nextFire = 0.5F;
	private float myTime = 0.0F;

	private AudioSource audioSource ;

	private Vector3 dirInit = Vector3.zero;

	private int childMode;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource >();

		dirInit.z = Input.acceleration.z;
		dirInit.x = Input.acceleration.x;

		childMode = PlayerPrefs.GetInt ("ChildMode");
	}

	void Update () {
		myTime = myTime + Time.deltaTime;

		if (Input.GetButton("Fire1") && myTime > nextFire)
		{
			nextFire = myTime + fireDelta;
			if (childMode == 0) {
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
			} else {
				for (int i = 0; i < shotspawns3.Length; i++) {
					Transform shotspawn = shotspawns3 [i].transform;
					Instantiate (shots[1], shotspawn.position, shotspawn.rotation); 
				}
			}

			// create code here that animates the newProjectile

			nextFire = nextFire - myTime;
			myTime = 0.0F;
			audioSource.Play ();
		}
	}

	void FixedUpdate () {
		Vector3 movement;
		if (Application.platform != RuntimePlatform.Android) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			if (childMode == 0) {
				movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
			} else {
				movement = new Vector3 (moveHorizontal, 0.0f, 0.0f);
			}
			rb.velocity = movement * speed;
		} else {
			movement = Vector3.zero;
			movement.x = Input.acceleration.x - dirInit.x;
			if (childMode == 0) {
				movement.z = - (Input.acceleration.z - dirInit.z);
			}
			rb.velocity = movement * speed * 2;
		}


		rb.position = new Vector3 
		(
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f, 
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
