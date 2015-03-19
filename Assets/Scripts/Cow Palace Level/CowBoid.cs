using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CowBoid : GeneralBoid {
	public static int POINT_PER_BOID = 40;
	public int framesToGo;
	public int FRAMES_STUNNED = 100;

	new public void Start(){
		base.Start ();
		framesToGo = -1;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "DetectionTag") {
			if (!Camera.main.GetComponent<MultiTouchCamera>().testing) {
				GlobalStateController.addScore(POINT_PER_BOID);
				AudioController.playSFX("SingleCow");
            }
			base.Destroy();
        }
	}
}
