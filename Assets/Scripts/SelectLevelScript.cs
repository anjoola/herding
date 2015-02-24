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
	public RawImage levelImage;
	public GameObject[] stars;
	public Text levelScore;
	public Button backButton;
	public Button startButton;
	public GameObject selectLevel;

	// The current level.
	public static Level currentLevel;

	void Start () {
		// Get original camera orientation.
		cameraOrigPos = Camera.main.transform.position;
		cameraOrigRot = Camera.main.transform.rotation;

		iTween.MoveBy(GameObject.Find("Level Select Info"),
		              iTween.Hash("y", -10, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
	}
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePos = Input.mousePosition;

			// See if the user clicked on a level and try to load that level's information.
			if (!isUILayerActive) {
				foreach (Level level in GlobalState.currentGame.levels) {
					LoadLevelInfo(mousePos, level);
				}
			}
		}
	}

	/**
	 * Goes back to the world map if on the Ui layer.
	 */
	public void goBack() {
		StartCoroutine(MoveCameraLoc(cameraOrigPos, cameraOrigRot, false));
		GlobalState.currentLevel = null;
	}
	/**
	 * Starts a level.
	 */
	public void startLevel() {
		Application.LoadLevel(GlobalState.currentLevel.sceneName);
		enableUI(false);
	}

	/**
	 * Loads level information if the user clicks on the right spot.
	 *
	 * mousePos: Where the user clicked.
	 * levelAssetsName: Name of all the assets for this level. For example, if the level is called "Level", then there
	 *                  needs to be objects called "Level Model" and "Level Zoom".
	 * sceneName: Name of the scene associated with this level.
	 */
	bool LoadLevelInfo(Vector3 mousePos, Level level) {
		Vector3 objLoc = Camera.main.WorldToScreenPoint(
			GameObject.Find(level.assetsName + " Model").transform.position);
		// TODO detect clicks on the objects better
		if (Mathf.Abs (objLoc.x - mousePos.x) < CLICK_THRESHOLD &&
		    Mathf.Abs (objLoc.y - mousePos.y) < CLICK_THRESHOLD) {

			// Find the target camera zoom location and move the camera there.
			GameObject cameraZoomLoc = GameObject.Find(level.assetsName + " Zoom");
			StartCoroutine(MoveCameraLoc(cameraZoomLoc));

			// Level name, score, stars, image.
			levelName.text = level.assetsName;
			levelScore.text = level.getScore();
			for (int i = 0; i < level.numStars; i++) {
				stars[i].SetActive(i < level.numStars);
			}
			// TODO image
		
			GlobalState.currentLevel = level;
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
			// TODO rotation?
			yield return 0;
		}

		if (enabled) enableUI(true);
	}

	/**
	 * Displays or hides the UI layer.
	 */
	void enableUI(bool active) {
		selectLevel.SetActive(!active);
		uiLayer.SetActive(active);
		isUILayerActive = active;
	}
}
