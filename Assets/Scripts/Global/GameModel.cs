using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameModel {
	public List<Level> levels;

	// Whether or not the user played the game yet. Set to false if this it the first time the user
	// has played the game.
	public bool played;

	// The user's last audio settings.
	public bool wasAudioOn;

	public GameModel() {
		levels = AllLevelsList.getAllLevels();
		played = false;
		wasAudioOn = true;
	}
}
