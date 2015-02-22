using UnityEngine;
using System.Collections;

/**
 *  TODO documentation
 */
public class SelectLevelScript : MonoBehaviour {
	float SCALE_FACTOR = 1.1f;
	float SCALE_TIME = 1.0f;
	float CLICK_THRESHOLD = 100f;

	Vector3 cowPalaceLevel;

	void Start () {
		cowPalaceLevel = Camera.main.WorldToScreenPoint(GameObject.Find ("Cow Palace Level").transform.position);
	}
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePos = Input.mousePosition;

			MaybeLoadLevel (mousePos, cowPalaceLevel, "CowPalace");
		}
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

	/**
	 * Possibly loads a level if the user clicks on the right spot.
	 *
	 * mousePos: Where the use clicked.
	 * levelPos: Location of the on-screen object representing the level.
	 * levelName: Name of the level.
	 */
	void MaybeLoadLevel(Vector3 mousePos, Vector3 levelPos, string levelName) {
		if (Mathf.Abs (levelPos.x - mousePos.x) < CLICK_THRESHOLD &&
		    Mathf.Abs (levelPos.y - mousePos.y) < CLICK_THRESHOLD) {
			Application.LoadLevel (levelName);
		}
	}
}
