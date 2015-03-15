using UnityEngine;
using System.Collections;

public class FishBoid : GeneralBoid {
	public float TUNE;
	private int prevFrameCollide;
	int numFramesBeforeNotInCollision;
	int speedHash;

	private Vector2 prevPosition;
	Animator animator;

	new void Start (){
		speedHash = Animator.StringToHash("speed");

		animator = GetComponent<Animator> ();
		base.forceMag = 2.0f;
		base.Start();
		Physics2D.gravity = new Vector2(5, 0);
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Fish entered target zone.
		if (other.gameObject.tag == "Food Tag") {
			if (!MultiTouchCamera.testing) {
				GlobalStateController.addScore(40);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Food Tag") {
			// Fish left target zone.
			if (!MultiTouchCamera.testing) {
                GlobalStateController.addScore(-40);
            }
        }
	}

	void Update () {
		Vector2 currPosition = rigidbody2D.position;
		float distTraveled = Vector2.SqrMagnitude (currPosition - prevPosition);
		prevPosition = currPosition;
		animator.SetFloat (speedHash, Mathf.Abs(TUNE * distTraveled));
		if (base.inCollision) {
			numFramesBeforeNotInCollision--;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag.Equals("WallTag") && numFramesBeforeNotInCollision <= 0) {
			base.inCollision = true;
			numFramesBeforeNotInCollision = 100;
		}
	}
}
