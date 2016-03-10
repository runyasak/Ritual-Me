using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void changeToStartScene () {
		Application.LoadLevel (1);
	}

	// Update is called once per frame
	void Update () {
		/*if (Input.GetMouseButtonDown(0)){
			Application.LoadLevel (1);
		}*/
	}
}
