using UnityEngine;
using System.Collections;

public class SharkSceneController : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("Deep in the dark depths of the ocean, two sharks hunger for a meal to eat. " +
			"Move the sharks around to help them trap the fish in the light!", true);
		AudioController.playAudio("Underwater");
	}
}
