using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Boid created by the BoidController class
public class CowBoid : GeneralBoid
{
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "DetectionTag")
		{
			
			if (!testing) {
				
				Debug.Log ("Increment Point");
				GlobalStateController.addScore(40);
            }
			base.Destroy ();
        }
	}
}
