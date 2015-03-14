using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredatorController : MultiTouchController {
    private List<Rigidbody2D> _prey;

	public float REPULSION_DISTANCE = 10;
	public float TO_APPLY = 1;

	public string name;


	protected void FixedUpdate () {
		if (GlobalStateController.isPaused && !MultiTouchController.testing) {
			return;
		}
		// Exert repulsion on other boids.
		ExertRepulsion();
	}

	/**
	 * Exerts replusion on the other boids.
	 */
	void ExertRepulsion() {
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
