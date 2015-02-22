using UnityEngine;
using System.Collections;

public class ManMenu : MonoBehaviour {

	double RANGE = 10.0f;
	double BUTTON_THRESHOLD = 200f;

	GameObject startButton;
	Vector3 startButtonPos;

	void Start () {
		startButton = GameObject.Find ("Start");
		startButtonPos = startButton.transform.position;

		iTween.MoveBy(GameObject.Find("Title"),
		              iTween.Hash("y", -RANGE, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
		iTween.MoveBy(startButton,
		              iTween.Hash("y", RANGE, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
		iTween.MoveBy(GameObject.Find("Preference"),
		              iTween.Hash("y", -RANGE, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
		iTween.MoveBy(GameObject.Find("Record"),
		              iTween.Hash("y", RANGE, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
		iTween.MoveBy(GameObject.Find("Exit"),
		              iTween.Hash("y", -RANGE, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePos = Input.mousePosition;

			if (Mathf.Abs (startButtonPos.x - mousePos.x) < BUTTON_THRESHOLD &&
			    Mathf.Abs (startButtonPos.y - mousePos.y) < BUTTON_THRESHOLD) {
				Application.LoadLevel ("WorldMap");
			}
		}
	}
}
