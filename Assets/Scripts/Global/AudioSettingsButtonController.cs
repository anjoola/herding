using UnityEngine;
using System.Collections;

public class AudioSettingsButtonController : MonoBehaviour {
	public TypogenicText text;
	private string VOLUME_ON = "";
	private string VOLUME_OFF = "";

	private Color darkColor;
	private Color medColor;
	private Color lightColor;

	void Start() {
		lightColor = new Color(0.6f, 0.6f, 0.6f);
		darkColor = new Color(0.3f, 0.3f, 0.3f);

		text.ColorTopLeft = Color.white;
		text.ColorBottomLeft = lightColor;
	}
	public void SetInitialState() {
		if (AudioController.isVolumeOn) {
			text.Set(VOLUME_ON);
		} else {
			text.Set(VOLUME_OFF);
		}
	}
	void OnMouseDown() {
		text.ColorTopLeft = Color.white;
		text.ColorBottomLeft = darkColor;
	}
	void OnMouseUp() {
		text.ColorTopLeft = Color.white;
		text.ColorBottomLeft = lightColor;

		toggleVolume();
	}
	void toggleVolume() {
		if (AudioController.isVolumeOn) {
			AudioController.turnVolumeOff();
			text.Set(VOLUME_OFF);
		} else {
			AudioController.turnVolumeOn();
			text.Set(VOLUME_ON);
		}
	}
}
