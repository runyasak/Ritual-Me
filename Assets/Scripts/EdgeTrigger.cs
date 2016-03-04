using UnityEngine;
using System.Collections;

public class EdgeTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ElementManager.instance.spawnElement ();
	}

	void OnTriggerEnter2D(Collider2D coll) {
//		Debug.Log (coll);
		Destroy (coll.gameObject);
		//ElementManager.instance.spawnElement ();
	}



}
