using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Level {
	public static List<Level> getInitLevels() {
		List<Level> levels = new List<Level>();
		levels.Add(new Level("Cow Palace", "CowPalace"));
		levels.Add(new Level("City", "CowPalace"));
		// TODO add more
		return levels;
	}

	// Name used by all the assets associated with this levels.
	public string assetsName;
	// Scene name.
	public string sceneName;

	// Whether or not the level is completed.
	public bool isCompleted;
	// Number of points gotten.
	public int score;
	// The number of stars earned.
	public int numStars;
	
	public Level(string assetsName, string sceneName) {
		this.assetsName = assetsName;
		this.sceneName = sceneName;

		this.isCompleted = false;
		this.score = 0;
		this.numStars = 3;
	}
	public string getScore() {
		return "Score: " + score;
	}
	public void incrementScore(int score) {
		this.score += score;
	}
	public void setCompleted(int score) {
		isCompleted = true;
		this.score = score;

		// Compute stars.
		// TODO
		numStars = 5;
	}

}
