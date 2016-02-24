using UnityEngine;
using System.Collections;

public class ElementController : MonoBehaviour {

	private Transform pivot;
	public float speedMoveAround = 20f;
	public float speedMoveForward = 40f;

	private bool isSwipe;
	private bool isHit;


	private Vector3 v;

	Vector3 firstPressPos;
	Vector3 secondPressPos;
	Vector3 currentSwipe;

	// Use this for initialization
	void Start () {
		isSwipe = false;
		pivot = GameObject.Find("Element/Pivot").transform;
		v = transform.position - pivot.position;
		transform.localScale = new Vector3 (0.1872487f, 0.1872487f, 0.1872487f);
	}

	void moveAround (){
		v = Quaternion.AngleAxis(-Time.deltaTime * speedMoveAround, Vector3.forward) * v;
		//      Debug.Log (Quaternion.AngleAxis (Time.deltaTime * speedMoveAround, Vector3.forward) * v);
		if (!isHit) {
			transform.position = pivot.position + v;
		}
	}

	void moveForward (){
		transform.position += currentSwipe * speedMoveForward * Time.deltaTime;
	}

	void OnMouseDown(){
		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
		//save began touch 2d point
		firstPressPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		if (hit.collider.transform.tag == "Element") {
			isHit = true;

		}
	}

	void OnMouseUp(){
		//save ended touch 2d point
		secondPressPos = new Vector3(Input.mousePosition.x,Input.mousePosition.y,0);

		//create vector from the two points
		currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y,0);

		//normalize the 2d vector
		currentSwipe.Normalize();
		if (isHit) {
			isSwipe = true;
		}

		Debug.Log (currentSwipe);

		//          //swipe upwards
		//          if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
		//          {
		//              Debug.Log("up swipe");
		//          }
		//          //swipe down
		//          if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
		//          {
		//              Debug.Log("down swipe");
		//          }
		//          //swipe left
		//          if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
		//          {
		//              Debug.Log("left swipe");
		//          }
		//          //swipe right
		//          if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
		//          {
		//              Debug.Log("right swipe");
		//          }
	}

	void OnMouseDrag(){

		Vector2 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);

		transform.position = curPosition;
		Debug.Log (transform.position);
//		Debug.Log ("Drag!!");
	}

	void moveCommand(){
		if(!isSwipe){
			moveAround ();
		} else {
			moveForward ();
		}
	}

	// Update is called once per frame
	void Update () {
//				Debug.Log (transform.position);
		Debug.Log(transform.position - pivot.position);
		//      Debug.Log(Input.mousePosition);
//		Debug.Log(Time.deltaTime * speedMoveAround);
		moveCommand ();
		//      clickOnElement();
		//      Debug.DrawLine (transform.position, pivot.position);
		//      Debug.Log (Camera.main.ScreenPointToRay (Input.mousePosition));
	}

}
