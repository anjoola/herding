using UnityEngine;
using System.Collections;

public class SharkSceneController3 : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("TODO maybe too hard??", true);
		AudioController.playAudio("Underwater");
	}
}
