﻿using UnityEngine;
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
	public int numPoints;
	// The number of stars earned.
	public int numStars;
	
	public Level(string assetsName, string sceneName) {
		this.assetsName = assetsName;
		this.sceneName = sceneName;

		this.isCompleted = false;
		this.numPoints = 0;
		this.numStars = 3;
	}
	public void setCompleted(int numPoints) {
		isCompleted = true;
		this.numPoints = numPoints;

		// Compute stars.
		// TODO
		numStars = 5;
	}

}
