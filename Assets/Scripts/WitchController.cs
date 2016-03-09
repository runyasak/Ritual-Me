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
//	public GameObject pentacle;
	public bool isFreeze;
	public GameObject heartTarget;



	public static WitchController instance;
	public GameObject cat_element, bird_element, candle_element, dead_element, hat_element, hook_element,
					  pot_element, star_element, talk_element, wand_element;

	private GameObject[] allElement;
	private GameObject[] prefer;

	private float count;

	void Awake(){
		instance = this;
		allElement = new GameObject[] {
			bird_element,
			candle_element,
			cat_element,
			hat_element,
			star_element,
			wand_element,
			dead_element,
			talk_element
		};
	}
	// Use this for initialization
	void Start () {
		isFreeze = false;
		addHRPoint = 10;
		minusHRPoint = 10;
		count = 0f;
		curHR = maxHR/2;
		prefer = new GameObject[3];
		assignPrefer ();
//		spawnPrefer ();
		printPrefer ();

		lrText.SetActive (false);
		hrText.SetActive (true);
//		pentacle.SetActive (false);
		heartTarget.SetActive (false);
		hrBar.GetComponent<Image> ().color = new Color(1,167f/255,167f/255,1);


	}

	void countTime (){
		count += 1 * Time.deltaTime;
	}

	void checkRitualPhase(){
		if (curHR == maxHR && hrText.active == true) {
			//			GameController.newScene = 1;
//			pentacle.SetActive(true);
			lrText.SetActive (true);
			hrText.SetActive (false);
			hrBar.GetComponent<Image> ().color = Color.red;
			GameController.instance.startRitualPhase ();
		}
		else if(curHR == 0 && lrText.active == true){
			lrText.SetActive (false);
			hrText.SetActive (true);
			hrBar.GetComponent<Image> ().color = new Color(1,167f/255,167f/255,1);
			curHR = maxHR;
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
					ElementManager.instance.preferArr.Add (prefer [i]);
				
				checkDuplicate[i] = rand;
				Debug.Log ("init " + prefer[i]);
			} else if(i != 0 && rand != checkDuplicate[0] && rand != checkDuplicate[1]) {
				prefer[i] = allElement[rand];
					ElementManager.instance.preferArr.Add (prefer [i]);
				checkDuplicate[i] = rand;
				Debug.Log (prefer[i]);
			} else {
				i--;
			}
		}
		//		prefer = new GameObject[]{bat_element, bird_element, candle_element};
	}

	void spawnPrefer(){
		GameObject temp = Instantiate(prefer[0]) as GameObject;
		temp.transform.parent = this.transform;
		temp.transform.localPosition= new Vector3 (0.3f, 0.3f, 0);
		temp.transform.localScale = new Vector3 (0.05f, 0.05f, 0);

		GameObject temp2 = Instantiate(prefer[1]) as GameObject;
		temp2.transform.parent = this.transform;
		temp2.transform.localPosition= new Vector3 (-0.3f, 0.3f, 0);
		temp2.transform.localScale = new Vector3 (0.05f, 0.05f, 0);

		GameObject temp3 = Instantiate(prefer[2]) as GameObject;
		temp3.transform.parent = this.transform;
		temp3.transform.localPosition= new Vector3 (-0.3f, -0.3f, 0);
		temp3.transform.localScale = new Vector3 (0.05f, 0.05f, 0);

	}

	void addHR(){
		curHR += addHRPoint;
		if (curHR > maxHR) {
			curHR = maxHR;
		}
	}

	void checkElement(GameObject element){
		int count_elem = 0;
		string element_name = element.name.Substring(0, element.name.Length - "(Clone)".Length);
		foreach (GameObject i in prefer) {
			Debug.Log ("i: " + i.name + "element: " + element.name);
			if(i.name == element_name){
				count_elem++;
				//				addHR (10);
				//				Debug.Log ("current HR: " + curHR);
			} 
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
			heartTarget.active = true;
			isFreeze = false;
			if (Input.GetKeyDown (KeyCode.H)) { addHR (); }
		} else {
			heartTarget.active = false;
		}

		checkRitualPhase ();
		deceaseHR ();

//		float myHR = curHR / maxHR;
		hrBar.transform.localScale = new Vector3 (curHR / maxHR, hrBar.transform.localScale.y, hrBar.transform.localScale.z);

	}
}
