using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	public static bool isVolumeOn;

	void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
	void Start() {
		isVolumeOn = true; // TODO detect ios volume setting
	}

	public static void turnVolumeOn() {
		isVolumeOn = true;
		// TODO ios-specific tihngs
	}
	public static void turnVolumeOff() {
		isVolumeOn = false;
		// TODO ios-specific things
	}
}
