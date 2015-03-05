using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Boid created by the BoidController class
public class ChildBoid : GeneralBoid
{
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Hello");
		if (other.gameObject.tag == "SeatTag")
		{
			
			if (!testing) {
				
				Debug.Log ("Increment Point");
				GlobalStateController.addScore(40);
            }
            base.Destroy ();
        }
    }
}
