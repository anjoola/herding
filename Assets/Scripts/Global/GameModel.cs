using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameModel {
	public List<Level> levels;

	public GameModel() {
		levels = AllLevelsList.getAllLevels();
	}
}
