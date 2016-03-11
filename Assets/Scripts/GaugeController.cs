using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour {
	public static GaugeController instance ;
	private Vector3 gaugePosition;
	public GameObject r_element1, r_element2, r_element3, r_element4, r_element5, r_element6, r_element7, r_element8, r_element9;
	private GameObject[] elementArr;
	private GameObject[] arr;
	private GameObject[] arrTalk;
	private float timeForCreate;
	public int maxElement;

	public TalkElement talkElement;

	void Start () {
		gaugePosition = this.transform.position;
		elementArr = new GameObject[] {
			r_element1,
			r_element2,
			r_element3,
			r_element4,
			r_element5,
			r_element6,
			r_element7,
			r_element8,
			r_element9,
		};
		instance = this;
//		CreateTalkElement ();
	}

	// Update is called once per frame
	void Update () {
		timeForCreate += Random.Range (0,5);
		Create ();
		startMission ();
	}

	public void Create(){
		int rand = Random.Range (0,2);
		if (rand == 0) {
			CreateTalkElement ();
		} else {
			CreateElement ();
		}
	}

	public void startMission (){
		GameController.instance.assignMissionText ("Combo x6: " + (RunnerController.instance.countCombo).ToString ());
		GameController.instance.assignTimerText ("Time Left: " + ((int) (20 - GameController.instance.ritualCounter)).ToString());
		GameController.instance.assignMissText ("Miss Left: " + (3 - RunnerController.instance.countMiss).ToString ());
		if(RunnerController.instance.countCombo >= 6){
			GameController.instance.isRitualSuccess = true;
			GameController.instance.stopRitualPhase ();
			GameController.instance.score += 100;
		}else if (RunnerController.instance.countMiss >= 3 || GameController.instance.ritualCounter >= 20) {
			GameController.instance.isRitualSuccess = false;
			GameController.instance.stopRitualPhase ();
		}
	}


//	public void startMission (){
//		
//		if(RunnerController.instance.randMission == 0){
//			Debug.Log ("Perfect Mission");
//			GameController.instance.assignMissionText ("Perfect: " + (5 - RunnerController.instance.countPerfect).ToString ());
//			if(RunnerController.instance.countPerfect >= 5){
//				GameController.instance.stopRitualPhase ();
//				//success
//				GameController.instance.score += 100;
//			}
//		} else if(RunnerController.instance.randMission == 1){
//			Debug.Log ("Miss Mission");
//			GameController.instance.assignMissionText ("Miss: " + (3 - RunnerController.instance.countMiss).ToString ());
//			if(RunnerController.instance.countMiss >= 3){
//				GameController.instance.stopRitualPhase ();
//				//success
//				Debug.Log (RunnerController.instance.countMiss);
//			} else if (GameController.instance.ritualCounter >= 15) {
//				GameController.instance.score += 100;
//			}
//		} else if(RunnerController.instance.randMission == 2){
//			Debug.Log ("Combo Mission");
//			GameController.instance.assignMissionText ("Combo 3x: " + (RunnerController.instance.countCombo).ToString ());
//			Debug.Log (GameController.instance.ritualCounter.ToString ());
//			GameController.instance.assignTimerText ("Time: " + ((int) (15 - GameController.instance.ritualCounter)).ToString());
//			if(RunnerController.instance.countCombo == 3){
//				GameController.instance.stopRitualPhase ();
//				//success
//				GameController.instance.score += 100;
//				Debug.Log (RunnerController.instance.countMiss);
//			}
//		}
//	}

	void CreateElement(){
		float randomPosition = Random.Range (-7f, 7f);
		int distanceCheck = 0;
		arr = GameObject.FindGameObjectsWithTag ("Element");
		arrTalk = GameObject.FindGameObjectsWithTag ("TalkElement");

		foreach (GameObject i in arr){
			if (Mathf.Abs (randomPosition - i.transform.position.x) > 1.0f) {
				distanceCheck++;
			}
		}
		if (arrTalk.Length != 0) {
			foreach (GameObject j in arrTalk) {
				//						Debug.Log ("   bird|"+randomPosition +"   talk|"+j.GetComponent<TalkElement>().getLeft()+"   length|"+j.GetComponent<TalkElement>().cLenght);
				if (randomPosition > j.GetComponent<TalkElement> ().getLeft ()-1.5f
					&& randomPosition < j.GetComponent<TalkElement> ().getRight ()+1.5f) {
					distanceCheck = -1;

				}
			}
		}

		int[] preferWizard = WitchController.instance.GetComponent<WitchController> ().preferNumber;
		int prefer_rand = Random.Range (0, preferWizard.Length);	
		if ((distanceCheck == arr.Length) && timeForCreate > 200 && arr.Length < maxElement) {
			GameObject instantElement = Instantiate (elementArr[preferWizard[prefer_rand]], new Vector3(randomPosition, this.transform.position.y, -1)
				, Quaternion.identity) as GameObject;
			timeForCreate = 0;
		}
		distanceCheck = 0;
	}

	void CreateTalkElement(){
		float randomPosition = Random.Range (-6f, 5f);
		float randomLenght = Random.Range(2f, 11f);
		int distanceCheck = 0;
		float fixSizeCenter = randomLenght;
		if (randomLenght >= 7) {
			fixSizeCenter -= fixSizeCenter / 10.0f;
		}

		talkElement.talkLeft.transform.position = new Vector3 (randomPosition, this.transform.position.y, talkElement.talkRight.transform.position.z);
		talkElement.talkRight.transform.position = new Vector3 (randomPosition + randomLenght, talkElement.talkLeft.transform.position.y, talkElement.talkLeft.transform.position.z);
		talkElement.center.transform.position = new Vector3 (talkElement.talkLeft.transform.position.x, talkElement.talkLeft.transform.position.y, -0.1f);
		talkElement.center.transform.localScale = new Vector3 (fixSizeCenter, talkElement.center.transform.localScale.y, talkElement.center.transform.localScale.z);
		talkElement.cLenght = randomLenght;

		arr = GameObject.FindGameObjectsWithTag ("Element");
		arrTalk = GameObject.FindGameObjectsWithTag ("TalkElement");

//		Debug.Log (talkElement.getLeft()+"     L     "+ talkElement.talkLeft.transform.position.x);
//		Debug.Log (talkElement.getRight()+"     R     "+ talkElement.talkRight.transform.position.x);
//		Debug.Log ("-----------------------------------------------------------------------------------");
		foreach (GameObject i in arr) {
			if (i.transform.position.x < talkElement.GetComponent<TalkElement> ().getLeft () - 1.5f
				|| i.transform.position.x > talkElement.GetComponent<TalkElement> ().getRight () + 1.5f) {
				distanceCheck++;
				Debug.Log ("CreateTalkElement");
			}
		}

		if (randomPosition + randomLenght < 7 && timeForCreate > 200 && distanceCheck == arr.Length && arrTalk.Length ==0) {
			GameObject instantElement = Instantiate (talkElement, new Vector3 (talkElement.transform.position.x
			, talkElement.transform.position.y, talkElement.transform.position.z)
			, Quaternion.identity) as GameObject;
			timeForCreate = 0;
		}
	}
	
}
