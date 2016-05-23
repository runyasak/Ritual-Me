using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GameController : MonoBehaviour {

	public static GameController instance;

	public GameObject wizard, wizard_2, wizard_3, wizard_4, wizard_5, wizard_6, wizard_7, wizard_8;
	public Text timerToFight_text;

	private static int checker;
	public SpriteRenderer startScene, restartScene, ritualScene, SuccessScene;
	private float timeStop;

	GameObject circleBar, gaugeBar, notice, magicCircle, botAura, score_canvas;//, ritualMission_canvas;
	bool isRitual;

	public bool isGameOver, isRitualSuccess;

	public int score;
	public int timer;
	private float timerToFight;
	public float[] wizardOfPlayer2Test;

	public float ritualCounter, gameCounter;

	private GameObject[] wizardArr, allWizard;

	static Camera cam;
	static float height;
	static float width;

	void Awake () {
		DontDestroyOnLoad (instance);
		instance = this;
	}

	// Use this for initialization
	void Start () {

		allWizard = new GameObject[] {
			wizard,
			wizard_2,
			wizard_3,
			wizard_4,
			wizard_5,
			wizard_6,
			wizard_7,
			wizard_8
		};

		cam = Camera.main;
		height = 2f * cam.orthographicSize;
		width = height * cam.aspect;

		isRitualSuccess = false;
		startScene.enabled = true;
		restartScene.enabled = false;
		ritualScene.gameObject.SetActive (false);
		SuccessScene.gameObject.SetActive (false);
		restartScene.gameObject.SetActive (false);
		circleBar = GameObject.Find ("Element");
		gaugeBar = GameObject.Find ("Gauge");
		notice = GameObject.Find ("Notice");
		magicCircle = GameObject.Find ("MagicCircle");
		botAura = GameObject.Find ("BotAura");
//		ritualMission_canvas = GameObject.Find("RitualMission_Canvas");

		isGameOver = false;
		gameCounter = 0;
		score = 0;

//		ritualMission_canvas.SetActive (false);
		notice.SetActive (false);
		hideGauge ();
		spawnWizard ();


	}

	public void noStartScene(){
		Application.LoadLevel (1);
	}

	public void restart(){
		restartScene.enabled = true;
		restartScene.gameObject.SetActive (true);
		clearElement ();
		hideGauge ();
		hideBotAura ();
	}

//	void ritualPhaseCounter(){
//		if (isRitual) {
//			ritualCounter += Time.deltaTime;
//		
//			if (ritualCounter >= 20) {
//				stopRitualPhase ();
//				ritualCounter = 0;
//			}
//		}
//	}

	IEnumerator startCounting(){
		yield return new WaitForSeconds(2);

		ritualScene.gameObject.SetActive (false);
		hideMagicCircle ();
		hideBotAura ();
		notice.SetActive (true);
//		ritualMission_canvas.SetActive (true);
		unhideGauge ();
		RunnerController.instance.initiate ();
		ritualCounter = 0;
		isRitual = true;
	}

	public void startRitualPhase () {
		wizardRitualPhase ();
		ritualScene.gameObject.SetActive (true);
		ritualScene.enabled = true;
		StartCoroutine (startCounting());
		hideCircleBar ();
	}

	IEnumerator stopCounting(){
		yield return new WaitForSeconds(2);
		SuccessScene.gameObject.SetActive (false);
		unhideCircleBar ();
		unhideMagicCircle ();
		unhideBotAura ();
		notice.SetActive (false);
//		ritualMission_canvas.SetActive (false);
	}

//	public void stopRitualPhase () {
//		SuccessScene.gameObject.SetActive (true);
//		SuccessScene.enabled = true;
//		if (isRitualSuccess) {
//			SuccessScene.GetComponentInChildren<Canvas> ().GetComponentInChildren<Text> ().text = "Success";
//		} else {
//			SuccessScene.GetComponentInChildren<Canvas> ().GetComponentInChildren<Text> ().text = "Fail";
//		}
//		StartCoroutine (stopCounting());
//
//		RunnerController.instance.missText.SetActive(false);
//		RunnerController.instance.perfectText.SetActive(false);
//		hideGauge ();
//		isRitual = false;
////		wizardNotRitualPhase ();
////		addWizard ();
//	}

//	public void assignMissionText(string input_text) {
//		mission_text.text = input_text;		
//	}
//
//	public void assignTimerText(string input_text) {
//		timer_text.text = input_text;		
//	}
//
//	public void assignMissText(string input_text){
//		miss_text.text = input_text;	
//	}

	void scoreTextCommand() {
//		score_text.text = "Score: " + score;	
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
		
	public void addWizard(){

//		if (isRitualSuccess) {
			WitchController.instance.hrBar.GetComponent<Image> ().color = Color.red;
			WitchController.instance.lrText.SetActive (true);
			WitchController.instance.hrText.SetActive (false);
			spawnWizard();
//		} else {
//			WitchController.instance.curHR = 20;
//		}
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Wizard");
		int j = 1;

		foreach(GameObject i in temp){
			float x = (-width/2) +((width) / (temp.Length + 1))*j;
			float y = 2;
			i.GetComponent<Transform>().position = new Vector3 (x, y, 0.1f);
			j += 1;
		}
	}

	void spawnWizard () {
		int rand_wizard = Random.Range (0, allWizard.Length);
		var instantElement = Instantiate (allWizard[rand_wizard], new Vector3 (0, allWizard[rand_wizard].transform.position.y, 0.1f), Quaternion.identity) as GameObject;	
	}

	int rand_wizard () {
		return Random.Range (0, allWizard.Length);
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

//	void gameTimeScore (){
//		if (!isGameOver) {
//			gameCounter += Time.deltaTime;
//		}
//		if(gameCounter >= 1){
//			GameObject[] wizardArr = GameObject.FindGameObjectsWithTag ("Wizard");
//			score += 10*wizardArr.Length;
//			gameCounter = 0;
//		}
//	}

	void wizardRitualPhase(){
		wizardArr = GameObject.FindGameObjectsWithTag("Wizard");
		int j = 1;
		float x = -6;
		float y = 0;
		foreach(GameObject i in wizardArr){
//			if (!i.GetComponent<WitchController> ().isRitual) {
//				i.GetComponent<WitchController> ().gameObject.SetActive (false);
//			} else {
//				i.GetComponent<WitchController> ().curHR = 99;
//			}
			WitchController aWitch = i.GetComponent<WitchController> ();
			aWitch.isFreeze = true;
			aWitch.changeToHpBar();
//			i.GetComponentInChildren<Canvas> ().enabled = false;
//			GameObject[] prefers = GameObject.FindGameObjectsWithTag ("PreferElement");
//			foreach (GameObject p in prefers) {
//				p.GetComponent<Image> ().enabled = false;
//			}

			//position of wizard in ritual phase;
			if (j == wizardArr.Length && j%2!=0) {
				y = height * 0.1f;
			} else {
				y = height*0.6f -((height) / (3))*j;
			}
			i.GetComponent<Transform>().position = new Vector3 (x, y, 0.1f);
			j += 1;
			if (j%2!= 0) {
				x += 3.5f;
			}


		}
		GameObject[] prefers = GameObject.FindGameObjectsWithTag ("PreferElement");
		foreach (GameObject p in prefers) {
			p.GetComponent<Renderer> ().enabled = false;
		}

	}

//	void wizardNotRitualPhase(){
//		foreach(GameObject i in wizardArr){
//			i.GetComponent<WitchController> ().isRitual = false;
//			i.GetComponent<WitchController> ().isFreeze = false;
//			i.GetComponent<WitchController> ().gameObject.SetActive (true);
//		}
//	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) { clearElement (); }

//		ritualPhaseCounter ();


//		gameTimeScore ();

		timertoFightCount ();
//		scoreTextCommand();
	}

	void timertoFightCount(){
		if (timerToFight <= timer && timerToFight != -1) {
			timerToFight += Time.deltaTime;
		}
		if (timerToFight > timer) {
			startRitualPhase ();
			timerToFight_text.enabled = false;
			timerToFight = -1;
		}

		timerToFight_text.text = "Time Left: " + (timer - (int)timerToFight);
//		score_text.enabled = false;
	}

	public void fightAction(int actionType){
		if(actionType == 1){
			actionByATK ();
		}
		else if(actionType == 2){
			actionByWIS ();
		}
		else if(actionType == 3){
			actionByInt ();
		}
	}

	void actionByATK (){
		Debug.Log ("atk");
	}

	void actionByWIS (){
		Debug.Log ("wis");
	}

	void actionByInt (){
		Debug.Log ("int");
	}
}
