using UnityEngine;
using System.Collections;

public class SharkSceneController3 : MonoBehaviour {
	void Start () {
		GlobalStateController.showNotes("The fish are getting smarter now... try to trap them between the sharks!", true);
		AudioController.playAudio("Underwater");
	}
}
