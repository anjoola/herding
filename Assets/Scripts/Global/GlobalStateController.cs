using UnityEngine;
using System.Collections;

/**
 * Controls:
 *   - Current level
 *   - Loading and saving files
 *   - Value of the timer
 *   - Creating the pause menu and level
 */
public class GlobalStateController : MonoBehaviour {
	public static GameModel currentGame;
	public static Level currentLevel;

	// Menus and UI.
	public static GameObject pauseMenu;
	private static PauseMenuController pauseMenuController;
	public static GameObject levelUI;
	private static LevelUIController levelUIController;
	public static GameObject levelComplete;
	private static LevelCompleteController levelCompleteController;

	// Timer.
	public static int currTime;
	private static bool timerEnabled;

	public static bool isPaused;

	// Volume.
	public static bool isVolumeOn;

	void Awake() {
		// Make sure prefabs are not destroyed.
		DontDestroyOnLoad(transform.gameObject);
		DontDestroyOnLoad(pauseMenu);
		DontDestroyOnLoad(levelUI);
		DontDestroyOnLoad(levelComplete);
	}
	void Start() {
		// Load savefile.
		SaveController.loadGame();

		// Get objects.
		pauseMenu = GameObject.Find("PauseMenu");
		pauseMenuController = pauseMenu.GetComponent("PauseMenuController") as PauseMenuController;
		levelUI = GameObject.Find("LevelUI");
		levelUIController = levelUI.GetComponent("LevelUIController") as LevelUIController;
		levelComplete = GameObject.Find("LevelComplete");
		levelCompleteController = levelComplete.GetComponent("LevelCompleteController") as LevelCompleteController;

		// Hide menus for now.
		enablePauseMenu(false);
		enableLevelUI(false);
		enableLevelComplete(false);
	
		// Disable timer but start the timer thread.
		currTime = 0;
		timerEnabled = false;
		InvokeRepeating("UpdateTimer", 0, 1.0f);

		isPaused = false;
		isVolumeOn = true; // TODO detect ios volume setting
	}
	void OnApplicationQuit() {
		// Save savefile.
		SaveController.saveGame();
	}

	/* --------------------------------------------------- LEVELS ----------------------------------------------------*/
	
	public static void startLevel() {
		currentLevel.start();
		AutoFade.LoadLevel(currentLevel.sceneName, 0.2f, 0.2f, Color.black);

		// TODO don't want to show this until level actually loads
		startTimer(currentLevel.maxTime);
		resetScore();

		enablePauseMenu(false);
		enableLevelUI(true);
		levelUIController.enableMenuButton(true);
		enableLevelComplete(false);
	}
	public static void restartLevel() {
		startLevel();
	}
	public static void pauseLevel() {
		pauseTimer();
		enablePauseMenu(true);
	}
	public static void resumeLevel() {
		enablePauseMenu(false);
		resumeTimer();
	}
	public static void exitLevel() {
		stopTimer();
	
		enableLevelUI(false);
		enablePauseMenu(false);
		enableLevelComplete(false);

		// TODO cleanup for this level?
		AutoFade.LoadLevel("WorldMap", 0.2f, 0.2f, Color.black);
	}
	public static void finishLevel(bool wasTimeUp=false) {
		stopTimer();
		if (wasTimeUp) {
			levelCompleteController.timeUp();
		} else {
			// TODO levelCompleteController.levelComplete();
			levelCompleteController.gameOver();
		}
	
		levelUIController.enableMenuButton(false);
		enablePauseMenu(false);
		enableLevelComplete(true);

		currentLevel.finish();
	}

	/* ---------------------------------------------------- TIMER ----------------------------------------------------*/

	public static void startTimer(int time) {
		currTime = time;
		timerEnabled = true;
		levelUIController.updateTimer(currTime);
	}
	public static void pauseTimer() {
		timerEnabled = false;
	}
	public static void resumeTimer() {
		timerEnabled = true;
	}

	public static void stopTimer() {
		timerEnabled = false;
	}
	/**
	 * Updates the timer every second if it is enabled.
	 */
	void UpdateTimer() {
		if (timerEnabled && levelUI.activeSelf) {
			currTime--;
			levelUIController.updateTimer(currTime);

			// Timer up!
			if (currTime == 0) {
				timerEnabled = false;
				// TODO send signal that timer stopped

				finishLevel(true);
			}
		}
	}

	/* ---------------------------------------------------- SCORE ----------------------------------------------------*/

	public static void resetScore() {
		currentLevel.score = 0;
		levelUIController.updateScore(0);
	}
    public static void addScore(int score) {
		currentLevel.incrementScore(score);
		levelUIController.updateScore(currentLevel.score);
	}

	/* --------------------------------------------------- VOLUME ----------------------------------------------------*/

	public static void turnVolumeOn() {
		isVolumeOn = true;
		// TODO ios-specific tihngs
	}
	public static void turnVolumeOff() {
		isVolumeOn = false;
		// TODO ios-specific things
	}

	/* ----------------------------------------------------- UI ------------------------------------------------------*/

	/**
	 * Enable and disable UI.
	 */
	public static void enablePauseMenu(bool enabled) {
		if (currentLevel == null && enabled) return;
		
		isPaused = enabled;

		if (enabled) {
			pauseMenuController.updateText(currentLevel.assetsName, currentLevel.score);
			pauseMenuController.slideIn();
		}
		else {
			GeneralBoid.UnPauseBoids();
			pauseMenuController.slideOut();
		}
	}
	public static void enableLevelUI(bool enabled) {
		if (currentLevel == null && enabled) return;

		levelUI.SetActive(enabled);
	}
	public static void enableLevelComplete(bool enabled) {
		levelComplete.SetActive(enabled);
	}
}
