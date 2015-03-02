using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	double RANGE = 1.0f;
	GameObject startButton;

	void Start () {
		startButton = GameObject.Find ("Start Button");

		iTween.MoveBy(GameObject.Find("Title"),
		              iTween.Hash("y", -RANGE, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
		iTween.MoveBy(startButton,
		              iTween.Hash("y", RANGE, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Application.LoadLevel("WorldMap");
		}
	}
}
