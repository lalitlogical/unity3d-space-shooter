using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

	void OnTriggerExit(Collider other)
	{
		// Destroy everything that leaves the trigger
		Destroy(other.gameObject);
	}
}
