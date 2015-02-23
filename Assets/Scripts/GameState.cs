using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameState {
	public List<Level> levels;

	public GameState() {
		levels = Level.getInitLevels();
	}
}
