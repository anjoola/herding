﻿using UnityEngine;
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

	// Camera position.
	Vector3 cameraOrigPos;

	float CLICK_THRESHOLD = 100f;

	// UI layer components.
	public GameObject uiLayer;
	public Text levelName;
	public RawImage levelImage;
	public GameObject[] stars;
	public Text levelScore;

	public GameObject levelMarkers;

	void Start () {
		enableWorldMapUI(false, true);

		// Get original camera orientation.
		cameraOrigPos = Camera.main.transform.position;

		if (!GlobalStateController.currentGame.played) {
			GlobalStateController.showNotes("Welcome to OVERRUN! Choose a level by tapping on any object with a marker!");
			GlobalStateController.currentGame.played = true;
		}

		AudioController.playAudio("WorldMapMusic");
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
		StartCoroutine(MoveCameraLoc(cameraOrigPos, false));
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
	void enableWorldMapUI(bool active, bool hurry=false) {
		float time = hurry ? 0 : TRANSITION_DURATION;
		if (hurry && !active) {
			uiLayer.SetActive(active);
			uiLayer.transform.position = new Vector3(uiLayer.transform.position.x+450, 
			                                         uiLayer.transform.position.y,
			                                         uiLayer.transform.position.z);
		}
		if (!hurry && active) {
			uiLayer.SetActive(active);
			iTween.MoveBy(uiLayer, iTween.Hash("y", -450, "easeType", "linear", "loopType", "none", "delay", 0.0,
			                                   "time", time));
		} else if (!hurry) {
			iTween.MoveBy(uiLayer, iTween.Hash("y", 450, "easeType", "linear", "loopType", "none", "delay", 0.0,
			                                   "time", time, "oncomplete", "onDisableWorldMapCompleted",
			                                   "oncompletetarget", this.gameObject));
		}
	}
	public void onDisableWorldMapCompleted() {
		uiLayer.SetActive(false);
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
			stars[i].SetActive(true);
		}
		for (int i = level.numStars; i < 5; i++) {
			stars[i].SetActive(false);
		}
		// TODO image
		
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
		enableWorldMapUI(enabled);
		if (enabled) levelMarkers.SetActive(false);

		float t = 0.0f;
		Vector3 startingPos = Camera.main.transform.position;
		while (t < 1.0f) {
			t += Time.deltaTime * (Time.timeScale / TRANSITION_DURATION);
			Camera.main.transform.position = Vector3.Lerp(startingPos, targetPos, t);
			yield return 0;
		}

		if (!enabled) levelMarkers.SetActive(true);
	}
}
