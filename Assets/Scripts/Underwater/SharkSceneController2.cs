using UnityEngine;
using System.Collections;

public class SharkSceneController2 : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("Sharks work together to herd the fish in the circular feeding ground.", true);
		AudioController.playAudio("Underwater");
	}
}
