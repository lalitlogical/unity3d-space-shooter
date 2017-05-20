using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRotator : MonoBehaviour {

	private Rigidbody rb;

	public float tumble;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Random.insideUnitSphere * tumble;
	}

	void Update () {
//		rb.gameObject.
//		transform.Rotate(Vector3.right * Time.deltaTime);
//		transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
	}
}
