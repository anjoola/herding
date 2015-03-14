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

	// Pauses.
	public static bool isPaused;
	public static bool showNotesPaused;
	public static bool timeUpPaused;

	// Timer.
	public static int currTime;
	private static bool timerEnabled;

	public enum CompletionType {
		GameOver,
		TimeUp,
		LevelComplete
	};

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
		hideNotes(true);
	
		// Disable timer but start the timer thread.
		currTime = 0;
		timerEnabled = false;
		InvokeRepeating("UpdateTimer", 0, 1.0f);

		isPaused = false;
	}
	void OnApplicationQuit() {
		SaveController.saveGame();
	}
	void Update() {
		if (Input.GetMouseButtonDown(0) && notes.activeSelf && notesController.dismissAnywhere) {
			hideNotes(false);
		}
	}

	public static bool shouldPause() {
		return isPaused || showNotesPaused || timeUpPaused;
	}

	/* --------------------------------------------------- LEVELS ----------------------------------------------------*/
	
	public static void startLevel() {
		timeUpPaused = false;
		currentLevel.start();
		resetScore();

		AutoFade.LoadLevel(currentLevel.sceneName, 0.2f, 0.2f, Color.black, onStartLevelFinish);
	}
	public static void onStartLevelFinish() {
		enablePauseMenu(false, true);
		enableLevelComplete(false);
		AudioController.resumeVolume();

		enableLevelUI(true);
		levelUIController.enableMenuButton(true);

		startTimer(currentLevel.maxTime);
	}
	public static void restartLevel() {
		timeUpPaused = false;
		AutoFade.LoadLevel(currentLevel.sceneName, 0.2f, 0.2f, Color.black, onRestartLevelFinish);
	}
	public static void onRestartLevelFinish() {
		enablePauseMenu(false, true);
		enableLevelComplete(false);

		AudioController.resumeVolume();		
		resetScore();
		currentLevel.start();
		enableLevelUI(true);
		levelUIController.enableMenuButton(true);
		
		startTimer(currentLevel.maxTime);
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
		AutoFade.LoadLevel("WorldMap", 0.2f, 0.2f, Color.black, onExitLevelComplete);
	}
	private static void onExitLevelComplete() {
		hideNotes(true);
		stopTimer();

		timeUpPaused = false;
		enableLevelUI(false);
		enablePauseMenu(false, true);
		enableLevelComplete(false);

		AudioController.resumeVolume();
	}
	public static void finishLevel(CompletionType type) {
		hideNotes(true);

		// Stop music and timer.
		AudioController.resumeVolume();
		AudioController.playAudio("LevelComplete", false);
		stopTimer();

		// Set level completion message.
		switch (type) {
			case CompletionType.GameOver:
				levelCompleteController.gameOver();
				break;
			case CompletionType.LevelComplete:
				levelCompleteController.levelComplete();
				break;
			case CompletionType.TimeUp:
				levelCompleteController.timeUp();
				break;
		}

		levelUIController.enableMenuButton(false);
		enablePauseMenu(false, true);
		enableLevelComplete(true);

		currentLevel.finish();
	}
	// TODO remove
	public static void finishLevel(bool wasTimeUp=false) {
		hideNotes(true);

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
				timeUpPaused = true;

				finishLevel(CompletionType.TimeUp);
			}
			// 3 2 1 countdown.
			if (currTime <= 3) {
				AudioController.timerBeep();
			}
		}
	}

	/* ---------------------------------------------------- NOTES ----------------------------------------------------*/

	public static void showNotes(string note, bool pause=false) {
		notesController.setText(note);
		notesController.showNotes(pause);

		if (pause) {
			showNotesPaused = true;
			try {
			GeneralBoid.PauseBoids();
			} catch  {
				// TODO hack for demo
				GeneralPreyBoid.PauseBoids();
			}
		}
	}
	public void hideNotesPublic() {
		hideNotes(false);
	}
	public static void hideNotes(bool hurry) {
		notesController.hideNotes(hurry);
		
		if (showNotesPaused) {
			try {
				GeneralBoid.UnpauseBoids();
			} catch {
				// TODO Hack for demo
				GeneralPreyBoid.UnPauseBoids();
			}
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
			GeneralBoid.UnpauseBoids();
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
