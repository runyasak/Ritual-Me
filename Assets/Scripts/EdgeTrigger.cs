using UnityEngine;
using System.Collections;

public class EdgeTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider coll) {
		Destroy (coll.gameObject);
		GameObject go = GameObject.Find("SocketIO");
	}

}
