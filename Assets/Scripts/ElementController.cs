using UnityEngine;
using System.Collections;

public class ElementController : MonoBehaviour {

	public Transform pivot ;
	public float speed = 100f;

	private Vector3 v;

	// Use this for initialization
	void Start () {
		v = transform.position - pivot.position;
	}

	void moveAround (){
		v = Quaternion.AngleAxis(Time.deltaTime * speed, Vector3.forward) * v;
		Debug.Log (Quaternion.AngleAxis (Time.deltaTime * speed, Vector3.forward) * v);
		transform.position = pivot.position + v;
	}

	// Update is called once per frame
	void Update () {
//		moveAround ();
		Debug.DrawLine (transform.position, pivot.position);
//		Debug.Log (Camera.main.ScreenPointToRay (Input.mousePosition));

		if(Input.GetMouseButton(0)){
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			if (hit.collider.transform.tag == "Element"){
				Debug.Log ("hit!");
			}
		}
	}

}
