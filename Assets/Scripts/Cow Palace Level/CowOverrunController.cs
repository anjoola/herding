using UnityEngine;
using System.Collections;

public class CowOverrunController : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("Ack, they even broke all the fences! Save as many cows as you can before" +
										"they are gone forever!", true);
		AudioController.playAudio("CowFlight");
	}
}
