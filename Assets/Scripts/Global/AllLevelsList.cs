using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllLevelsList : MonoBehaviour {
	public static int POINTS_PER_OBJECT = 40;

	/**
	 * Gets a list of all the levels in the game.
	 */
	public static List<Level> getAllLevels() {
		List<Level> levels = new List<Level>();

		// (prefix of all related assets, scene name, 5-star score, time limit).
		levels.Add(new Level("Cow Palace", "CowFlight", (20 - 2) * POINTS_PER_OBJECT, 100));
		levels.Add(new Level("Classroom Chaos", "Classroom", 100, 100));
		levels.Add(new Level("Underwater Meal", "SharkWater", 100, 30));
		levels.Add(new Level("Aerial Dangers", "BirdAndPropeller", 100, 100));
		// TODO add more

		return levels;
	}
}
