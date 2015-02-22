using UnityEngine;
using System.Collections;

public class CowSpawner : MonoBehaviour {

	public int numCows;
	public int timeDifferential;
	public GameObject cow;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Time.frameCount % timeDifferential == 0) {
			if (numCows > 0){
				Instantiate(cow);
				numCows--;
			}
		}
	}
}
