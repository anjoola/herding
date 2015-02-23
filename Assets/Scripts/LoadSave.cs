using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadSave {
	static string SAVE_FILE = "/savefile.gd";
	public static GameState currentGame;

	/**
	 * Saves the current game.
	 */
	public static void saveGame() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + SAVE_FILE);
		bf.Serialize(file, LoadSave.currentGame);
		file.Close(); 
	}
	/**
	 * Loads an existing game or creates a new file if one doesn't exist.
	 */
	public static void loadGame() {
		if (File.Exists(Application.persistentDataPath + SAVE_FILE)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + SAVE_FILE, FileMode.Open);
			LoadSave.currentGame = (GameState)bf.Deserialize(file);
			file.Close();
		}
		// Otherwise, create a new game save.
		else {
			LoadSave.currentGame = new GameState();
		}
	}
}
