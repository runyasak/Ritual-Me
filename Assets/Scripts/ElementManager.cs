using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementManager : MonoBehaviour {

	public Transform entry;

	public GameObject element;
	public GameObject bat_element, bird_element, cat_element, candle_element, hat_element, hook_element,
	pot_element, star_element, wand_element, talk_element, ghost_element, lock_element;
	private GameObject[] elementsArr;

	public ArrayList preferArr, notPreferArr;

	public static ElementManager instance;

	void Awake () {
		instance = this;
		preferArr = new ArrayList();
	}

	// Use this for initialization
	void Start () {
		elementsArr = new GameObject[] {
			bat_element, bird_element, cat_element, candle_element, hat_element, hook_element,
			pot_element, star_element, wand_element, talk_element, ghost_element, lock_element
		};
	}

	public void deleteDuplicatePrefer () {
		foreach(GameObject i in preferArr) {
			foreach(GameObject j in preferArr) {
				if(j == i)
					preferArr.Remove(i);
			}
		}
	}

	public void createNotPrefer () {
		foreach (GameObject i in elementsArr) {
			if(!preferArr.Contains(System.Array.IndexOf (elementsArr, i))) {
				notPreferArr.Add(System.Array.IndexOf (elementsArr, i));
			}
		}
	}
	
	GameObject randomElementController () {
		float rand = Random.Range (0f, 1f);
		GameObject element = null;
		if(rand <= 0.35f){ element = talk_element; }
		/*else if(rand <= 0.55f) { 
			//not prefer
			int all_rand;
			GameObject[] wizardArr = GameObject.FindGameObjectsWithTag ("Wizard");
			int wizard_rand =Random.Range (0, wizardArr.Length);

			int[] preferWizard = wizardArr [wizard_rand].GetComponent<WitchController> ().preferNumber;
			do{
				all_rand = Random.Range (0, 9);
			} while(System.Array.IndexOf(preferWizard, all_rand)>=0);

			element = elementsArr[all_rand];
		}*/
		else if (rand <= 0.8f) {
			/*//prefer
			//choose wizard
			GameObject[] wizardArr = GameObject.FindGameObjectsWithTag ("Wizard");
			int wizard_rand =Random.Range (0, wizardArr.Length);

			//choose prefer from wizard
			int[] preferWizard = wizardArr [wizard_rand].GetComponent<WitchController> ().preferNumber;
			int prefer_rand = Random.Range (0, preferWizard.Length);

			element = elementsArr[preferWizard[prefer_rand]];*/
			int all_rand;
			all_rand = Random.Range (0, 9);
			element = elementsArr[all_rand];
		}
		else if( rand <= 1f) { element = ghost_element;}

		return element;
	}
		
	public void spawnElement () {
		GameObject[] eleArr = GameObject.FindGameObjectsWithTag ("Element");
		float countDistance = 0;
		foreach (GameObject i in eleArr) {
			if (Vector3.Distance (entry.position, i.transform.position) > 2.6) {
				countDistance++;
			}
		}

		if (countDistance == eleArr.Length) {
			var new_element = Instantiate (randomElementController(), entry.position, Quaternion.identity) as GameObject;
			new_element.transform.parent = element.transform;
			new_element.transform.position = entry.position;
			int rand = Random.Range (0,10);
			if (rand == 0) {
				var lockElement = Instantiate (elementsArr[elementsArr.Length-1], entry.position, Quaternion.identity) as GameObject;
				lockElement.transform.parent = element.transform;
				lockElement.transform.position = entry.position;
			}
		}

		countDistance = 0;
	}
		
	// Update is called once per frame
	void Update () {
		spawnElement ();
	}
}
