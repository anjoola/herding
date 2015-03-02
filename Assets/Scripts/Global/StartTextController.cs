using UnityEngine;
using System.Collections;

public class StartTextController : MonoBehaviour {
	void OnMouseUp() {
		// TODO smooth transition
		Application.LoadLevel("WorldMap");
	}
}
