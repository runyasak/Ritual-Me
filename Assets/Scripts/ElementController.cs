using UnityEngine;
using System.Collections;

public class ElementController : MonoBehaviour {

	private Transform pivot, dropPoint;
	public float speedMoveAround = 20f;
	public float speedMoveForward = 40f;

	private bool isSwipe, isHit, isDrop;

	private Vector3 v;

	private Transform one_click;
	private float timer_for_double_click;
	private float delay = 0.5f;

	Vector3 firstPressPos;
	Vector3 secondPressPos;
	Vector3 currentSwipe;

	// Use this for initialization
	void Start () {
		isSwipe = false;
		isDrop = false;
		one_click = null;
		timer_for_double_click = 0;
		pivot = GameObject.Find("Element/Pivot").transform;
		dropPoint = GameObject.Find ("Element/DropPoint").transform;
		v = transform.position - pivot.position;
		transform.localScale = new Vector3 (0.1872487f, 0.1872487f, 0.1872487f);
	}

	void moveAround (){
		v = Quaternion.AngleAxis(-Time.deltaTime * speedMoveAround, Vector3.forward) * v;
		//      Debug.Log (Quaternion.AngleAxis (Time.deltaTime * speedMoveAround, Vector3.forward) * v);
		if (!isHit) {
//			Debug.Log (this.gameObject + "" +isHit);
			transform.position = pivot.position + v;
		}
	}

	void moveForward (){
		transform.position += currentSwipe * speedMoveForward * Time.deltaTime;
	}

	void moveDrop (){
//		Debug.Log ("DROP!!");
		transform.position = Vector3.MoveTowards (transform.position, dropPoint.position, speedMoveForward*Time.deltaTime);
	}

	void OnMouseDown(){
		RaycastHit hit; Ray ray;
		ray = new Ray (Camera.main.ScreenToWorldPoint (Input.mousePosition), new Vector3(0f,0f,1.0f));
		firstPressPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y);
		if (Physics.Raycast (ray, out hit, 100)) {
			doubleClick (hit.transform);
			if (hit.collider.transform.tag == "Element") {
				isHit = true;
			}
		}

	}

	void OnMouseUp(){
		//save ended touch 2d point
		secondPressPos = new Vector3(Input.mousePosition.x,Input.mousePosition.y,0);

		//create vector from the two points
		currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y,0);


//		Vector3.Distance (secondPressPos, firstPressPos);
		//normalize the 2d vector
		currentSwipe.Normalize();
//		Debug.Log (currentSwipe);

		if (isHit) {
			if (currentSwipe == Vector3.zero ){
				isDrop = true;
			} else {
				isSwipe = true;
			}
		}

//		Debug.Log (currentSwipe);

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

	void doubleClick(Transform hit){
		bool timer_running;
		Debug.Log (Time.time);
		if(one_click == null){
			one_click = hit.transform;
			timer_for_double_click = Time.time;
		} else{
			if (one_click == hit) {
				Destroy (hit.gameObject);
			}
			one_click = null;
		}
	}

	void countDoubleClick(){
		if(one_click != null){
			if((Time. time - timer_for_double_click ) > delay){
				one_click = null;
			}
		}
	}

	void OnMouseDrag(){

		Vector2 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);

		transform.position = curPosition;
//		Debug.Log (transform.position);
//		Debug.Log ("Drag!!");
	}

	void moveCommand(){
		if(!isSwipe){
			moveAround ();
//			Debug.Log ("Moving Around");
		} else {
			moveForward ();
//			Debug.Log ("Moving Forward");
		}

		if (isDrop){
//			Debug.Log ("CHECK!!");
			moveDrop ();	
		}
	}

	// Update is called once per frame
	void Update () {
//		Debug.Log (transform.localPosition);
//		Debug.Log(transform.position - pivot.position);
		//      Debug.Log(Input.mousePosition);
//		Debug.Log(Time.deltaTime * speedMoveAround);
		moveCommand ();
		countDoubleClick ();

		//      clickOnElement();
		//      Debug.DrawLine (transform.position, pivot.position);
		//      Debug.Log (Camera.main.ScreenPointToRay (Input.mousePosition));
	}

}
