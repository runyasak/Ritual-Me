using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController instance;
	public GameObject wizard;
	public Text mission_text, timer_text;

	private static int checker;
	public SpriteRenderer startScene, restartScene;

	GameObject circleBar, gaugeBar, notice, magicCircle, botAura;
	bool isRitual;

	public float ritualCounter;

	static Camera cam = Camera.main;
	static float height = 2f * cam.orthographicSize;
	static float width = height * cam.aspect;

	void Awake () {
		DontDestroyOnLoad (instance);
		instance = this;
	}

	// Use this for initialization
	void Start () {
		Time.timeScale = 0;
		startScene.enabled = true;
		restartScene.enabled = false;
		restartScene.gameObject.SetActive (false);
		circleBar = GameObject.Find ("Element");
		gaugeBar = GameObject.Find ("Gauge");
		notice = GameObject.Find ("Notice");
		magicCircle = GameObject.Find ("MagicCircle");
		botAura = GameObject.Find ("BotAura");
		notice.SetActive (false);
		hideGauge ();
	}

	public void noStartScene(){
		checker = 1;
		Application.LoadLevel (0);
	}

	public void startGame(){
		startScene.gameObject.SetActive (false);
		Time.timeScale = 1;
	}

	public void restart(){
		restartScene.enabled = true;
		restartScene.gameObject.SetActive (true);
		clearElement ();
		hideGauge ();
		hideBotAura ();
	}

	void ritualPhaseCounter(){
		if (isRitual) {
			ritualCounter += Time.deltaTime;

			if (ritualCounter >= 15) {
				stopRitualPhase ();
				ritualCounter = 0;
			}
		}
	}

	public void startRitualPhase () {
		hideCircleBar ();
		hideMagicCircle ();
		hideBotAura ();
		notice.SetActive (true);
		unhideGauge ();
		RunnerController.instance.initiate ();

//		GaugeController.instance.startMission ();
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
		unhideBotAura ();
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

	public void assignMissionText(string input_text) {
		mission_text.text = input_text;		
	}

	public void assignTimerText(string input_text) {
		timer_text.text = input_text;		
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

	void hideBotAura() {
		clearElement ();
		botAura.SetActive (false);
	}

	void unhideCircleBar () { circleBar.SetActive (true); }
	void unhideGauge () { gaugeBar.SetActive (true); }
	void unhideMagicCircle() { magicCircle.SetActive (true); }
	void unhideBotAura(){ botAura.SetActive (true); }

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) { clearElement (); }

		if (checker == 1) {
			startGame ();
		}

		ritualPhaseCounter ();

//		if (Input.GetKeyDown (KeyCode.Q)) { hideCircleBar (); }
//		if (Input.GetKeyDown (KeyCode.W)) { unhideCircleBar (); }
	}
}
