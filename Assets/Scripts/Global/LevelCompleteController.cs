using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelCompleteController : MonoBehaviour {
	string TIME_UP = "Time's Up!";
	string GAME_OVER = "Game Over!";
	string LEVEL_COMPLETE = "Level Complete!";

	public Text levelCompleteText;

	public void levelComplete() {
		levelCompleteText.text = LEVEL_COMPLETE;
	}
	public void timeUp() {
		levelCompleteText.text = TIME_UP;
	}
	public void gameOver() {
		levelCompleteText.text = GAME_OVER;
	}

	public void restart() {
		GlobalStateController.restartLevel();
	}
	public void exitLevel() {
		GlobalStateController.exitLevel();
	}
}
