using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WitchController : MonoBehaviour {


	public float maxHR = 100;
	public float curHR = 0;
	public float addHRPoint;
	public float minusHRPoint;
	public float decreaseSpeedHR = 2;
	public float decreaseSpeedLR = 1;
	public GameObject hrBar;
	public GameObject hrText;
	public GameObject lrText;
	public SpriteRenderer angryEmo;
	public SpriteRenderer loveEmo;
	private float countEmo;

	public ParticleSystem healEffect;
	public ParticleSystem debuffEffect;

//	public GameObject pentacle;
	public bool isFreeze;
	public bool isRitual;



	public static WitchController instance;
	public GameObject bat_prefer, bird_prefer, cat_prefer, candle_prefer, hat_prefer, hook_prefer,
		pot_prefer, star_prefer, wand_prefer;

	private GameObject[] allElement;
	private GameObject[] prefer;
	public int[] preferNumber;

	private GameObject prefer1, prefer2, prefer3;

	private float count;

	void Awake(){
		instance = this;
		allElement = new GameObject[] {
			bat_prefer,
			bird_prefer,
			cat_prefer,
			candle_prefer,
			hat_prefer,
			hook_prefer,
			pot_prefer,
			star_prefer,
			wand_prefer,
		};
		preferNumber = new int[3];
	}
	// Use this for initialization
	void Start () {
		isRitual = false;
		isFreeze = false;
		addHRPoint = 10;
		minusHRPoint = 10;
		count = 0f;
		curHR = maxHR/2;
		prefer = new GameObject[3];
		assignPrefer ();
		spawnPrefer ();

		spawnEmo ();
		loveEmo.enabled = false;
		angryEmo.enabled = false;
//		printPrefer ();

		lrText.SetActive (false);
		hrText.SetActive (true);
//		pentacle.SetActive (false);
//		heartTarget.SetActive (false);
		hrBar.GetComponent<Image> ().color = new Color(1,167f/255,167f/255,1);

	}

	void countTime (){
		count += 1 * Time.deltaTime;
	}

	void checkRitualPhase(){
		if (curHR == maxHR && hrText.active == true) {
			//			GameController.newScene = 1;
//			pentacle.SetActive(true);
			isRitual = true;
			this.transform.position = new Vector3(0,2,this.transform.position.z);
//			lrText.SetActive (true);
//			hrText.SetActive (false);
//			hrBar.GetComponent<Image> ().color = Color.red;
			GameController.instance.startRitualPhase ();
		}
		/*else if(curHR == 0 && lrText.active == true){
			lrText.SetActive (false);
			hrText.SetActive (true);
			hrBar.GetComponent<Image> ().color = new Color(1,167f/255,167f/255,1);
			curHR = maxHR;
		}*/
	}
	void checkGameover(){
		if (curHR == 0) {
			GameController.instance.restart ();
		}
	}

	void OnMouseDown(){
		instance = this;
	}

	void assignPrefer(){
		int[] checkDuplicate = new int[3];
//		foreach(GameObject i in ElementManager.instance.preferArr){
//			Debug.Log ("Element Manager: " + i);
//		}
//		GameObject[] oldPrefer = new GameObject[ElementManager.instance.preferArr.Length];
//
//		System.Array.Copy(oldPrefer, ElementManager.instance.preferArr, ElementManager.instance.preferArr.Length);
//		Debug.Log (oldPrefer.Length);
//		foreach(GameObject i in oldPrefer){
//			Debug.Log ("OLD: " + i);
//		}
		for(int i = 0; i < prefer.Length; i++){
			int rand = Random.Range(0, allElement.Length);
			if(i == 0){
				prefer[i] = allElement[rand];
				preferNumber [i] = rand;
//				ElementManager.instance.preferArr.Add (preferNumber [i]);
				checkDuplicate[i] = rand;
				Debug.Log ("init " + prefer[i]);
			} else if(i != 0 && rand != checkDuplicate[0] && rand != checkDuplicate[1]) {
				prefer[i] = allElement[rand];
				preferNumber [i] = rand;
//				ElementManager.instance.preferArr.Add (preferNumber [i]);
				checkDuplicate [i] = rand;
				Debug.Log (prefer[i]);
			} else {
				i--;
			}
		}
			
		//		prefer = new GameObject[]{bat_element, bird_element, candle_element};
	}

	void spawnPrefer(){
		prefer1 = Instantiate(prefer[0]) as GameObject;
		prefer1.transform.localPosition= new Vector3 (this.transform.position.x + 0.75f, this.transform.position.y + 0.75f, 0);

		prefer2 = Instantiate(prefer[1]) as GameObject;
		prefer2.transform.localPosition= new Vector3 (this.transform.position.x + 0.75f, this.transform.position.y , 0);

		prefer3 = Instantiate(prefer[2]) as GameObject;
		prefer3.transform.localPosition= new Vector3 (this.transform.position.x + 0.75f, this.transform.position.y - 0.75f, 0);

	}

	void Moveprefer(){
		prefer1.transform.localPosition= new Vector3 (this.transform.position.x + 0.75f, this.transform.position.y + 0.75f, 0);
		prefer2.transform.localPosition= new Vector3 (this.transform.position.x + 0.75f, this.transform.position.y , 0);
		prefer3.transform.localPosition= new Vector3 (this.transform.position.x + 0.75f, this.transform.position.y - 0.75f, 0);
	}

	void spawnEmo(){
		angryEmo.transform.position= new Vector3 (this.transform.position.x - 0.75f, this.transform.position.y + 1.25f, 0);
		loveEmo.transform.position= new Vector3 (this.transform.position.x - 0.75f, this.transform.position.y + 1.0f, 0);
//		angryEmo.transform.localPosition= new Vector3 (this.transform.position.x - 1.25f, this.transform.position.y - 2.0f, 0);
//		loveEmo.transform.localPosition= new Vector3 (this.transform.position.x - 1.25f, this.transform.position.y - 2.5f, 0);
	}

	void showHealEffect(){
		ParticleSystem newHealElementEffect = Instantiate (healEffect, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z)
			, Quaternion.identity) as ParticleSystem;
		Destroy (newHealElementEffect.gameObject, 2);
	}

	void showDebuffEffect(){
		ParticleSystem newDebuffElementEffect = Instantiate (debuffEffect, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z)
			, Quaternion.identity) as ParticleSystem;
		Destroy (newDebuffElementEffect.gameObject, 2);
	}

	void addHR(float hr){
		curHR += hr;
		if (curHR > maxHR) {
			curHR = maxHR;
		}
		loveEmo.enabled = true;
		angryEmo.enabled = false;
		countEmo = 0;
		showHealEffect ();
	}

	void addHR(){
		curHR += addHRPoint;
		if (curHR > maxHR) {
			curHR = maxHR;
		}
		loveEmo.enabled = true;
		angryEmo.enabled = false;
		countEmo = 0;
		showHealEffect ();
	}

	void checkElement(GameObject element){
		int count_elem = 0;
		string element_name = element.name.Substring(0, element.name.Length - "_element(Clone)".Length);
		foreach (GameObject i in prefer) {
			string i_name = i.name.Substring(0, i.name.Length - "_prefer".Length);
			Debug.Log ("i: " + i_name + "                  element: " + element_name);
			if (element_name == "talk") {
				Debug.Log ("talkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
				count_elem = 2;
			}
			if(i_name == element_name){
				count_elem++;
				//				addHR (10);
				//				Debug.Log ("current HR: " + curHR);
			} 
		}
		if (count_elem == 2) {
			//			Debug.Log("yesssssss   Input "+element); 
			addHR (5f);
		}

		if (count_elem == 1) {
//			Debug.Log("yesssssss   Input "+element); 
			addHR ();
		}
		else if(count_elem == 0) {
//			Debug.Log("noooooooo   Input"+element); 
			minusHR ();
		}
	}

	void minusHR(){
		curHR -= minusHRPoint;
		if (curHR < 0) {
			curHR = 0;
		}
		angryEmo.enabled = true;
		loveEmo.enabled = false;
		countEmo = 0;
		showDebuffEffect ();
	}



	private void deceaseHR(){
		float decreaseSpeed = 1;
		if (isFreeze) {
			decreaseSpeed = 0;
		} else if (lrText.active) {
			decreaseSpeed = decreaseSpeedLR;
		} else if (hrText.active) {
			decreaseSpeed = decreaseSpeedHR;
		}

		if (curHR > 0) {
			curHR -= Time.deltaTime * decreaseSpeed;
		}
		if (curHR < 0) {
			curHR = 0;
		}

	}

	void showPrefer(){
		if (count > 3f) {
			count = 0;
		}
	}

	private void printPrefer(){
//		Debug.Log (prefer[0] + " " + prefer[1]+ " " + prefer[2]);
//		Debug.Log(ElementManager.instance.preferArr);
		foreach(GameObject i in prefer){
			Debug.Log ("Witch: " + i);
		}
		if (ElementManager.instance.preferArr != null) {
			foreach (GameObject i in ElementManager.instance.preferArr) {
				Debug.Log ("Element Manager: " + i);
			}
		}


	}

	void OnTriggerEnter(Collider coll) {
		Debug.Log ("HIT");
		checkElement (coll.gameObject);
		Destroy (coll.gameObject);
//		addHR ();
//		ElementManager.instance.spawnElement ();
	}

	// Update is called once per frame
	void Update () {
		countTime ();

		showPrefer ();

		if (instance == this) {
//			heartTarget.active = true;
//			isFreeze = false;
			if (Input.GetKeyDown (KeyCode.H)) { addHR (); }
			if (Input.GetKeyDown (KeyCode.L)) { minusHR (); }
		} else {
//			heartTarget.active = false;
		}

		checkRitualPhase ();
		checkGameover ();
		deceaseHR ();

//		float myHR = curHR / maxHR;
		hrBar.transform.localScale = new Vector3 (curHR / maxHR, hrBar.transform.localScale.y, hrBar.transform.localScale.z);
		Moveprefer ();

		spawnEmo ();
		countEmo += Time.deltaTime;
		if (countEmo >= 1.5) {
			loveEmo.enabled = false;
			angryEmo.enabled = false;
		}
	}
}
