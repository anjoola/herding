using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Boid created by the BoidController class
public class ChildBoid : GeneralBoid
{
	private bool physicsRemoved = false;
	public Material filledMaterial;

	new void FixedUpdate(){
		if (physicsRemoved) {
			gameObject.rigidbody2D.velocity = new Vector2(0,0);

		} else {
			base.FixedUpdate();
		}



	}
	
//	
//	void OnMouseDown()
//	{
//		if (physicsRemoved)
//			return;
//		base.OnMouseDown ();
//	}
//	
//	void OnMouseUp()
//	{
//		if (physicsRemoved)
//			return;
//		base.OnMouseUp ();
//	}
//	
//	void OnMouseDrag()
//	{
//		if (physicsRemoved)
//			return;
//		base.OnMouseDrag ();
//	}

//	void OnTriggerEnter2D(Collider2D other) {
//		Debug.Log ("Hello");
//		if (other.gameObject.tag == "SeatTag")
//		{
//			other.gameObject.tag = "Untagged";
////			other.gameObject.GetComponentsInChildren<Renderer>()[0].material = filledMaterial;
//			
//			//base.RemovePhysicsNoDestroy();
//			physicsRemoved = true;
//			gameObject.rigidbody2D.transform.position = other.gameObject.rigidbody2D.transform.position;
//			
//			if (!esting) {
//				
//				Debug.Log ("Increment Point");
//				GlobalStateController.addScore(40);
//            }
//        }
//    }
//
//	void OnCollisionEnter2D(Collision2D coll)
//	{
//		Debug.Log ("Avoiding collision");
//		if (physicsRemoved)	return;
//
//	}
}
