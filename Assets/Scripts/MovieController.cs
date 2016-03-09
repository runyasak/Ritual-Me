using UnityEngine;
using System.Collections;

public class MovieController : MonoBehaviour {
	private MovieTexture movie;
	public static MovieController instance;	
	// Use this for initialization
	void Awake(){
		instance = this;
	}
	void Start () {
		movie = GetComponent<Renderer> ().material.mainTexture as MovieTexture;
		this.GetComponent<Renderer> ().enabled = false;
	}
	
	public void playRitual(){
		this.GetComponent<Renderer> ().enabled = true;
		movie.Play ();
//		GameObject[] temp = GameObject.FindGameObjectsWithTag("Wizard");
//		foreach(GameObject i in temp){
//			if (WitchController.instance != i) {
//				i.GetComponent<WitchController> ().isFreeze = true;
//			}
//			Debug.Log ("isFreeze      "+      i.GetComponent<WitchController> ().isFreeze);
//		}
	}

	public void stopRitual(){
		Debug.Log ("Video stop");
		this.GetComponent<Renderer> ().enabled = false;
		movie.Stop ();
//		GameObject[] temp = GameObject.FindGameObjectsWithTag("Wizard");
//		foreach(GameObject i in temp) {
//			if (WitchController.instance != i) {
//				i.GetComponent<WitchController> ().isFreeze = false;
////				i.GetComponent<PlayerController> ().pentacle.active = false;
//			}
//		}
	}

	// Update is called once per frame
	void Update () {

	}
}
