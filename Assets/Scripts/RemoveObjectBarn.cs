using UnityEngine;
using System.Collections;

public class RemoveObjectBarn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Trigger: " + collider.isTrigger);
	}

	
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Trigger");
		if (other.gameObject.tag == "Boid")
		{
			Destroy(other.gameObject);
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
