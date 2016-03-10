using UnityEngine;
using System.Collections;

public class RunnerController : MonoBehaviour {
	
	private Vector3 dir;
	private float mult;
	public static RunnerController instance;

	public GameObject LeftGauge;
	public GameObject RightGauge;

	public GameObject missText;
	public GameObject perfectText;

	public ParticleSystem talkEffect;
	public ParticleSystem elementEffect;

	private int countTalkElement;
	private Transform hitTemp;
	private Vector3 startPoint;

	public int countPerfect, countMiss, countCombo, randMission;

	private bool leftToRight;

	void Awake () {
		instance = this;
		startPoint = this.transform.position;
	}

	// Use this for initialization
	void Start () {
//		mult = 5f;
//		dir = new Vector3(1, 0, 0);
//		countTalkElement = 0;
//		missText.SetActive (false);
//		perfectText.SetActive (false);
//		leftToRight = true;
//		talkEffect.enableEmission = false;
		Debug.Log ("Start: "+startPoint);
//		initiate();
	}

	public void initiate () {
//		Debug.Log ("KUY");
		this.transform.position = startPoint;
		mult = 5f;
		dir = new Vector3(1, 0, 0);
		countTalkElement = 0;
		missText.SetActive (false);
		perfectText.SetActive (false);
		leftToRight = true;
		talkEffect.enableEmission = false;

		countPerfect = 0;
		countMiss = 0;
		countCombo = 0;
		randMission = Random.Range (0, 3);
		Debug.Log (randMission);
	}

	void move() {
		this.transform.position += dir * Time.deltaTime * 5f;
	}
	
	// Update is called once per frame
	void Update () {
		move ();
		checkElement ();
		checkTalkElement ();
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.tag == "Domain") {
			dir = -dir;
			countTalkElement = 0;
			if (leftToRight) {
				leftToRight = false;
			} else {
				leftToRight = true;
			}
//			GaugeController.instance.Create ();
		}
	}

	void checkElement(){
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray ray;
			ray = new Ray (transform.position, new Vector3 (0f, 0f, 1.0f));
			if (Physics.Raycast (ray, out hit, 100)) {
				Debug.Log (hit.transform.tag);
				if (hit.transform.tag == "Element") {
					Destroy (hit.transform.gameObject);
					countPerfect++;
					countCombo++;
					showElementEffect ();
					showPerfectText ();
				} else if (hit.transform.tag == "Domain" && countTalkElement == 0) {
					countTalkElement++;
					hitTemp = hit.transform;
				} else if (hit.transform.tag == "Witch") {
					showMissText ();
					countMiss++;
					countCombo = 0;
					Destroy (hit.transform.parent.parent.gameObject);
				} else {
					showMissText ();
					countMiss++;
					countCombo = 0;
				}
			}
		}
	}

	void checkTalkElement(){
		if (Input.GetMouseButton (0)) {
			RaycastHit hit; Ray ray;
			ray = new Ray (transform.position, new Vector3(0f,0f,1.0f));
			if (Physics.Raycast (ray, out hit, 100)) {
				if (hit.transform.tag == "Witch" && countTalkElement == 1) {
					showTalkEffect ();
					countTalkElement++;
				} else if (hit.transform.tag == "Domain" && countTalkElement == 2) {
					showTalkEffect ();
					countTalkElement++;
				} else if(hit.transform.tag == "Untagged" && (countTalkElement == 1|| countTalkElement == 3)){
					Destroy (hitTemp.parent.parent.gameObject);
					showMissText ();
					countMiss++;
					countCombo = 0;
					talkEffect.enableEmission = false;
					countTalkElement = 0;
				}
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			RaycastHit hit; Ray ray;
			ray = new Ray (transform.position, new Vector3(0f,0f,1.0f));
			if (Physics.Raycast (ray, out hit, 100)) {
				if (hit.transform.tag == "Domain" && countTalkElement == 3) {
					showPerfectText ();
					countPerfect++;
					countCombo++;
//					Debug.Log (countPerfect);
					Destroy (hit.transform.parent.parent.gameObject);
					talkEffect.enableEmission = false;
//					Debug.Log ("clearrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
				}

				if (countTalkElement > 0 && countTalkElement < 3) {//&& hit.transform.tag != "Untagged" && hit.transform.tag != "Element"){
//					Destroy (hit.transform.parent.parent.gameObject);
					Destroy (hitTemp.parent.parent.gameObject);
					showMissText ();
					countMiss++;
					countCombo = 0;
					talkEffect.enableEmission = false;
				} 
//				else if (countTalkElement == 10) {
//					Destroy (hitTemp.parent.parent.gameObject);
//					showMissText ();
//				}
				countTalkElement = 0;
			}

		}
	}

	void showMissText(){
		perfectText.SetActive (false);
		missText.SetActive (true);
	}

	void showPerfectText(){
		missText.SetActive (false);
		perfectText.SetActive (true);
	}

	void showTalkEffect(){
		if (leftToRight) {
			talkEffect.transform.localPosition = new Vector3 (-30, talkEffect.transform.localPosition.y, talkEffect.transform.localPosition.z);
			talkEffect.transform.localRotation = Quaternion.Euler (325, -90, 0); //leftToRight
		} else{
			talkEffect.transform.localPosition = new Vector3 (28, talkEffect.transform.localPosition.y, talkEffect.transform.localPosition.z);
			talkEffect.transform.localRotation = Quaternion.Euler (325, 90, 0); //RightToLeft
		}
		talkEffect.enableEmission = true;
	}

	void showElementEffect(){
		ParticleSystem newElementEffect = Instantiate (elementEffect, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z)
			, Quaternion.identity) as ParticleSystem;
		Destroy (newElementEffect.gameObject, 2);
	}
}
