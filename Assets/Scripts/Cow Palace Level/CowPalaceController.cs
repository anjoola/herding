using UnityEngine;
using System.Collections;

public class CowPalaceController : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("Ay carumba! Even the prized red bulls from the Cow Palace are loose. " +
										"What's going on? A mutiny?", true);
		AudioController.playAudio("CowFlight");
	}
}
