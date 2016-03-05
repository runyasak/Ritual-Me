using UnityEngine;
using System.Collections;

public class EdgeTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider coll) {
//		Debug.Log (coll);
		Destroy (coll.gameObject);
	}



}
