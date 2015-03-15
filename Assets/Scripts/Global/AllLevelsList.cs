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
		levels.Add(new Level("The Little Red Barn", "CowFlight", (10 - 2) * CowBoid.POINT_PER_BOID, 10));
		levels.Add(new Level("Cow Palace", "CowFlight", (10 - 2) * CowBoid.POINT_PER_BOID, 10));
		levels.Add(new Level("Hank's Mini-Barn", "CowFlight", (10 - 2) * CowBoid.POINT_PER_BOID, 10));
		levels.Add(new Level("Cows Overrun", "CowFlight", (10 - 2) * CowBoid.POINT_PER_BOID, 10));

		// Underwater.
		levels.Add(new Level("Underwater Meal", "SharkWater", 100, 30));

		// Other
		//levels.Add(new Level("Classroom Chaos", "Classroom", 100, 100));
		//levels.Add(new Level("Aerial Dangers", "BirdAndPropeller", 100, 100));

		return levels;
	}
}
