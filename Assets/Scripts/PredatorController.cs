using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredatorController : Character {
    private List<Rigidbody2D> _prey;

	public float REPULSION_DISTANCE = 8;
	public float TO_APPLY = 1;

	protected void FixedUpdate () {
		// TODO pause shark here
		if (GlobalStateController.shouldPause() && !Camera.main.GetComponent<MultiTouchCamera>().testing) {
			return;
		}
		// Exert repulsion on other boids.
		ExertRepulsion();
	}

	/**
	 * Exerts replusion on the other boids.
	 */
	void ExertRepulsion() {
		if (GeneralBoid.boidRigidbodies == null) return;

		for (int i = 0; i < GeneralBoid.boidRigidbodies.Count; i++) {
			float dist = Vector2.Distance(rigidbody2D.position, GeneralBoid.boidRigidbodies[i].position);
			Vector2 forceDir = GeneralBoid.boidRigidbodies[i].position - rigidbody2D.position;

			if (dist < REPULSION_DISTANCE && dist > 0) {
				Vector2 force = forceDir * TO_APPLY * Time.fixedDeltaTime;
				GeneralBoid.boidRigidbodies[i].AddForce(force);
			}
		}
	}
}
