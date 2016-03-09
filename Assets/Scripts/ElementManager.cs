using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementManager : MonoBehaviour {

	public Transform entry;

	public GameObject element;
	public GameObject cat_element, bird_element, candle_element, dead_element, hat_element, hook_element,
					  pot_element, star_element, talk_element, wand_element, ghost_element;
	private GameObject[] elementsArr;

	public ArrayList preferArr;

	public static ElementManager instance;

	void Awake () {
		instance = this;
		preferArr = new ArrayList();
	}

	// Use this for initialization
	void Start () {
		elementsArr = new GameObject[] {
			cat_element, bird_element, candle_element, dead_element, hat_element, hook_element,
			pot_element, star_element, talk_element, wand_element, ghost_element
		};
	}
		
//	void checkNotPrefer(){
//		for(int i = 0; i < elementsArr; i++){
//			
//		}
//	}
	
	GameObject randomElementController () {
		float rand = Random.Range (0f, 1f);
		Debug.Log (rand);
		GameObject element;
		if(rand <= 0.35f){ element = talk_element; }
//		else if(rand <= 0.55f) { element = bird_element;}
		else if (rand <= 0.7f) {
			int prefer_rand = Random.Range (0, preferArr.Count);
			element = (GameObject)preferArr[prefer_rand];
		}
		else if( rand <= 0.9f) { element = ghost_element;}
		else {element = dead_element; }

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
//			int rand = Random.Range (0, elementsArr.Length);
//			Debug.Log (randomElementController());
			var new_element = Instantiate (randomElementController(), entry.position, Quaternion.identity) as GameObject;
			new_element.transform.parent = element.transform;
			new_element.transform.position = entry.position;
//			Debug.Log (new_element.transform.name);
		}

		countDistance = 0;
		//var new_element = Instantiate (cat_element, entry.position, Quaternion.identity) as GameObject;
		//new_element.transform.parent = element.transform;
		//new_element.transform.position = entry.position;

//		var instantElement = Instantiate (allElement[choice], new Vector3(spawnLocate, this.transform.position.y, -1), Quaternion.identity) as GameObject;
//		var instantElement_script = instantElement.GetComponent<ElementController>();
//		instantElement_script.isMove = true;
	}
		
	// Update is called once per frame
	void Update () {
		spawnElement ();
	}
}
