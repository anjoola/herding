using UnityEngine;
using System.Collections;

public class GlobalState : MonoBehaviour {
	public static GameState currentGame;
	public static Level currentLevel;

	public static GameObject pauseMenu;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
		DontDestroyOnLoad(pauseMenu);
	}
	void Start() {
		// Load savefile.
		LoadSave.loadGame();

		pauseMenu = GameObject.Find("PauseMenu");
		GlobalState.enablePauseMenu(false);
	}
	void OnApplicationQuit() {
		// Save savefile.
		LoadSave.saveGame();
	}

	public static void enablePauseMenu(bool enabled) {
		if (GlobalState.currentLevel == null && enabled) return;

		GlobalState.pauseMenu.SetActive(enabled);
	}
}
