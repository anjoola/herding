using UnityEngine;
using System.Collections;

public class ManMenu : MonoBehaviour {

	double range = 10.0f;

	// Use this for initialization
	void Start () {
		iTween.MoveBy(GameObject.Find("Title"), iTween.Hash("y", -range, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
		iTween.MoveBy(GameObject.Find("Start"), iTween.Hash("y", range, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
		iTween.MoveBy(GameObject.Find("Preference"), iTween.Hash("y", -range, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
		iTween.MoveBy(GameObject.Find("Record"), iTween.Hash("y", range, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
		iTween.MoveBy(GameObject.Find("Exit"), iTween.Hash("y", -range, "easeType", "linear", "loopType", "pingPong", "delay", 0.0, "time", 1));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
