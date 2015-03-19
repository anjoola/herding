using UnityEngine;
using System.Collections;

public class SharkSceneController : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("Deep in the dark depths of the ocean, a shark hungers for a meal to eat. " +
			"Move the shark around to trap the fish in the rocks!", true);
		AudioController.playAudio("Underwater");
	}
}
