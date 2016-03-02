using UnityEngine;
using System.Collections;

public class RunnerController : MonoBehaviour {
	
	private Vector3 dir;
	private float mult;
	public static RunnerController instance;

	public GameObject LeftGauge;
	public GameObject RightGauge;
	// Use this for initialization
	void Start () {
		mult = 5f;
		dir = new Vector3(1, 0, 0);
	}

	void move() {
		this.transform.position += dir * Time.deltaTime * 5f;
	}
	
	// Update is called once per frame
	void Update () {
		move ();
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit; Ray ray;
			ray = new Ray (transform.position, new Vector3(0f,0f,1.0f));
			if (Physics.Raycast (ray, out hit, 100)) {
				if (hit.transform.tag == "Element"){
//					foreach(RitualElement i in GaugeController.positionArr){
//						Debug.Log ("FOR");
//						if (i.getElementID() == (hit.transform.gameObject.GetInstanceID())) {
//							Debug.Log ("IF");
//							GaugeController.positionArr.Remove (i);
//							Debug.Log(GaugeController.positionArr.Count);
							Destroy (hit.transform.gameObject);
//							Debug.Log ("BREAK");
//							break;
//						}
//					}
//					Debug.Log("ggggggggggggggggg"+GaugeController.positionArr[0].getElement().GetInstanceID());
//					Debug.Log("hhhhhhhhhhhhhhh"+hit.transform.gameObject.GetInstanceID());
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.tag == "Domain") {
			dir = -dir;
		}
	}


}
