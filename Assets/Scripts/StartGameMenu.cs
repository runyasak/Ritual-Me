using UnityEngine;
using System.Collections;

public class StartGameMenu : MonoBehaviour {

	public SpriteRenderer grayBG;

	// Use this for initialization
	void Start () {
		Time.timeScale = 0;
		grayBG.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void startGame(){
		grayBG.gameObject.SetActive (false);
		Time.timeScale = 1;
	}
}
