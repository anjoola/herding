using UnityEngine;
using System.Collections;

public class SharkSceneController1 : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("Place fish into the rock cave for the sharks to feed on later.", true);
		AudioController.playAudio("Underwater");
	}
}
