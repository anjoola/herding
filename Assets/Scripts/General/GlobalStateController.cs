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

	// Timer.
	public static int currTime;
	private static bool timerEnabled;

	public static bool isPaused;

	void Awake() {
		// Make sure prefabs are not destroyed.
		DontDestroyOnLoad(transform.gameObject);
		DontDestroyOnLoad(pauseMenu);
		DontDestroyOnLoad(levelUI);
	}
	void Start() {
		// Load savefile.
		SaveController.loadGame();

		// Get objects.
		pauseMenu = GameObject.Find("PauseMenu");
		pauseMenuController = pauseMenu.GetComponent("PauseMenuController") as PauseMenuController;
		levelUI = GameObject.Find("LevelUI");
		levelUIController = levelUI.GetComponent("LevelUIController") as LevelUIController;

		// Hide menus for now.
		enablePauseMenu(false);
		enableLevelUI(false);
	
		// Disable timer but start the timer thread.
		currTime = 0;
		timerEnabled = false;
		InvokeRepeating("UpdateTimer", 0, 1.0f);

		isPaused = false;
	}
	void OnApplicationQuit() {
		// Save savefile.
		SaveController.saveGame();
	}

	/* --------------------------------------------------- LEVELS ----------------------------------------------------*/

	public static void startLevel() {
		currentLevel.start();
	
		Application.LoadLevel(currentLevel.sceneName);

		startTimer(currentLevel.maxTime);
		resetScore();
		enablePauseMenu(false);
		enableLevelUI(true);
	}
	public static void pauseLevel() {
//		SendMessage("Pause");
		pauseTimer();
		enablePauseMenu(true);
	}
	public static void resumeLevel() {
		enablePauseMenu(false);
		resumeTimer();
	}
	public static void restartLevel() {
		//SendMessage("restartLevel"); TODO
		// TODO pretty similar to startLevel
		currentLevel.start();
	
		startTimer(currentLevel.maxTime);
		resetScore();
		enablePauseMenu(false);
		enableLevelUI(true);
	}
	public static void exitLevel() {
		stopTimer();
		enableLevelUI(false);
		enablePauseMenu(false);
		
		// TODO cleanup for this level?
		Application.LoadLevel("WorldMap");
	}
	public static void finishLevel() {
		currentLevel.finish();

		exitLevel();
	}

	/* ---------------------------------------------------- TIMER ----------------------------------------------------*/

	public static void startTimer(int time) {
//		Time.timeScale = 1;
		currTime = time;
		timerEnabled = true;
		levelUIController.updateTimer(currTime);
	}
	public static void pauseTimer() {
//		Time.timeScale = 0;
		timerEnabled = false;
	}
	public static void resumeTimer() {
//		Time.timeScale = 1;
		timerEnabled = true;
	}

	public static void stopTimer() {
//		Time.timeScale = 0;
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
			}

			// TODO remove, only for demo purposes
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

	/* ------------------------------------------------------ UI -----------------------------------------------------*/

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
}
