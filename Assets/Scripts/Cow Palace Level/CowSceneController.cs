using UnityEngine;
using System.Collections;

public class CowSceneController : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("It was a peaceful day at the Little Red Barn... until the cows got loose! " +
		                                "Tap and drag them back to the barn before the time runs out!", true);
		AudioController.playAudio("CowFlight");
	}
}
