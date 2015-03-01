using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuController : MonoBehaviour {
	public Text levelName;
	public Text score;
	
	public void updateText(string levelName, int score) {
		this.levelName.text = levelName;
		this.score.text = "Score: " + score;
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
