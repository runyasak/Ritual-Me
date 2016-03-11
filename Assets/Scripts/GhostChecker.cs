using UnityEngine;
using System.Collections;

public class GhostChecker : MonoBehaviour {

	void OnTriggerEnter(Collider coll) {
		if (coll.name == "ghost_element(Clone)") {
			GameObject[] wizardArr = GameObject.FindGameObjectsWithTag ("Wizard");
			foreach(GameObject i in wizardArr){
				i.GetComponent<WitchController> ().curHR -= 5;
				i.GetComponent<WitchController> ().showDebuffEffect ();
			}
		}

	}
}
