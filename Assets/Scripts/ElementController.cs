using UnityEngine;
using System.Collections;

public class ElementController : MonoBehaviour {

	public Transform pivot ;
	public float speedMoveAround = 20f;
	public float speedMoveForward = 40f;

	private bool isClick;

	private Vector3 v;

	// Use this for initialization
	void Start () {
		isClick = false;
		v = transform.position - pivot.position;
	}

	void moveAround (){
		v = Quaternion.AngleAxis(Time.deltaTime * speedMoveAround, Vector3.forward) * v;
//		Debug.Log (Quaternion.AngleAxis (Time.deltaTime * speedMoveAround, Vector3.forward) * v);
		transform.position = pivot.position + v;
	}

	void moveForward (){
		if (transform.position.x >= 0) {
			transform.position += new Vector3 (speedMoveForward, speedMoveForward, 0f) * Time.deltaTime;
		} else {
			transform.position += new Vector3 (-speedMoveForward, speedMoveForward, 0f) * Time.deltaTime;
		}
	}
		
	void clickOnElement(){
		if(Input.GetMouseButtonDown(0)){
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			if (hit.collider.transform.tag == "Element"){
				isClick = true;
			}
		}
	}

	void moveCommand(){
		if(!isClick){
			moveAround ();
		} else {
			moveForward ();
		}
	}

	// Update is called once per frame
	void Update () {
		Debug.Log (transform.position);
		moveCommand ();
		clickOnElement();
//		Debug.DrawLine (transform.position, pivot.position);
//		Debug.Log (Camera.main.ScreenPointToRay (Input.mousePosition));
	}

}
