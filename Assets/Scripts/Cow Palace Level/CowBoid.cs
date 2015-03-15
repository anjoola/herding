using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CowBoid : GeneralBoid {
	public static int POINT_PER_BOID = 40;
	public int framesToGo;
	public int FRAMES_STUNNED = 100;

	public void Start(){
		base.Start ();
		framesToGo = -1;
	}


//	void OnCollisionEnter2D(Collision2D col) {
//		if (col.gameObject.tag == "WallTag") {
//			base.Pause ();
//			framesToGo = FRAMES_STUNNED;
//			gameObject.tag = "WallTag";
//		}
//		//rigidbody2D.velocity = new Vector2(0, 0);
//		//rigidbody2D.AddForce(Random.onUnitSphere * 1.0f, ForceMode2D.Impulse);
//	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "DetectionTag") {
			if (!MultiTouchCamera.testing) {
				GlobalStateController.addScore(POINT_PER_BOID);
				AudioController.playSFX("SingleCow");
            }
			base.Destroy();
        }
	}

//	void FixedUpdate(){
//
//		// Not ready to move yet!
//		if (framesToGo > 0) return;
//		base.FixedUpdate ();
//	}
//
//	void Update(){
//		if (framesToGo == 0) {
//			base.Unpause ();
//			gameObject.tag = "Untagged";
//			framesToGo--;
//		} else if (framesToGo > 0) {
//			framesToGo--;
//		}
//	}
}
