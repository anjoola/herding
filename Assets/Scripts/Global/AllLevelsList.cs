using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllLevelsList : MonoBehaviour {
	/**
	 * Gets a list of all the levels in the game.
	 */
	public static List<Level> getAllLevels() {
		List<Level> levels = new List<Level>();

		// Cow palace.
		// (prefix of all related assets, scene name, 5-star score, time limit).
		levels.Add(new Level("The Little Red Barn", "CowFlight", 10 * CowBoid.POINT_PER_BOID, 10));
		levels.Add(new Level("Barn-by-the-Sea", "CowSea", 10 * CowBoid.POINT_PER_BOID, 10));
		levels.Add(new Level("Cow Palace", "CowPalace", (12 - 1) * CowBoid.POINT_PER_BOID, 20));
		levels.Add(new Level("Cows Overrun", "CowOverrun", (10 - 2) * CowBoid.POINT_PER_BOID, 20));

		// Underwater.
		levels.Add(new Level("Crab Meal", "SharkWater0", 100, 20));
		levels.Add(new Level("Rock Cave", "SharkWater1", 100, 20));
		levels.Add(new Level("Teamwork", "SharkWater2", 100, 30));
		levels.Add(new Level("Freedom", "SharkWater3", 100, 30));

		// Other
		//levels.Add(new Level("Classroom Chaos", "Classroom", 100, 100));
		//levels.Add(new Level("Aerial Dangers", "BirdAndPropeller", 100, 100));

		return levels;
	}
}
