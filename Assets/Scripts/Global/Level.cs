using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Level {
	// Name used by all the assets associated with this levels.
	public string assetsName;
	// Scene name.
	public string sceneName;
	// Maximum time for a level.
	public static int MAX_TIME = 100;

	// Whether or not the level is completed.
	public bool isCompleted;
	// Number of points currently gotten.
	public int score;
	// Number of points needed for each star.
	public int maxScoreIncrement;
	// High score.
	public int highScore;
	// The number of stars earned.
	public int numStars;
	
	public Level(string assetsName, string sceneName, int maxScore) {
		this.assetsName = assetsName;
		this.sceneName = sceneName;
		this.maxScoreIncrement = maxScore / 5;

		this.isCompleted = false;
		this.score = 0;
		this.highScore = 0;
		this.numStars = 0;
	}

	public void incrementScore(int score) {
		this.score += score;
	}
	public int computeStars() {
		this.numStars = Mathf.Min(5, score / maxScoreIncrement);
		return this.numStars;
	}

	public void start() {
		score = 0;
	}
	public void finish() {
		isCompleted = true;

		// New high score.
		if (score > highScore) {
			highScore = score;
		}
		computeStars();
	}
}
