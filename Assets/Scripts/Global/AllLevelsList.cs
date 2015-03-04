using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllLevelsList : MonoBehaviour {

	/**
	 * Gets a list of all the levels in the game.
	 */
	public static List<Level> getAllLevels() {
		List<Level> levels = new List<Level>();

		// (prefix of all related assets, e.g. "Cow Palace Model", "Cow Palace Zoom", scene name, max time).
		levels.Add(new Level("Cow Palace", "CowFlight", 100));
		levels.Add(new Level("City", "CowFlight", 100));
		// TODO add more

		return levels;
	}
}
