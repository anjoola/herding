using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 *  TODO documentation
 */
public class SelectLevelScript : MonoBehaviour {
	// Transition duration from one camera position to another.
	float TRANSITION_DURATION = 0.7f;

	// Camera position and rotation.
	Vector3 cameraOrigPos;
	Quaternion cameraOrigRot;
	
	float CLICK_THRESHOLD = 100f;

	// UI layer components.
	bool isUILayerActive = false;
	public GameObject uiLayer;
	public Text levelName;
	public Text levelScore;
	public RawImage levelImage;
	public Button startButton;
	public Button backButton;
	string selectedLevel;

	//float SCALE_FACTOR = 1.1f;
	//float SCALE_TIME = 1.0f;

	public void goBack() {
		StartCoroutine(MoveCameraLoc(cameraOrigPos, cameraOrigRot, false));
	}
	public void startLevel() {
		Application.LoadLevel(selectedLevel);
	}
	void Start () {
		// Get original camera orientation.
		cameraOrigPos = Camera.main.transform.position;
		cameraOrigRot = Camera.main.transform.rotation;
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePos = Input.mousePosition;
	
			// 
			if (isUILayerActive) {


			}
			// See if the user clicked on a level and try to load that level's information.
			else {
				LoadLevelInfo(mousePos, "Cow Palace", "CowPalace");
				LoadLevelInfo(mousePos, "City", "CowPalace");
			}
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
		// TODO detect clicks on the objects better
		if (Mathf.Abs (objLoc.x - mousePos.x) < CLICK_THRESHOLD &&
		    Mathf.Abs (objLoc.y - mousePos.y) < CLICK_THRESHOLD) {

			// Find the target camera zoom location and move the camera there.
			GameObject cameraZoomLoc = GameObject.Find(levelAssetsName + " Zoom");
			StartCoroutine(MoveCameraLoc(cameraZoomLoc));

			levelName.text = levelAssetsName;
			// TODO score, stars, and image

			selectedLevel = sceneName;
			return true;
		}
		return false;
	}

	/**
	 * Moves the camera location to the target position and rotation.
	 */
	IEnumerator MoveCameraLoc(GameObject target) {
		return MoveCameraLoc(target.transform.position, target.transform.rotation, true);
	}
	IEnumerator MoveCameraLoc(Vector3 targetPos, Quaternion targetRot, bool enabled) {
		if (!enabled) enableUI(false);

		float t = 0.0f;
		Vector3 startingPos = Camera.main.transform.position;
		while (t < 1.0f) {
			t += Time.deltaTime * (Time.timeScale / TRANSITION_DURATION);
			Camera.main.transform.position = Vector3.Lerp(startingPos, targetPos, t);
			// TODO rotation
			yield return 0;
		}

		if (enabled) enableUI(true);
	}

	/**
	 * Displays or hides the UI layer.
	 */
	void enableUI(bool active) {
		uiLayer.SetActive(active);
		isUILayerActive = active;
	}
}
