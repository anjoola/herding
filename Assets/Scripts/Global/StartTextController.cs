using UnityEngine;
using System.Collections;

public class StartTextController : MonoBehaviour {
	void OnMouseUp() {
		AutoFade.LoadLevel("WorldMap", 0.2f, 0.2f, Color.black);
	}
}
