using UnityEngine;
using System.Collections;

public class SharkSceneController3 : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("TODO", true);
		AudioController.playAudio("Underwater");
	}
}
