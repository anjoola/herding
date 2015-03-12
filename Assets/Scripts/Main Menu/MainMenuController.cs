using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	double RANGE = 8.0f;
	public GameObject startButton;
	public GameObject title;

	// TODO animate boy, and stuff

	void Start () {
		iTween.MoveBy(title,
		              iTween.Hash("x", 585, "easeType", "easeOutElastic", "loopType", "none", "delay", 1.0f, "time", 4));
		iTween.PunchPosition(title,
		                     iTween.Hash("x", 30, "loopType", "loop", "delay", 12, "time", 1));
		iTween.MoveBy(startButton,
		              iTween.Hash("y", RANGE, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
	}
}
