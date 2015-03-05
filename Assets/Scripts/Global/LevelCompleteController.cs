using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelCompleteController : MonoBehaviour {
	string TIME_UP = "Time's Up!";
	string GAME_OVER = "Game Over!";
	string LEVEL_COMPLETE = "Level Complete!";
	float STAR_SCALE = 1.2f;
	float SCALE = 20.0f;
	float DISPLAY_TIME = 0.3f;

	public GameObject topPanel;
	public GameObject buttons;

	public Text levelCompleteText;
	public GameObject overlay;
	public GameObject[] stars;
	public GameObject levelCompleteObj;

	public void slideIn() {
		activate();
		iTween.MoveBy(topPanel, iTween.Hash("y", -10, "easeType", "linear", "loopType", "none", "delay", 0.0,
		                                      "time", DISPLAY_TIME));
		iTween.ScaleBy(buttons, iTween.Hash("x", SCALE, "y", SCALE, "z", SCALE, "easeType", "linear", "loopType", "none",
		                                    "delay", 0.0, "time", DISPLAY_TIME));
	}
	public void slideOut() {
		iTween.MoveBy(topPanel, iTween.Hash("y", 10, "easeType", "linear", "loopType", "none", "delay", 0.0,
		                                    "time", 0));
		iTween.ScaleBy(buttons, iTween.Hash("x", 1/SCALE, "y", 1/SCALE, "z", 1/SCALE, "easeType", "linear",
		                                    "loopType", "none", "delay", 0.0, "time", 0,
		                                    "oncomplete", "deactivate", "oncompletetarget", levelCompleteObj));
	}
	public void activate() {
		levelCompleteObj.SetActive(true);
	}
	public void deactivate() {
		levelCompleteObj.SetActive(false);
	}

	public void levelComplete() {
		levelCompleteText.text = LEVEL_COMPLETE;
		computeStars();
	}
	public void timeUp() {
		levelCompleteText.text = TIME_UP;
		computeStars();
	}
	public void gameOver() {
		levelCompleteText.text = GAME_OVER;
		computeStars();
	}
	public void computeStars() {
		int numStars = GlobalStateController.currentLevel.computeStars();
		for (int j = numStars; j < 5; j++) {
			stars[j].SetActive(false);
		}
		for (int i = 0; i < numStars; i++) {
			stars[i].SetActive(true);
			iTween.ScaleBy(stars[i], iTween.Hash("x", STAR_SCALE, "y", STAR_SCALE, "z", STAR_SCALE,
			                                     "easeType", "linear", "loopType", "pingPong",
			                                     "delay", 0.1f, "time", 0.6f));
		}
	}

	public void restart() {
		GlobalStateController.restartLevel();
	}
	public void exitLevel() {
		GlobalStateController.exitLevel();
	}
}
