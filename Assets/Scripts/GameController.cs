using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController instance;
	public GameObject wizard;

	GameObject circleBar, gaugeBar, notice, magicCircle;
	bool isRitual;
	float ritualCounter;

	static Camera cam = Camera.main;
	static float height = 2f * cam.orthographicSize;
	static float width = height * cam.aspect;

	void Awake () {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		circleBar = GameObject.Find ("Element");
		gaugeBar = GameObject.Find ("Gauge");
		notice = GameObject.Find ("Notice");
		magicCircle = GameObject.Find ("MagicCircle");
		notice.SetActive (false);
		hideGauge ();
	}

	void ritualPhaseCounter(){
		if (isRitual) {
			ritualCounter += Time.deltaTime;

			if (ritualCounter >= 15) {
				stopRitualPhase ();
				ritualCounter = 0;
//				RitualController.instance.stopRitual();
			}
		}
	}


	public void startRitualPhase () {
		hideCircleBar ();
		hideMagicCircle ();
		notice.SetActive (true);
		unhideGauge ();
		isRitual = true;

//		GameObject[] temp = GameObject.FindGameObjectsWithTag("Wizard");
//		foreach(GameObject i in temp){
//			if (WitchController.instance != i) {
//				i.GetComponent<WitchController> ().isFreeze = true;
//			}
//			Debug.Log ("isFreeze      "+      i.GetComponent<WitchController> ().isFreeze);
//		}
		MovieController.instance.playRitual ();
	}

	public void stopRitualPhase () {
		unhideCircleBar ();
		unhideMagicCircle ();
		hideGauge ();
		isRitual = false;
		addWizard ();
		notice.SetActive (false);

//		GameObject[] temp = GameObject.FindGameObjectsWithTag("Wizard");
//		foreach(GameObject i in temp) {
//			if (WitchController.instance != i) {
//				i.GetComponent<WitchController> ().isFreeze = false;
//				//				i.GetComponent<PlayerController> ().pentacle.active = false;
//			}
//		}
			
		MovieController.instance.stopRitual();
	}

	void clearElement () {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Element");
		GameObject[] temp_talkElement = GameObject.FindGameObjectsWithTag("TalkElement");
		GameObject[] temp_deadElement = GameObject.FindGameObjectsWithTag("DeadElement");
		foreach(GameObject i in temp){
			Destroy (i);	
		}
		foreach(GameObject i in temp_talkElement){
			Destroy (i);	
		}
		foreach(GameObject i in temp_deadElement){
			Destroy (i);	
		}
	}
		
	void addWizard(){

		var instantElement_1 = Instantiate (wizard, new Vector3(0, wizard.transform.position.y, -1), Quaternion.identity) as GameObject;
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Wizard");
		Debug.Log ("lllllllllllllll"+temp.Length);
		int j = 1;

		foreach(GameObject i in temp){
			float x = (-width/2) +((width) / (temp.Length + 1))*j;
			i.GetComponent<Transform>().position = new Vector3 (x, i.transform.position.y, 0.1f);
			Debug.Log ("ppppppppppppppppppppppppp"+i.GetComponent<Transform>().position);
			j += 1;
		}

	}

	void hideCircleBar () {
		clearElement ();
		circleBar.SetActive (false);
	}
		
	void hideGauge () {
		clearElement ();
		gaugeBar.SetActive (false);
	}

	void hideMagicCircle() {
		clearElement ();
		magicCircle.SetActive (false);
	}

	void unhideCircleBar () { circleBar.SetActive (true); }
	void unhideGauge () { gaugeBar.SetActive (true); }
	void unhideMagicCircle() { magicCircle.SetActive (true); }

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) { clearElement (); }

		ritualPhaseCounter ();

//		if (Input.GetKeyDown (KeyCode.Q)) { hideCircleBar (); }
//		if (Input.GetKeyDown (KeyCode.W)) { unhideCircleBar (); }
	}
}
