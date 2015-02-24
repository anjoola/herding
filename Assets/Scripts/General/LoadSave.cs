using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadSave {
	static string SAVE_FILE = "/savefile.gd";

	/**
	 * Saves the current game.
	 */
	public static void saveGame() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + SAVE_FILE);
		bf.Serialize(file, GlobalState.currentGame);
		file.Close(); 
	}
	/**
	 * Loads an existing game or creates a new file if one doesn't exist.
	 */
	public static void loadGame() {
		try {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + SAVE_FILE, FileMode.Open);
			GlobalState.currentGame = (GameState)bf.Deserialize(file);
			file.Close();
		} catch (System.SystemException) {
			// Otherwise, create a new game save.
			GlobalState.currentGame = new GameState();
		}
	}
}
