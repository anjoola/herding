using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
	public static AudioController instance;

	public static bool isVolumeOn;
	public static int numTimesToggled;

	private static GameObject button;
	private static AudioSettingsButtonController buttonController;
	private static int MAX_TIMES_TOGGLED = 3;

	private static AudioSource currAudio;
	private static float FADE_DURATION = 1.0f;

	void Awake() {
		DontDestroyOnLoad(this.gameObject);
		instance = this;
	}
	void Start() {
		button = GameObject.Find("Audio Settings Button");
		buttonController = button.GetComponent("AudioSettingsButtonController") as AudioSettingsButtonController;

		isVolumeOn = GlobalStateController.currentGame.wasAudioOn;
		buttonController.SetInitialState();

		// Play main menu music.
		playAudio("MainMenuMusic");
	}

	public static AudioSource getSource(string audioName) {
		GameObject obj = Instantiate(Resources.Load(audioName)) as GameObject;
		obj.transform.position = GlobalStateController.instance.transform.position;
		return obj.audio;
	}
	public static void buttonPress() {
		AudioController.playSFX("ButtonPress", 1.0f);
		// TODO no sound
	}
	public static void playSFX(string audioName, float volume=1.0f) {
		AudioSource sfx = getSource(audioName);

		sfx.volume = volume;
		sfx.Play();
	}
	public static void playAudio(string audioName, bool fadeIn=true) {
		AudioSource newAudio = getSource(audioName);
	
		// Play the audio.
		newAudio.volume = 0;
		newAudio.Play();

		// Fade out old audio and fade in new one.
		Crossfade(currAudio, newAudio, fadeIn);
		currAudio = newAudio;
	}

	public static void turnVolumeOn() {
		checkToggle();

		isVolumeOn = true;
		GlobalStateController.currentGame.wasAudioOn = true;
		AudioListener.volume = 1.0f;
	}
	public static void turnVolumeOff() {
		checkToggle();

		isVolumeOn = false;
		GlobalStateController.currentGame.wasAudioOn = false;
		AudioListener.volume = 0.0f;
	}
	public static void halfVolume() {
		AudioListener.volume = 0.3f;
	}
	public static void resumeVolume() {
		AudioListener.volume = 1.0f;
	}
	// Display a message if the user tries to toggle too many times.
	private static void checkToggle() {
		numTimesToggled++;
		if (numTimesToggled > 1 && numTimesToggled % MAX_TIMES_TOGGLED == 0) {
			GlobalStateController.showNotes("Check your device's volume setting! It may not be on.");
		}
	}

	public static void Crossfade(AudioSource a1, AudioSource a2, bool fadeIn) {
		instance.StartCoroutine(instance.CrossfadeEnum(a1, a2, fadeIn));
	}
	IEnumerator CrossfadeEnum(AudioSource a1, AudioSource a2, bool fadeIn) {
		float startTime = Time.time;
		float endTime = startTime + FADE_DURATION;
		if (!fadeIn && a2 != null) a2.volume = 1.0f;
		while (Time.time < endTime) {
			float i = (Time.time - startTime) / FADE_DURATION;
			if (a1 != null) a1.volume = (1 - i);
			if (fadeIn && a2 != null) a2.volume = i;
			yield return new WaitForSeconds(0.1f);
		}
	}
}
