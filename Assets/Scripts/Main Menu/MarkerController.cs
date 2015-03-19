using UnityEngine;
using System.Collections;

public class MarkerController : MonoBehaviour {
	public GameObject marker1, marker2, marker3, marker4;
	private float SCALE = 1.1f;

	void Start () {
		iTween.ScaleBy(marker1,
		               iTween.Hash("x", SCALE, "y", SCALE, "z", SCALE, "easeType", "linear",
		            			  "loopType", "pingPong", "delay", 0, "time", 0.5f));
		iTween.ScaleBy(marker2,
		               iTween.Hash("x", SCALE, "y", SCALE, "z", SCALE, "easeType", "linear",
		           				   "loopType", "pingPong", "delay", 0, "time", 0.5f));
		iTween.ScaleBy(marker3,
		               iTween.Hash("x", SCALE, "y", SCALE, "z", SCALE, "easeType", "linear",
		            			   "loopType", "pingPong", "delay", 0, "time", 0.5f));
		iTween.ScaleBy(marker4,
		               iTween.Hash("x", SCALE, "y", SCALE, "z", SCALE, "easeType", "linear",
		            			   "loopType", "pingPong", "delay", 0, "time", 0.5f));
	}
}
