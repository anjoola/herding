using UnityEngine;
using System.Collections;

public class FishBoid : GeneralBoid {
	public static int POINT_PER_BOID = 40;
	public float TUNE;
	private int prevFrameCollide;
	int numFramesBeforeNotInCollision;
	int speedHash;
	public float FORCEMAG = 2.0f;

	private Vector2 prevPosition;
	Animator myAnimator;

	new void Start (){
		speedHash = Animator.StringToHash("speed");

		myAnimator = GetComponent<Animator>();
		base.forceMag = FORCEMAG;
		base.Start();
		Physics2D.gravity = new Vector2(5, 0);
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Fish entered target zone.
		if (other.gameObject.tag == "DetectionRemove") {
			AudioController.playSFX("CrabAww");
			Destroy();
			GlobalStateController.addScore(POINT_PER_BOID);
		}
		else if (other.gameObject.tag == "DetectionTag") {
			AudioController.playSFX("Bubbles");
			if (!Camera.main.GetComponent<MultiTouchCamera>().testing) {
				GlobalStateController.addScore(POINT_PER_BOID);
			}
		}
	}


	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "DetectionTag") {
			// Fish left target zone.
			if (!Camera.main.GetComponent<MultiTouchCamera>().testing) {
				GlobalStateController.addScore(-POINT_PER_BOID);
            }
        }
	}

	void Update () {
		Vector2 currPosition = rigidbody2D.position;
		float distTraveled = Vector2.SqrMagnitude (currPosition - prevPosition);
		prevPosition = currPosition;
		// TODO myAnimator.SetFloat(speedHash, Mathf.Abs(TUNE * distTraveled));
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
