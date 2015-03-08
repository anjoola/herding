using UnityEngine;
using System.Collections;

public class PreyBoid : GeneralPreyBoid
{
	int numFramesBeforeNotInCollision;
	// Use this for initialization
	void Start () {
		base.forceMag = 2.0f;
		base.Start ();
		Physics2D.gravity = new Vector2 (5, 0);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Food Tag")
		{
			Debug.Log ("In food area!");
			if (!testing) {

				// Eaten by shark
				Debug.Log ("Eaten by shark");
				GlobalStateController.addScore(40);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Food Tag")
		{
			Debug.Log ("Out of food area!");
			if (!testing) {
				
				// Eaten by shark
				Debug.Log ("Eaten by shark");
                GlobalStateController.addScore(-40);
            }
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

	void OnMouseDown()
	{

	}

	void OnMouseDrag()
	{

	}

	void OnMouseUp()
	{

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
