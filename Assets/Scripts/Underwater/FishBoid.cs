using UnityEngine;
using System.Collections;

public class FishBoid : GeneralBoid {
	int numFramesBeforeNotInCollision;

	new void Start (){
		base.forceMag = 2.0f;
		base.Start();
		Physics2D.gravity = new Vector2(5, 0);
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Fish entered target zone.
		if (other.gameObject.tag == "Food Tag") {
			if (!testing) {
				GlobalStateController.addScore(40);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Food Tag") {
			// Fish left target zone.
			if (!testing) {
                GlobalStateController.addScore(-40);
            }
        }
	}

	void Update () {
		if (base.inCollision) {
			// TODO remove?
//			if (numFramesBeforeNotInCollision <= 0)
//			{
//				base.inCollision = false;
//			}
			numFramesBeforeNotInCollision--;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag.Equals("WallTag")) {
			base.inCollision = true;
			numFramesBeforeNotInCollision = 100;
		}
	}
}
