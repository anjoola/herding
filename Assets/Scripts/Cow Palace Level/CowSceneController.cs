using UnityEngine;
using System.Collections;

public class CowSceneController : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("Foolish ol' Charlie forgot to check on the cows, and now they are on the run! " +
		                                "Tap and drag them back to the barn before Hank finds out!", true);
		AudioController.playAudio("CowFlight");
	}
}
