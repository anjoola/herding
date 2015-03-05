using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotesController : MonoBehaviour {
	public Text field;

	public void setText(string text) {
		field.text = text;
	}
}
