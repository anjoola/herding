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
	float ITWEEN_DISTANCE = 300;

	// Camera positions.
	Vector3 cameraOrigPos;
	Vector3 previousZoomPos;

	// TODO remove
	float CLICK_THRESHOLD = 100f;

	// Level UI layer components.
	public GameObject levelNamePanel;
	private Transform levelNamePos;
	public GameObject levelInfoPanel;
	private Transform levelInfoPos;
	public Text levelName;
	public GameObject[] stars;
	public Text levelScore;

	// Navigation within the world map
	public GameObject subworldNav;
	public GameObject rightZoom;
	public GameObject leftZoom;
	public GameObject rightBack;
	public GameObject leftBack;
	public GameObject zoomBackToSubworldButton;
	public GameObject cowLevelMarkers;
	public GameObject waterLevelMarkers;
	bool isZoomedOut;
	bool isLeftSubworld;
	bool focusedOnLevel;

	void Start () {
		levelNamePos = levelNamePanel.transform;
		levelInfoPos = levelInfoPanel.transform;
		zoomBackToSubworldButton.SetActive(false);

		enableLevelNameUI(false, true);
		enableLevelInfoUI(false, true);
		rightBack.SetActive(false);
		leftBack.SetActive(false);
		waterLevelMarkers.SetActive(false);
		cowLevelMarkers.SetActive(false);

		// Get original camera orientation.
		cameraOrigPos = Camera.main.transform.position;
		isZoomedOut = true;
		isLeftSubworld = false;

		if (!GlobalStateController.currentGame.played) {
			GlobalStateController.showNotes("Welcome to Overrun! Choose a world by tapping either left or right.");
			GlobalStateController.currentGame.played = true;
		}
		AudioController.playAudio("WorldMapMusic");
	}
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePos = Input.mousePosition;
			bool clickedOnLevel = false;

			// See if the user clicked on a level and try to load that level's information.
			if (!isZoomedOut && !focusedOnLevel) {
				foreach (Level level in GlobalStateController.currentGame.levels) {
					clickedOnLevel |= loadLevelInfo(mousePos, level);
				}
			}
			if (clickedOnLevel) {
				setArrows(false);
				zoomBackToSubworldButton.SetActive(true);
			}
		}
	}

	/* ----------------------------------------------- Button Handlers ---------------------------------------------- */

	/**
	 * User clicked on left subworld.
	 */
	public void leftButton() {
		previousZoomPos = leftZoom.transform.position;
		zoomIntoSubworld(true);
	}
	/**
	 * User clicked on right subworld.
	 */
	public void rightButton() {
		previousZoomPos = rightZoom.transform.position;
		zoomIntoSubworld(false);
	}
	/**
	 * Camera zoom into a subworld once a user clicks on it.
	 *
	 * isLeftSubworld: True if the user clicked on the left subworld.
	 */
	private void zoomIntoSubworld(bool isLeftSubworld) {
		subworldNav.SetActive(false);
		isZoomedOut = false;
		this.isLeftSubworld = isLeftSubworld;
		setArrows();

		StartCoroutine(MoveCameraLoc(previousZoomPos, false));
	}
	/**
	 * Camera zoom out of the subworld.
	 */
	public void zoomOutOfSubworld() {
		isZoomedOut = true;
		setArrows();
		subworldNav.SetActive(true);

		StartCoroutine(MoveCameraLoc(cameraOrigPos, false));
	}
	private void setArrows(bool directSet=true) {
		if (!isZoomedOut && directSet) {
			leftBack.SetActive(!isLeftSubworld);
			rightBack.SetActive(isLeftSubworld);
		}
		else {
			waterLevelMarkers.SetActive(false);
			cowLevelMarkers.SetActive(false);
			leftBack.SetActive(false);
			rightBack.SetActive(false);
		}
	}
	public void zoomBackIntoSubworld() {
		setArrows();
		focusedOnLevel = false;
		StartCoroutine(MoveCameraLoc(previousZoomPos, false));
		zoomBackToSubworldButton.SetActive(false);
	}
		/**
	 * Goes back to the world map if on the level info layer.
	 */
	public void goBack() {
		StartCoroutine(MoveCameraLoc(cameraOrigPos, false));
		GlobalStateController.currentLevel = null;
	}
	/**
	 * Starts a level. Hides the level information UI.
	 */
	public void startLevel() {
		GlobalStateController.startLevel();
		enableLevelNameUI(false, false);
		enableLevelInfoUI(false, false);
	}

	/* ---------------------------------------- World Map Level Info Display ---------------------------------------- */

	void enableLevelInfoUI(bool active, bool hurry=false) {
		float time = hurry ? 0 : TRANSITION_DURATION;
		if (hurry && !active) {
			levelInfoPanel.SetActive(active);
			levelInfoPanel.transform.position = new Vector3(levelInfoPanel.transform.position.x, 
			                                                levelInfoPanel.transform.position.y-ITWEEN_DISTANCE,
			                                                levelInfoPanel.transform.position.z);
		}
		if (!hurry && active) {
			if (levelInfoPanel.activeSelf) return;
			levelInfoPanel.SetActive(active);
			iTween.MoveBy(levelInfoPanel, iTween.Hash("y", -ITWEEN_DISTANCE, "easeType", "linear", "loopType", "none",
			                                          "delay", 0.0, "time", time));
		} else if (!hurry) {
			if (!levelInfoPanel.activeSelf) return;
			iTween.MoveBy(levelInfoPanel, iTween.Hash("y", ITWEEN_DISTANCE, "easeType", "linear", "loopType", "none",
			                                          "delay", 0.0, "time", time,
			                                          "oncomplete", "onDisableLevelInfoPanelComplete",
			                                          "oncompletetarget", this.gameObject));
		}
	}
	private void onDisableLevelInfoPanelComplete() {
		levelInfoPanel.SetActive(false);
	}
	/**
	 * Displays or hides the world map UI layer.
	 */
	void enableLevelNameUI(bool active, bool hurry=false) {
		float time = hurry ? 0 : TRANSITION_DURATION;
		if (hurry && !active) {
			levelNamePanel.SetActive(active);
			levelNamePanel.transform.position = new Vector3(levelNamePos.position.x, 
			                                                levelNamePos.position.y + ITWEEN_DISTANCE,
			                                                levelNamePos.position.z);
		}
		if (!hurry && active) {
			if (levelNamePanel.activeSelf) return;
			levelNamePanel.SetActive(active);
			iTween.MoveTo(levelNamePanel, iTween.Hash("y", levelNamePos.position.y - ITWEEN_DISTANCE,
			                                          "easeType", "linear", "loopType", "none",
			                                          "delay", 0.0, "time", time,
			                                          "oncomplete", "onEnableLevelNameUIComplete",
			                                          "oncompletetarget", this.gameObject));
		} else if (!hurry) {
			if (!levelNamePanel.activeSelf) return;
			iTween.MoveTo(levelNamePanel, iTween.Hash("y", levelNamePos.position.y + ITWEEN_DISTANCE, "easeType", "linear", "loopType", "none",
			                                          "delay", 0.0, "time", time,
			                                          "oncomplete", "onDisableLevelNameUIComplete",
													  "oncompletetarget", this.gameObject));
		}
	}
	private void onEnableLevelNameUIComplete() {
		levelNamePanel.SetActive(true);
	}
	private void onDisableLevelNameUIComplete() {
		levelNamePanel.SetActive(false);
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
		focusedOnLevel = true;

		// Level name, score, stars.
		levelName.text = level.assetsName;
		levelScore.text = "High Score: " + level.highScore;
		for (int i = 0; i < level.numStars; i++) {
			stars[i].SetActive(true);
		}
		for (int i = level.numStars; i < 5; i++) {
			stars[i].SetActive(false);
		}
		GlobalStateController.currentLevel = level;
	}

	/* --------------------------------------------------- Camera --------------------------------------------------- */

	/**
	 * Moves the camera location to the target game object.
	 */
	IEnumerator MoveCameraLoc(GameObject target) {
		return MoveCameraLoc(target.transform.position, true);
	}

	/**
	 * Moves the camera location to the target position.
	 */
	IEnumerator MoveCameraLoc(Vector3 targetPos, bool enabled) {
		enableLevelNameUI(enabled);
		enableLevelInfoUI(enabled);
		if (enabled) {
			waterLevelMarkers.SetActive(false);
			cowLevelMarkers.SetActive(false);
		}

		float t = 0.0f;
		Vector3 startingPos = Camera.main.transform.position;
		while (t < 1.0f) {
			t += Time.deltaTime * (Time.timeScale / TRANSITION_DURATION);
			Camera.main.transform.position = Vector3.Lerp(startingPos, targetPos, t);
			yield return 0;
		}

		if (!enabled && !isZoomedOut) {
			if (isLeftSubworld) waterLevelMarkers.SetActive(true);
			else cowLevelMarkers.SetActive(true);
		}
	}
}
