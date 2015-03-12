using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotesController : MonoBehaviour {
	public Text field;
	public GameObject notes;
	public GameObject mainNotes;
	public GameObject pointer;

	public bool dismissAnywhere;

	float SCALE = 0.001f;
	float TIME = 0.2f;

	void Awake() {
		iTween.MoveBy(pointer, iTween.Hash("amount", new Vector3(0, 0.1f, 0), "easeType", "linear",
		                                   "loopType", "pingPong", "delay", 0.0, "time", 0.5f));
	}
	void Start() {
		dismissAnywhere = false;
	}

	public void setText(string text) {
		field.text = text;
	}
	public void showNotes(bool targetDismiss=false) {
		this.dismissAnywhere = !targetDismiss;
		mainNotes.SetActive(true);
		iTween.ScaleBy(notes, iTween.Hash("y", 1/SCALE, "easeType", "linear", "loopType", "none", "delay", 0.0,
		                                  "time", TIME, "oncomplete", "activate", "oncompletetarget", mainNotes));
	}
	public void activate() {
		pointer.SetActive(true);
	}
	public void hideNotes(bool hurry=false) {
		pointer.SetActive(false);
		float time = hurry ? 0 : TIME;
		iTween.ScaleBy(notes, iTween.Hash("y", SCALE, "easeType", "linear", "loopType", "none", "delay", 0.0,
		                                  "time", time, "oncomplete", "deactivate", "oncompletetarget", mainNotes));
	}
	public void deactivate() {
		mainNotes.SetActive(false);
	}
}
