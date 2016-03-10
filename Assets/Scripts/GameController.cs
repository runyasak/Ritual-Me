using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController instance;
	public GameObject wizard;
	public Text mission_text, timer_text, score_text;

	private static int checker;
	public SpriteRenderer startScene, restartScene;

	GameObject circleBar, gaugeBar, notice, magicCircle, botAura, ritualMission_canvas, score_canvas;
	bool isRitual;

	public bool isGameOver;

	public int score;

	public float ritualCounter, gameCounter;

	private GameObject[] wizardArr;

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
		ritualMission_canvas = GameObject.Find("RitualMission_Canvas");

		isGameOver = false;
		gameCounter = 0;
		score = 0;

		ritualMission_canvas.SetActive (false);
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
		ritualMission_canvas.SetActive (true);
		unhideGauge ();
		RunnerController.instance.initiate ();
		ritualCounter = 0;

		wizardRitualPhase ();



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
		wizardNotRitualPhase ();
		addWizard ();
		notice.SetActive (false);
		ritualMission_canvas.SetActive (false);

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

	void scoreTextCommand() {
		score_text.text = "Score: " + score;	
	}

	void clearElement () {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Element");
		GameObject[] temp_talkElement = GameObject.FindGameObjectsWithTag("TalkElement");
		GameObject[] temp_deadElement = GameObject.FindGameObjectsWithTag("LockElement");
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

	void gameTimeScore (){
		if (!isGameOver) {
			gameCounter += Time.deltaTime;
		}
		if(gameCounter >= 1){
			GameObject[] wizardArr = GameObject.FindGameObjectsWithTag ("Wizard");
			score += 10*wizardArr.Length;
			gameCounter = 0;
		}
	}

	void wizardRitualPhase(){
		wizardArr = GameObject.FindGameObjectsWithTag("Wizard");
		foreach(GameObject i in wizardArr){
			if (!i.GetComponent<WitchController> ().isRitual) {
				i.GetComponent<WitchController> ().gameObject.SetActive (false);
			} else {
				i.GetComponent<WitchController> ().curHR = 99;
			}
			i.GetComponent<WitchController> ().isFreeze = true;
		}
	}

	void wizardNotRitualPhase(){
//		GameObject[] wizardArr = GameObject.FindGameObjectsWithTag("Wizard");
		foreach(GameObject i in wizardArr){
			if (i.GetComponent<WitchController> ().isRitual) {
				i.GetComponent<WitchController> ().hrBar.GetComponent<Image> ().color = Color.red;
				i.GetComponent<WitchController> ().lrText.SetActive (true);
				i.GetComponent<WitchController> ().hrText.SetActive (false);
			}
			i.GetComponent<WitchController> ().isRitual = false;
			i.GetComponent<WitchController> ().isFreeze = false;
			i.GetComponent<WitchController> ().gameObject.SetActive (true);
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) { clearElement (); }

		if (checker == 1) {
			startGame ();
		}

		ritualPhaseCounter ();

		gameTimeScore ();
		scoreTextCommand();


//		if (Input.GetKeyDown (KeyCode.Q)) { hideCircleBar (); }
//		if (Input.GetKeyDown (KeyCode.W)) { unhideCircleBar (); }
	}
}
