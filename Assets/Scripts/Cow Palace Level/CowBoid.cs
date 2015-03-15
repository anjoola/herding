using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CowBoid : GeneralBoid {
	public static int POINT_PER_BOID = 40;

	void OnCollisionEnter2D(Collision2D col) {
		//rigidbody2D.velocity = new Vector2(0, 0);
		//rigidbody2D.AddForce(Random.onUnitSphere * 1.0f, ForceMode2D.Impulse);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "DetectionTag") {
			if (!MultiTouchCamera.testing) {
				GlobalStateController.addScore(POINT_PER_BOID);
				AudioController.playSFX("SingleCow");
            }
			base.Destroy();
        }
	}
}
