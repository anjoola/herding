using UnityEngine;
using System.Collections;

/**
 *  TODO documentation
 */
public class SelectLevelScript : MonoBehaviour {
	float SCALE_FACTOR = 1.1f;
	float SCALE_TIME = 1.0f;

	void Start () {
	
	}
	void Update () {

	}
	void OnMouseEnter() {
		iTween.ScaleBy (gameObject, iTween.Hash (
			"x", SCALE_FACTOR,
			"y", SCALE_FACTOR,
			"z", SCALE_FACTOR,
			"time", SCALE_TIME));
	}
	void OnMouseExit() {
		iTween.ScaleBy (gameObject, iTween.Hash (
			"x", 1/SCALE_FACTOR,
			"y", 1/SCALE_FACTOR,
			"z", 1/SCALE_FACTOR,
			"time", SCALE_TIME));
	}
}
