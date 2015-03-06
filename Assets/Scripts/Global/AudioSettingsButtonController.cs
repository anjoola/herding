using UnityEngine;
using System.Collections;

public class AudioSettingsButtonController : MonoBehaviour {
	public TypogenicText text;
	private string VOLUME_ON = "";
	private string VOLUME_OFF = "";

	private Color darkColor;
	private Color lightColor;

	void Start() {
		lightColor = new Color(0.6f, 0.6f, 0.6f);
		darkColor = new Color(0.5f, 0.5f, 0.5f);

		text.ColorTopLeft = Color.white;
		text.ColorBottomLeft = lightColor;
	
		if (AudioController.isVolumeOn) {
			text.Set(VOLUME_ON);
		} else {
			text.Set(VOLUME_OFF);
		}
	}
	void OnMouseDown() {
		text.ColorTopLeft = Color.white;
		text.ColorBottomLeft = darkColor;

		// TODO change button style
	}
	void OnMouseUp() {
		text.ColorTopLeft = Color.white;
		text.ColorBottomLeft = lightColor;

		// TODO change button style

		toggleVolume(); // TODO
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
