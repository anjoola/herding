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
	public static GameObject instance;
	public static GameModel currentGame;
	public static Level currentLevel;

	// Menus and UI.
	public static GameObject pauseMenu;
	private static PauseMenuController pauseMenuController;
	public static GameObject levelUI;
	private static LevelUIController levelUIController;
	public static GameObject levelComplete;
	private static LevelCompleteController levelCompleteController;
	public static GameObject notes;
	private static NotesController notesController;

	public static bool isPaused;
	public static bool showNotesPaused;

	// Timer.
	public static int currTime;
	private static bool timerEnabled;

	void Awake() {
		instance = this.gameObject;

		// Get objects.
		pauseMenu = GameObject.Find("PauseMenu");
		pauseMenuController = pauseMenu.GetComponent("PauseMenuController") as PauseMenuController;
		levelUI = GameObject.Find("LevelUI");
		levelUIController = levelUI.GetComponent("LevelUIController") as LevelUIController;
		levelComplete = GameObject.Find("LevelComplete");
		levelCompleteController = levelComplete.GetComponent("LevelCompleteController") as LevelCompleteController;
		notes = GameObject.Find("NotesArea");
		notesController = notes.GetComponent("NotesController") as NotesController;

		// Make sure prefabs are not destroyed.
		DontDestroyOnLoad(transform.gameObject);
		DontDestroyOnLoad(pauseMenu);
		DontDestroyOnLoad(levelUI);
		DontDestroyOnLoad(levelComplete);
		DontDestroyOnLoad(notes);
	}
	void Start() {
		// Load savefile.
		SaveController.loadGame();
	
		// Hide menus for now.
		enablePauseMenu(false, true);
		enableLevelUI(false);
		enableLevelComplete(false);
		hideNotes();
	
		// Disable timer but start the timer thread.
		currTime = 0;
		timerEnabled = false;
		InvokeRepeating("UpdateTimer", 0, 1.0f);

		isPaused = false;
	}
	void Update() {
		// Dismiss notes if user taps.
		if (Input.GetMouseButtonDown(0)) {
			if (notes.activeSelf) {
				hideNotes();
			}
		}
	}
	void OnApplicationQuit() {
		// Save savefile.
		SaveController.saveGame();
	}

	/* --------------------------------------------------- LEVELS ----------------------------------------------------*/
	
	public static void startLevel() {
		currentLevel.start();
		resetScore();

		enablePauseMenu(false, true);
		AutoFade.LoadLevel(currentLevel.sceneName, 0.2f, 0.2f, Color.black, loadLevelUI);
		AudioController.resumeVolume();
		enableLevelComplete(false);
	}
	public static void loadLevelUI() {
		if (currentLevel == null) return;

		enableLevelUI(true);
		levelUIController.enableMenuButton(true);

		startTimer(currentLevel.maxTime);
	}
	public static void restartLevel() {
		AudioController.resumeVolume();
		startLevel();
	}
	public static void pauseLevel() {
		pauseTimer();
		enablePauseMenu(true);
		AudioController.halfVolume();
	}
	public static void resumeLevel() {
		enablePauseMenu(false);
		resumeTimer();
		AudioController.resumeVolume();
	}
	public static void exitLevel() {
		hideNotes();
		stopTimer();
	
		enableLevelUI(false);
		enablePauseMenu(false, true);
		enableLevelComplete(false);

		// TODO cleanup for this level?
		AutoFade.LoadLevel("WorldMap", 0.2f, 0.2f, Color.black);
		AudioController.resumeVolume();
	}
	public static void finishLevel(bool wasTimeUp=false) {
		hideNotes();

		AudioController.resumeVolume();
		AudioController.playAudio("LevelComplete", false);
		stopTimer();
		if (wasTimeUp) {
			levelCompleteController.timeUp();
		} else {
			// TODO levelCompleteController.levelComplete();
			levelCompleteController.gameOver();
		}
	
		levelUIController.enableMenuButton(false);
		enablePauseMenu(false, true);
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
		if (timerEnabled && levelUI.activeSelf && !showNotesPaused) {
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

	/* ---------------------------------------------------- NOTES ----------------------------------------------------*/

	public static void showNotes(string note, bool pause=false) {
		notes.SetActive(true);
		notesController.setText(note);
		// TODO nice fade in effect

		if (pause) {
			showNotesPaused = true;
			GeneralBoid.PauseBoids();
		}
	}
	public static void hideNotes() {
		// TODO nice fade out effect
		notes.SetActive(false);

		if (showNotesPaused) {
			GeneralBoid.UnPauseBoids();
			showNotesPaused = false;
		}
	}

	/* ---------------------------------------------------- SCORE ----------------------------------------------------*/

	public static void resetScore() {
		if (currentLevel == null) return;
		currentLevel.score = 0;
		levelUIController.updateScore(0);
	}
	public static void addScore(int score) {
		if (currentLevel == null) return;
		currentLevel.incrementScore(score);
		levelUIController.updateScore(currentLevel.score);
	}

	/* ----------------------------------------------------- UI ------------------------------------------------------*/

	/**
	 * Enable and disable UI.
	 */
	public static void enablePauseMenu(bool enabled, bool hurry=false) {
		if (currentLevel == null && enabled) return;
		
		isPaused = enabled;

		if (enabled) {
			pauseMenuController.updateText(currentLevel.assetsName, currentLevel.score);
			pauseMenuController.slideIn();
		}
		else {
			GeneralBoid.UnPauseBoids();
			pauseMenuController.slideOut(hurry);
		}
	}
	public static void enableLevelUI(bool enabled) {
		if (currentLevel == null && enabled) return;

		levelUI.SetActive(enabled);
	}
	public static void enableLevelComplete(bool enabled) {
		if (enabled) {
			levelCompleteController.slideIn();
		} else {
			levelCompleteController.slideOut();
		}
	}
}
