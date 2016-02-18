using UnityEngine;
using System.Collections;

public class ElementController : MonoBehaviour {

	public Transform pivot ;
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
		v = transform.position - pivot.position;
	}

	void moveAround (){
		v = Quaternion.AngleAxis(Time.deltaTime * speedMoveAround, Vector3.forward) * v;
//		Debug.Log (Quaternion.AngleAxis (Time.deltaTime * speedMoveAround, Vector3.forward) * v);
		transform.position = pivot.position + v;
	}

	void moveForward (){
		transform.position += currentSwipe * speedMoveForward * Time.deltaTime;
	}
		
	void clickOnElement(){
		if(Input.GetMouseButtonDown(0)){
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			if (hit.collider.transform.tag == "Element"){
				isSwipe = true;
			}
		}
	}

	void swipe() {
		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
		if(Input.GetMouseButtonDown(0)) {
			//save began touch 2d point
			firstPressPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
			if (hit.collider.transform.tag == "Element") {
				isHit = true;
			}
		}
		if(Input.GetMouseButtonUp(0))
		{
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

//			//swipe upwards
//			if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
//			{
//				Debug.Log("up swipe");
//			}
//			//swipe down
//			if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
//			{
//				Debug.Log("down swipe");
//			}
//			//swipe left
//			if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
//			{
//				Debug.Log("left swipe");
//			}
//			//swipe right
//			if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
//			{
//				Debug.Log("right swipe");
//			}
		}
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
//		Debug.Log (transform.position);
		swipe();
		moveCommand ();
//		clickOnElement();
//		Debug.DrawLine (transform.position, pivot.position);
//		Debug.Log (Camera.main.ScreenPointToRay (Input.mousePosition));
	}



}
