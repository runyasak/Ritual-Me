using UnityEngine;
using System.Collections;

public class WitchController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		Debug.Log ("HIT");
		Destroy (coll.gameObject);
//		ElementManager.instance.spawnElement ();
	}
}
