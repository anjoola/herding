using UnityEngine;
using System.Collections;

public class CowSceneController : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("It was a peaceful day in the Cow Palace... until all the cows got loose! " +
		                                "Tap and drag them back to the barn!", true);
		AudioController.playAudio("CowFlight");
	}
}
