using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {
	public GameObject pauseMenu;
	public Text levelName;
	public Text score;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//TODO score.text = GlobalState.currentLevel.getScore();
	}

	public void updateText() {
		levelName.text = GlobalState.currentLevel.assetsName;
		score.text = GlobalState.currentLevel.getScore();
	}

	public void resume() {
		GlobalState.enablePauseMenu(false);
	}
	public void restart() {
		//SendMessage("restartLevel"); TODO
		GlobalState.enablePauseMenu(false);
	}
	public void exitLevel() {
		// TODO cleanup for this level?
		Application.LoadLevel("WorldMap");
	}
}
