using UnityEngine;
using System.Collections;

public class CowFlightMusic : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("It was a peaceful day in the Cow Palace... until all the cows got loose! " +
		                                "Tap and drag them back to the barn!", true);
		AudioController.playAudio("CowFlight");
	}
}
