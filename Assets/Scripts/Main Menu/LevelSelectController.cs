using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 *  Script for selecting levels
 *    - Show level description UI when clicking on a level
 *    - Detecting taps on level objects
 *    - Starts a level
 */
public class LevelSelectController : MonoBehaviour {
	// Transition duration from one camera position to another.
	float TRANSITION_DURATION = 0.7f;

	// Camera position and rotation.
	Vector3 cameraOrigPos;
	Quaternion cameraOrigRot;
	
	float CLICK_THRESHOLD = 100f;

	// UI layer components.
	public GameObject uiLayer;
	public Text levelName;
	public RawImage levelImage;
	public GameObject[] stars;
	public Text levelScore;
	public Button backButton;
	public Button startButton;
	public GameObject selectLevelText;

	void Start () {
		// Get original camera orientation.
		cameraOrigPos = Camera.main.transform.position;
		cameraOrigRot = Camera.main.transform.rotation;

		iTween.MoveBy(GameObject.Find("Select a Level Text"),
		              iTween.Hash("y", -2, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
	}
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePos = Input.mousePosition;

			// See if the user clicked on a level and try to load that level's information.
			if (!uiLayer.activeSelf) {
				foreach (Level level in GlobalStateController.currentGame.levels) {
					loadLevelInfo(mousePos, level);
				}
			}
		}
	}

	/* ----------------------------------------------- Button Handlers ---------------------------------------------- */

	/**
	 * Goes back to the world map if on the UI layer.
	 */
	public void goBack() {
		StartCoroutine(MoveCameraLoc(cameraOrigPos, cameraOrigRot, false));
		GlobalStateController.currentLevel = null;
	}

	/**
	 * Starts a level. Hides the world map UI.
	 */
	public void startLevel() {
		enableWorldMapUI(false);
		GlobalStateController.startLevel();
	}

	/* ---------------------------------------- World Map Level Info Display ---------------------------------------- */

	/**
	 * Displays or hides the world map UI layer.
	 */
	void enableWorldMapUI(bool active) {
		selectLevelText.SetActive(!active);
		uiLayer.SetActive(active);
	}

	/**
	 * Loads level information if the user clicks on the right spot.
	 *
	 * mousePos: Where the user clicked.
	 * levelAssetsName: Name of all the assets for this level. For example, if the level is called "Level", then there
	 *                  needs to be objects called "Level Model" and "Level Zoom".
	 * sceneName: Name of the scene associated with this level.
	 */
	bool loadLevelInfo(Vector3 mousePos, Level level) {
		Vector3 objLoc = Camera.main.WorldToScreenPoint(
			GameObject.Find(level.assetsName + " Model").transform.position);
		// TODO detect clicks on the objects better
		if (Mathf.Abs (objLoc.x - mousePos.x) < CLICK_THRESHOLD &&
		    Mathf.Abs (objLoc.y - mousePos.y) < CLICK_THRESHOLD) {

			// Find the target camera zoom location and move the camera there.
			GameObject cameraZoomLoc = GameObject.Find(level.assetsName + " Zoom");
			StartCoroutine(MoveCameraLoc(cameraZoomLoc));

			setLevelInfo(level);
			return true;
		}
		return false;
	}

	/**
	 * Set the level information in the UI.
	 */
	void setLevelInfo(Level level) {
		// Level name, score, stars, image.
		levelName.text = level.assetsName;
		levelScore.text = "High Score: " + level.highScore;
		for (int i = 0; i < level.numStars; i++) {
			stars[i].SetActive(i < level.numStars);
		}
		// TODO image
		
		GlobalStateController.currentLevel = level;
	}

	/* --------------------------------------------------- Camera --------------------------------------------------- */

	/**
	 * Moves the camera location to the target game object.
	 */
	IEnumerator MoveCameraLoc(GameObject target) {
		return MoveCameraLoc(target.transform.position, target.transform.rotation, true);
	}

	/**
	 * Moves the camera location to the target position and rotation.
	 */
	IEnumerator MoveCameraLoc(Vector3 targetPos, Quaternion targetRot, bool enabled) {
		if (!enabled) enableWorldMapUI(false);

		float t = 0.0f;
		Vector3 startingPos = Camera.main.transform.position;
		while (t < 1.0f) {
			t += Time.deltaTime * (Time.timeScale / TRANSITION_DURATION);
			Camera.main.transform.position = Vector3.Lerp(startingPos, targetPos, t);
			// TODO rotation?
			yield return 0;
		}

		if (enabled) enableWorldMapUI(true);
	}
}
