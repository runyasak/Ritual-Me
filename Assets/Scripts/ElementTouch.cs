using UnityEngine;
using System.Collections;

public class ElementTouch : MonoBehaviour {

//	public GameObject ghost;
	public Color defualtColor;
	public Color selectedColor;
	private Renderer rend;
	private bool isHit;
	private Touch touch;
	private Vector3 v;
	// Use this for initialization
	void Start () {
		isHit = false;
		rend = this.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isHit) {
			transform.position = new Vector3(Camera.main.ScreenToWorldPoint (touch.position).x,
				Camera.main.ScreenToWorldPoint (touch.position).y,transform.position.z);
		}
	}

	void OntouchDown (Touch t){
//		Debug.Log (hitPoint);
		touch = t;
		isHit = true;
		rend.material.color = selectedColor;
		Debug.Log ("touch down");
	}
	void OntouchUp (Touch t){
		rend.material.color = defualtColor;
		isHit = false;
//		touch = null;
		Debug.Log ("touch up");
	}
	void OntouchStay (Vector3 hitPoint){
		v = hitPoint;
//		rend.material.color = selectedColor;

//		Vector2 curScreenPoint = new Vector3(, Input.mousePosition.y);
//		Vector2 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		Debug.Log (hitPoint);	
		if (isHit) {
			transform.position = new Vector3 (hitPoint.x, hitPoint.y, transform.position.z);
		}
		Debug.Log ("touch stay");
	}
	void OntouchExit (Vector3 hitPoint){
		rend.material.color = defualtColor;
		Debug.Log ("touch exit");
	}

	void getTouchPosition(Vector3 hitPoint){
		
	}
}
