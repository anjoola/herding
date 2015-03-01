using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Script controlling the level UI display.
 *    - Current score
 *    - Current time left
 *    - Showing the pause menu
 */
public class LevelUIController : MonoBehaviour {
	public Text timer;
	public Text score;
	
	/**
	 * Updates the score and timer.
	 */
	public void updateScore(int score) {
		this.score.text = "Score: " + score;
	}
	public void updateTimer(int timeLeft) {
		timer.text = "Time: " + timeLeft;
	}

	/**
	 * Show the pause menu.
	 */
	public void showMenu() {
		GlobalStateController.pauseLevel();
	}
}
