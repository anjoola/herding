using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Level {
	// Name used by all the assets associated with this levels.
	public string assetsName;
	// Scene name.
	public string sceneName;
	// Maximum time for this level.
	public int maxTime;

	// Whether or not the level is completed.
	public bool isCompleted;
	// Number of points currently gotten.
	public int score;
	// High score.
	public int highScore;
	// The number of stars earned.
	public int numStars;
	
	public Level(string assetsName, string sceneName, int maxTime) {
		this.assetsName = assetsName;
		this.sceneName = sceneName;
		this.maxTime = maxTime;

		this.isCompleted = false;
		this.score = 0;
		this.highScore = 0;
		this.numStars = 5;
	}

	public void incrementScore(int score) {
		this.score += score;
	}
	public void computeStars() {
		// TODO
		this.numStars = 3;
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
