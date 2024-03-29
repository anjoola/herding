﻿using UnityEngine;
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

	public GameObject menuButton;
	
	/**
	 * Updates the score and timer.
	 */
	public void updateScore(int score) {
		this.score.text = "" + score;
	}
	public void updateTimer(int timeLeft) {
		timer.text = ("" + timeLeft).PadLeft(3, '0');
	}

	/**
	 * Show the pause menu.
	 */
	public void showMenu() {
		GlobalStateController.pauseLevel();
	}

	public void enableMenuButton(bool enabled) {
		menuButton.SetActive(enabled);
	}
}
