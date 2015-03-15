using UnityEngine;
using System.Collections;

public class CowSeaController : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("The peaceful waves of the ocean roll by - except the cows are loose again!" +
										"This time the faster bulls are running around too!", true);
		AudioController.playAudio("CowFlight");
	}
}
