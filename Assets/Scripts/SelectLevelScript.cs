using UnityEngine;
using System.Collections;

/**
 *  TODO documentation
 */
public class SelectLevelScript : MonoBehaviour {
	// Transition duration from one camera position to another.
	float TRANSITION_DURATION = 0.8f;

	// Camera position and rotation.
	Vector3 cameraOrigPos;
	Quaternion cameraOrigRot;
	
	float CLICK_THRESHOLD = 100f;

	//float SCALE_FACTOR = 1.1f;
	//float SCALE_TIME = 1.0f;

	void Start () {
		// Get original camera orientation.
		cameraOrigPos = Camera.main.transform.position;
		cameraOrigRot = Camera.main.transform.rotation;
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePos = Input.mousePosition;

			// See if the user clicked on a level and try to load that level
			LoadLevelInfo(mousePos, "Cow Palace", "CowPalace");

			//cameraTargetPos = cameraOrigPos;
			//cameraTargetRot = cameraOrigRot;
			//StartCoroutine(MoveCameraLoc());
		}
	}
	/*
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
	}*/

	/**
	 * Loads level information if the user clicks on the right spot.
	 *
	 * mousePos: Where the user clicked.
	 * levelAssetsName: Name of all the assets for this level. For example, if the level is called "Level", then there
	 *                  needs to be objects called "Level Model" and "Level Zoom".
	 * sceneName: Name of the scene associated with this level.
	 */
	bool LoadLevelInfo(Vector3 mousePos, string levelAssetsName, string sceneName) {
		Vector3 objLoc = Camera.main.WorldToScreenPoint(GameObject.Find(levelAssetsName + " Model").transform.position);
		if (Mathf.Abs (objLoc.x - mousePos.x) < CLICK_THRESHOLD &&
		    Mathf.Abs (objLoc.y - mousePos.y) < CLICK_THRESHOLD) {

			// Find the target camera zoom location and move the camera there.
			GameObject cameraZoomLoc = GameObject.Find(levelAssetsName + " Zoom");
			StartCoroutine(MoveCameraLoc(cameraZoomLoc));

			// Show level information.
			//Application.LoadLevel (levelName);
			return true;
		}
		return false;
	}

	/**
	 * Moves the camera location to the target position and rotation.
	 */
	IEnumerator MoveCameraLoc(GameObject target) {
		float t = 0.0f;
		Vector3 startingPos = Camera.main.transform.position;
		Vector3 targetPos = target.transform.position;
		Quaternion targetRot = target.transform.rotation;
		while (t < 1.0f) {
			t += Time.deltaTime * (Time.timeScale / TRANSITION_DURATION);
			Camera.main.transform.position = Vector3.Lerp(startingPos, targetPos, t);
			// TODO rotation
			yield return 0;
		}
	}
}
