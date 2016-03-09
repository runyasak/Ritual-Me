using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController instance;

	void Awake () {
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}

	public void clearElement () {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Element");
		foreach(GameObject i in temp){
			Destroy (i);	
		}
	}
		

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) { clearElement (); }
	}
}
