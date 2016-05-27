using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PlayFab;
using PlayFab.ClientModels;
//using ArrayJSON;

using SocketIO;


public class GameController : MonoBehaviour {

	public static GameController instance;
	public SocketIOComponent socketIO;
	public GameObject wizard, wizard_2, wizard_3, wizard_4, wizard_5, wizard_6, wizard_7, wizard_8 , wizardPlayer2;
	public Text timerToFight_text;
	public Text endGame_text;

	//PlayFab ID of player
	public string PlayFabId;
	//WIN/LOSE
	private int win, lose;

	private static int checker;
	public SpriteRenderer startScene, restartScene, ritualScene, SuccessScene;
	private float timeStop;

	GameObject circleBar, gaugeBar, notice, magicCircle, botAura, score_canvas;//, ritualMission_canvas;
	bool isRitual;

	public bool isGameStart, isGameOver, isRitualSuccess, isFightPhase;

	public int score;
	private int timer;
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
		Debug.Log (PlayFabSettings.TitleId);
		//Socket.IO
		GameObject go = GameObject.Find("SocketIO");
		socketIO = go.GetComponent<SocketIOComponent>();
		socketIO.On ("PLAYER_ID", onPlayerID);
		socketIO.On ("START_GAME", onStartGame);
		socketIO.On ("FIGHT_TIMER", onTimerToFight);
		socketIO.On ("SPAWN_WIZARD", onSpawnEnemyWizard);
		socketIO.On ("START_FIGHT_PHASE", onStartFightPhase);
		socketIO.On ("ATK_TO_PLAYER", onEnemyATK);
		socketIO.On ("WIS_TO_PLAYER", onEnemyWIS);
		socketIO.On ("INT_TO_PLAYER", onEnemyINT);
		socketIO.On ("END_GAME", onEndGame);

		StartCoroutine("CalltoServer");

		GetUserData ();

		timerToFight_text.enabled = false;

		isGameStart = false;
		isFightPhase = false;

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
//		spawnWizard ();
	}


	//Call server
	private IEnumerator CalltoServer(){

		yield return new WaitForSeconds(1f);

		Debug.Log("Send message to the server");
		socketIO.Emit("USER_IN_GAME");

	}

	//Convert JsonToString
	string  JsonToString( string target, string s){
		string[] newString = Regex.Split(target,s);
		return newString[1];
	}

	//From server >> trigger to start spawn
	void onStartGame (SocketIOEvent obj) {
		spawnWizard ();
		timerToFight_text.enabled = true;
		isGameStart = true;
		timer = int.Parse(obj.data.GetField ("time").ToString());

	}

	//From server >> enemy's wizard random number
	void onSpawnEnemyWizard (SocketIOEvent obj) {
		int enemy_rand = int.Parse(JsonToString(obj.data.GetField("rand").ToString(), "\""));
		Debug.Log ("enemy spawn: " + enemy_rand);
		//		var instantElement = Instantiate (allWizard[rand], new Vector3 (-4.1f, 1.5f, 0.1f), Quaternion.identity) as GameObject;

	}

	void onStartFightPhase (SocketIOEvent obj) {
		isFightPhase = true;
		ArrayJSON wizardJSON = ArrayJSON.createFromJson (obj.data.ToString ());
//		wizardOfPlayer2Test = new float[int.Parse(JsonToString(obj.data.GetField("numb_wizard").ToString(), "\""))];
		wizardOfPlayer2Test = new float[wizardJSON.hp_wizard.Length];
		Debug.Log ("wizardOfPlayer2Test length: " + wizardOfPlayer2Test.Length);
		for(int i = 0; i < wizardJSON.hp_wizard.Length; i++){
			Debug.Log ("Enemy wizard " + i + " HP: " + wizardJSON.hp_wizard [i]);
			wizardOfPlayer2Test [i] = wizardJSON.hp_wizard [i];
		}


		addWizardForPlayer2 ();
	}

	void addWizardForPlayer2 (){
		int j = 1;
		float x = 6;
		float y = 0;
		for (int i = 0; i < wizardOfPlayer2Test.Length; i++) {
			int rand_wizard = Random.Range (0, allWizard.Length);
			var aWitch = Instantiate (wizardPlayer2, new Vector3 (0, allWizard [rand_wizard].transform.position.y, 0.1f), Quaternion.identity) as GameObject;
			aWitch.GetComponent<wizard2Controller>().maxHR = wizardOfPlayer2Test[i];

			if (j == wizardOfPlayer2Test.Length && j%2!=0) {
				y = height * 0.1f;
			} else {
				y = height*0.6f -((height) / (3))*(j%2+1);
			}
			aWitch.GetComponent<Transform>().position = new Vector3 (x, y, 0.1f);
			j += 1;
			if (j%2!= 0) {
				x -= 3.5f;
			}
		}

	}

	void onPlayerID (SocketIOEvent obj) {
		PlayFabId = obj.data.GetField ("id").ToString ();
		Debug.Log (PlayFabId);
	}

	public void noStartScene(){
		Application.LoadLevel (0);
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
//		addWizardForPlayer2 ();

		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		JSONObject json_arr = new JSONObject(JSONObject.Type.ARRAY);
		j.AddField("hp_wizard", json_arr);
		for(int i = 0; i < wizardArr.Length; i++){
			Debug.Log ("Wizard HP: " + wizardArr [i].GetComponent<WitchController> ().HP);
			json_arr.Add (wizardArr [i].GetComponent<WitchController> ().HP);
		}

		socketIO.Emit ("START_FIGHT_PHASE", j);
//		Dictionary<string, string> data = new Dictionary<string, string>();
//		data["numb_wizard"] = wizardArr.Length + "";
//		socketIO.Emit ("START_FIGHT_PHASE", new JSONObject(data));

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
		Dictionary<string, string> data = new Dictionary<string, string>();
		data["rand"] = rand_wizard+"";
		socketIO.Emit ("SPAWN_WIZARD", new JSONObject(data));

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
			aWitch.isRitual = true;
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
				y = height*0.6f -((height) / (3))*(j%2+1);
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
		updateHPWizardForPlayer2();
		isEndedGame ();
//		gameTimeScore ();

//		timertoFightCount ();
	}

	void timertoFightCount(){
		if(isGameStart) {
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
	}

	void onTimerToFight(SocketIOEvent obj) {
//		Debug.Log (obj.data.ToString ());
		ArrayJSON timeJSON = ArrayJSON.createFromJson (obj.data.ToString ());
		timerToFight = float.Parse (obj.data.GetField ("time").ToString ());
		Debug.Log ("Timer: " + timerToFight);

		if (timerToFight == 0) {
			startRitualPhase ();
			timerToFight_text.enabled = false;
//			timerToFight = -1;
		}

		timerToFight_text.text = "Time Left: " + (int)timerToFight;
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

	//mock atk
	void actionByATK (){
		Debug.Log ("atk");
		Debug.Log (wizardOfPlayer2Test.Length);
		float[] atkArr = new float[wizardOfPlayer2Test.Length];

		foreach (GameObject i in wizardArr) {
			WitchController aWitch = i.GetComponent<WitchController> ();
			if (aWitch.cooldown == aWitch.maxCooldown) {
				int rand = Random.Range (0,atkArr.Length);
				if (wizardOfPlayer2Test [rand] == 0) {
					for (int j = 0; j < wizardOfPlayer2Test.Length; j++) {
						rand = j;
						if (wizardOfPlayer2Test [rand] != 0) {
							break;
						}
					}
				}
//				while(wizardOfPlayer2Test [rand] != 0) {
//					rand = Random.Range (0,atkArr.Length);
//				}
				atkArr [rand] = aWitch.ATK;
				aWitch.cooldown = 0;
			}
		}

		//ATK ENEMY TO CLIENT DEVICE
		for (int i = 0; i < atkArr.Length; i++) {
			wizardOfPlayer2Test [i] -= atkArr [i];
			Debug.Log (wizardOfPlayer2Test [i]);

			if (wizardOfPlayer2Test [i] < 0) {
				atkArr [i] += wizardOfPlayer2Test [i];
				wizardOfPlayer2Test [i] = 0;
			}
		}

		//emit atkArr to socket.io
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		JSONObject json_arr = new JSONObject(JSONObject.Type.ARRAY);
		json.AddField("atk_arr", json_arr);
		foreach(float i in atkArr){
			json_arr.Add (i);
		}
		socketIO.Emit ("ATK_TO_PLAYER", json);
	}
	            
	void onEnemyATK (SocketIOEvent obj) {
		Debug.Log ("enemy ATK");

		//ATK FROM ENEMY
		ArrayJSON atk_arr = ArrayJSON.createFromJson (obj.data.ToString ());
		for (int i = 0; i < atk_arr.atk_arr.Length; i++) {
			Debug.Log ("Enemy atk_arr: " + atk_arr.atk_arr[i]);
			wizardArr [i].GetComponent<WitchController>().curHR -= atk_arr.atk_arr [i];
		}
	}

	//mock heal all
	void actionByWIS (){
		Debug.Log ("wis");
		int heal = 0;
		float[] hpArr = new float[wizardArr.Length];
		int temp = 0;
		float min = 0;
		for(int i =0; i < wizardArr.Length; i++) {
			WitchController aWitch = wizardArr[i].GetComponent<WitchController> ();
			if (i == 0 || min > aWitch.curHR && aWitch.curHR != 0) {
				min = aWitch.curHR;
				temp = i;
			}
			heal += aWitch.WIS;
		}

		WitchController w = wizardArr[temp].GetComponent<WitchController> ();
		Debug.Log ("before"+w.curHR);
		w.curHR += heal;
		Debug.Log ("after"+w.curHR);
		if (w.curHR > w.HP) {
			w.curHR = w.HP;
		}

		//emit healArr to socket.io
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		j.AddField ("wizard_index", temp);
		j.AddField("heal_point", heal);
		socketIO.Emit ("WIS_TO_PLAYER", j);


	}

	void onEnemyWIS (SocketIOEvent obj) {
		Debug.Log ("enemy WIS");
		//HEAL ENEMY
		ArrayJSON wis_arr = ArrayJSON.createFromJson (obj.data.ToString ());
		Debug.Log("Enemy heal wizard: " + wis_arr.wizard_index + " with heal: " + wis_arr.heal_point);
		wizardOfPlayer2Test [wis_arr.wizard_index] += wis_arr.heal_point;
	}

	void actionByInt (){
		Debug.Log ("int");
		float[] intArr = new float[wizardOfPlayer2Test.Length];
		foreach (GameObject i in wizardArr) {
			WitchController aWitch = i.GetComponent<WitchController> ();
			if (aWitch.cooldown == aWitch.maxCooldown) {
				for (int j = 0; j < intArr.Length; j++) {
					intArr [j] += aWitch.INT;
				}
				aWitch.cooldown = 0;
			}
		}
		for (int i = 0; i < intArr.Length; i++) {
			wizardOfPlayer2Test [i] -= intArr [i];
			if (wizardOfPlayer2Test [i] < 0) {
				intArr [i] += wizardOfPlayer2Test [i];
				wizardOfPlayer2Test [i] = 0;
			}
		}

		//emit atkArr to socket.io
		JSONObject json_obj = new JSONObject(JSONObject.Type.OBJECT);
		JSONObject json_arr = new JSONObject(JSONObject.Type.ARRAY);
		json_obj.AddField("int_arr", json_arr);
		foreach(float i in intArr){
			json_arr.Add (i);
		}
		socketIO.Emit ("INT_TO_PLAYER", json_obj);

	}

	void onEnemyINT (SocketIOEvent obj) {
		Debug.Log ("enemy INT");
		//INT FROM ENEMY
		ArrayJSON int_arr = ArrayJSON.createFromJson (obj.data.ToString ());
		for (int i = 0; i < int_arr.int_arr.Length; i++) {
			Debug.Log ("Enemy int_arr: " + int_arr.int_arr[i]);
			wizardArr [i].GetComponent<WitchController>().curHR -= int_arr.int_arr [i];
		}
	}

	//YOU LOSE!!
	void isEndedGame(){
		if(!isGameOver && isFightPhase) {
			int allZero = 0;
			for (int i = 0; i < wizardArr.Length; i++) {
				if (wizardArr [i].GetComponent<WitchController> ().curHR <= 0) {
					allZero++;
				}
			}
			if (allZero == wizardArr.Length) {
				Debug.Log ("You Lose");
				socketIO.Emit ("END_GAME");
				isGameOver = true;
				/*GameObject textGO = GameObject.Find("restartScene");
				endGame_text = textGO.GetComponent<Text> ();
				endGame_text.text = "You Lose";*/
				SetWinLose ("lose");
				restart ();
			}
		}
	}

	void onEndGame (SocketIOEvent obj) {
		Debug.Log ("You win");
		/*GameObject textGO = GameObject.Find("restartScene");
		endGame_text = textGO.GetComponent<Text> ();
		endGame_text.text = "You Win";
		endGame_text.color = Color.green;*/
		SetWinLose ("win");
		restart ();
	}

	//Playfab
	void SetWinLose(string key) {

		if (key == "win") {
			UpdateUserDataRequest request = new UpdateUserDataRequest () {
				Data = new Dictionary<string, string> () {
					{ "win", win+1 + "" },
				}
			};
			PlayFabClientAPI.UpdateUserData(request, (result) =>
				{
					Debug.Log("Successfully updated user data");
				}, (error) =>
				{
					Debug.Log("Got error setting user data Ancestor to Arthur");
					Debug.Log(error.ErrorDetails);
				});
		}

		if (key == "lose") {
			UpdateUserDataRequest request = new UpdateUserDataRequest () {
				Data = new Dictionary<string, string> () {
					{ "lose", lose+1 + "" },
				}
			};
			PlayFabClientAPI.UpdateUserData(request, (result) =>
				{
					Debug.Log("Successfully updated user data");
				}, (error) =>
				{
					Debug.Log("Got error setting user data Ancestor to Arthur");
					Debug.Log(error.ErrorDetails);
				});
		}
			
	}
		

	void GetUserData() {
		GetUserDataRequest request = new GetUserDataRequest() {
			PlayFabId = PlayFabId,
			Keys = null
		};

		PlayFabClientAPI.GetUserData(request,(result) => {
			Debug.Log("Got user data:");
			if ((result.Data == null) || (result.Data.Count == 0)) {
				Debug.Log("No user data available");
			} else {
				win = int.Parse(result.Data["win"].Value);
				lose = int.Parse(result.Data["lose"].Value);
				Debug.Log("WIN: " + win + ", LOSE: " + lose);
			}
		}, (error) => {
			Debug.Log("Got error retrieving user data:");
			Debug.Log(error.ErrorMessage);
		});
	}

	public void updateHPWizardForPlayer2(){
		GameObject[] wizardArr2 = GameObject.FindGameObjectsWithTag("Wizard2");
		int j = 0;
		foreach (GameObject i in wizardArr2) {
			i.GetComponent<wizard2Controller>().curHR = wizardOfPlayer2Test[j];
			j++;
		}
	}

//	public void addWizardForPlayer2(){
//		for (int i = 0; i < wizardOfPlayer2Test.Length; i++) {
//			int rand_wizard = Random.Range (0, allWizard.Length);
//			var aWitch = Instantiate (wizardPlayer2, new Vector3 (0, allWizard [rand_wizard].transform.position.y, 0.1f), Quaternion.identity) as GameObject;
//		}
//
//		GameObject[] wizardArr2 = GameObject.FindGameObjectsWithTag("Wizard2");
//		int j = 1;
//		float x = 6;
//		float y = 0;
//		foreach(GameObject i in wizardArr2){
//			//			if (!i.GetComponent<WitchController> ().isRitual) {
//			//				i.GetComponent<WitchController> ().gameObject.SetActive (false);
//			//			} else {
//			//				i.GetComponent<WitchController> ().curHR = 99;
//			//			}
//			WitchController aWitch = i.GetComponent<WitchController> ();
//			aWitch.isFreeze = true;
//			aWitch.isRitual = true;
//			aWitch.changeToHpBar();
//			//			i.GetComponentInChildren<Canvas> ().enabled = false;
//			//			GameObject[] prefers = GameObject.FindGameObjectsWithTag ("PreferElement");
//			//			foreach (GameObject p in prefers) {
//			//				p.GetComponent<Image> ().enabled = false;
//			//			}
//
//			//position of wizard in ritual phase;
//			if (j == wizardArr.Length && j%2!=0) {
//				y = height * 0.1f;
//			} else {
//				y = height*0.6f -((height) / (3))*(j%2+1);
//			}
//			i.GetComponent<Transform>().position = new Vector3 (x, y, 0.1f);
//			j += 1;
//			if (j%2!= 0) {
//				x -= 3.5f;
//			}
//
//
//		}
//		int j = 1;
//		float x = 6;
//		float y = 0;
//		GameObject[] wizardOfPlayer2 = new GameObject[wizardOfPlayer2Test.Length];
//		for(int i =0; i< wizardOfPlayer2Test.Length ; i++){
//			int rand_wizard = Random.Range (0, allWizard.Length);
//			var aWitch = Instantiate (allWizard[rand_wizard], new Vector3 (0, allWizard[rand_wizard].transform.position.y, 0.1f), Quaternion.identity) as GameObject;
//			WitchController w = aWitch.GetComponent<WitchController> ();
//			w.isFreeze = true;
//			w.isRitual = true;
//			w.curHR = 10;
//			Debug.Log ("aaaaaaaaaaaa");
//			Debug.Log (w.isRitual);
//			w.changeToHpBar();
//			if (j == wizardArr.Length && j%2!=0) {
//				y = height * 0.1f;
//			} else {
//				y = height*0.6f -((height) / (3))*(j%2+1);
//			}
//			w.GetComponent<Transform>().position = new Vector3 (x, y, 0.1f);
//			j += 1;
//			if (j%2!= 0) {
//				x -= 3.5f;
//			}
//			Debug.Log ("bbbbbbbb");
//			Debug.Log (w.isRitual);
//		}


//		GameObject[] wizardArr2 = GameObject.FindGameObjectsWithTag("Wizard2");
//		foreach(GameObject i in wizardArr2){
//			WitchController w = i.GetComponent<WitchController> ();
//			w.isFreeze = true;
//			w.isRitual = true;
//			w.changeToHpBar();
//		}


}
