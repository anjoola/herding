using UnityEngine;
using System.Collections;

public class SharkSceneController : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("Deep in the dark depths of the ocean, Crab King hungers for a delicious " +
			"meal. Move the shark minion around to direct fish towards the King!", true);
		AudioController.playAudio("Underwater");
	}
}
