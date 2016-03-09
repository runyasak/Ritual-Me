using UnityEngine;
using System.Collections;

public class StartGameMenu : MonoBehaviour {
	
	public static StartGameMenu instance;
	public static int checker;
	public SpriteRenderer grayBG;

	// Use this for initialization
	void awake(){
		DontDestroyOnLoad (instance);
	}

	void Start () {
		instance = this;
		Time.timeScale = 0;
		grayBG.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (checker == 1) {
			startGame ();
		}

	}

	public void startGame(){
		grayBG.gameObject.SetActive (false);
		Time.timeScale = 1;
	}
}
