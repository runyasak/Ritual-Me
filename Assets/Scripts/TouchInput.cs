using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour
{	
	public LayerMask touchInputMask;
	private List<GameObject> touchList = new List<GameObject> ();
	private GameObject[] touchesOld;

	private RaycastHit hit;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{

#if UNITY_EDITOR
//		if (Input.GetMouseButton(0)|| Input.GetMouseButtonUp(0)|| Input.GetMouseButtonDown(0)) {
			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo (touchesOld);
			touchList.Clear();

				Ray ray = new Ray (Camera.main.ScreenToWorldPoint (Input.mousePosition), new Vector3(0f,0f,1.0f));

				if (Physics.Raycast (ray, out hit, touchInputMask)) {
					GameObject hitObj = hit.transform.gameObject;
					touchList.Add (hitObj);

					if (Input.GetMouseButtonDown(0)) {
					Debug.Log (hit.point);

						hitObj.SendMessage ("OntouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if (Input.GetMouseButtonUp(0)) {
						hitObj.SendMessage ("OntouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if (Input.GetMouseButton(0)) {
				hitObj.SendMessage ("OntouchStay", Camera.main.ScreenToWorldPoint (Input.mousePosition), SendMessageOptions.DontRequireReceiver);
					}
				}
			

//			foreach (GameObject g in touchesOld) {
//				if (!touchList.Contains (g)) {
//					g.SendMessage ("OntouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
//				}
//			}
//		}
#endif
		if (Input.touchCount > 0) {
			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo (touchesOld);
			touchList.Clear();

			Debug.Log ("aaaaaaaaaaaaaaaaaa");

			foreach (Touch touch in Input.touches) {
				Debug.Log (touch);
				Ray ray1 = new Ray (Camera.main.ScreenToWorldPoint (touch.position), new Vector3(0f,0f,1.0f));

				if (Physics.Raycast (ray1, out hit, touchInputMask)) {
					GameObject hitObj = hit.transform.gameObject;
					touchList.Add (hitObj);

					if (touch.phase == TouchPhase.Began) {
						hitObj.SendMessage ("OntouchDown", touch, SendMessageOptions.DontRequireReceiver);
					}
					if (touch.phase == TouchPhase.Ended) {
						hitObj.SendMessage ("OntouchUp", touch, SendMessageOptions.DontRequireReceiver);
					}
					if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
						hitObj.SendMessage ("OntouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
					}
					if (touch.phase == TouchPhase.Canceled) {
						hitObj.SendMessage ("OntouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
					}
				}
			}

			foreach (GameObject g in touchesOld) {
				if (!touchList.Contains (g)) {
					g.SendMessage ("OntouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
				}

			}


		}
	}
}

