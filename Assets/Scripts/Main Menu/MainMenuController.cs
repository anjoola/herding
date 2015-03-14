using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	double RANGE = 8.0f;
	public GameObject startButton;
	public GameObject title;

	public GameObject obj1;
	public GameObject obj2;
	public GameObject obj3;

	void Start () {
		iTween.MoveBy(title,
		              iTween.Hash("x", 585, "easeType", "easeOutElastic", "loopType", "none", "delay", 1.0f, "time", 4));
		iTween.PunchPosition(title,
		                     iTween.Hash("x", 30, "loopType", "loop", "delay", 12, "time", 1));
		iTween.MoveBy(startButton,
		              iTween.Hash("y", RANGE, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));

		iTween.MoveBy(obj1,
		              iTween.Hash("x", 35, "easeType", "linear", "loopType", "none", "delay", 0.7f, "time", 0.4f));
		iTween.MoveBy(obj2,
		              iTween.Hash("x", 35, "easeType", "linear", "loopType", "none", "delay", 0.5f, "time", 0.3f));
		iTween.MoveBy(obj3,
		              iTween.Hash("x", 35, "easeType", "linear", "loopType", "none", "delay", 0.9f, "time", 0.5f));
	}
}
