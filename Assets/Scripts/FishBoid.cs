using UnityEngine;
using System.Collections;

public class FishBoid : GeneralBoid
{

	int numFramesBeforeNotInCollision;
	// Use this for initialization
	void Start () {
		base.Start ();
		Physics2D.gravity = new Vector2 (100, 0);
	}

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

	// Update is called once per frame
	void Update () {
		if (base.inCollision) 
		{
//			if (numFramesBeforeNotInCollision <= 0)
//			{
//				base.inCollision = false;
//			}
			numFramesBeforeNotInCollision--;
		}

	}
	

	
	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log ("Collided");
		if (coll.gameObject.tag.Equals ("WallTag")) 
		{
			base.inCollision = true;
			numFramesBeforeNotInCollision = 100;
		}

	}

}
