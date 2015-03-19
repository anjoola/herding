using UnityEngine;
using System.Collections;

public class SharkSceneController1 : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("Herd fish into the seaweed cave for the sharks to feed on later!", true);
		AudioController.playAudio("Underwater");
	}
}
