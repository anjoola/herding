using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuController : MonoBehaviour {
	float DISPLAY_TIME = 0.3f;
	float SCALE = 20;

	public Text levelName;
	public Text score;

	public GameObject overlay;
	public GameObject pauseMenu;
	public GameObject upperPanel;
	public GameObject lowerPanel;
	public GameObject buttons;

	public void updateText(string levelName, int score) {
		this.levelName.text = levelName;
		this.score.text = "Score: " + score;
	}
	
	public void slideIn() {
		activate();
		iTween.MoveBy(upperPanel, iTween.Hash("y", -15, "easeType", "linear", "loopType", "none", "delay", 0.0,
		                                      "time", DISPLAY_TIME));
		iTween.MoveBy(lowerPanel, iTween.Hash("y", 10, "easeType", "linear", "loopType", "none", "delay", 0.0,
		                                      "time", DISPLAY_TIME));
		iTween.ScaleBy(buttons, iTween.Hash("x", SCALE, "y", SCALE, "z", SCALE, "easeType", "linear", "loopType", "none",
		                                    "delay", 0.0, "time", DISPLAY_TIME));
		iTween.FadeTo(overlay, iTween.Hash("alpha", 0, "includechildren", true, "time", DISPLAY_TIME));
	}
	public void slideOut() {
		iTween.MoveBy(upperPanel, iTween.Hash("y", 15, "easeType", "linear", "loopType", "none", "delay", 0.0,
		                                      "time", DISPLAY_TIME));
		iTween.MoveBy(lowerPanel, iTween.Hash("y", -10, "easeType", "linear", "loopType", "none", "delay", 0.0,
		                                      "time", DISPLAY_TIME));
		iTween.ScaleBy(buttons, iTween.Hash("x", 1/SCALE, "y", 1/SCALE, "z", 1/SCALE, "easeType", "linear",
		                                    "loopType", "none", "delay", 0.0, "time", DISPLAY_TIME,
		                                    "oncomplete", "deactivate", "oncompletetarget", pauseMenu));
	}
	public void activate() {
		pauseMenu.SetActive(true);
	}
	public void deactivate() {
		pauseMenu.SetActive(false);
	}

	public void resume() {
		GlobalStateController.resumeLevel();
	}
	public void restart() {
		GlobalStateController.restartLevel();
	}
	public void exitLevel() {
		GlobalStateController.exitLevel();
	}
}
