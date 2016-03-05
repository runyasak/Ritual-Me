using UnityEngine;
using System.Collections;

public class ElementManager : MonoBehaviour {

	public Transform entry;

	public GameObject element;
	public GameObject cat_element, bird_element, candle_element, dead_element, hat_element, hook_element,
						pot_element, star_element, talk_element, wand_element;
	private GameObject[] elementsArr;

	public static ElementManager instance;



	void Awake () {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		elementsArr = new GameObject[] {
			cat_element, bird_element, candle_element, dead_element, hat_element, hook_element,
			pot_element, star_element, talk_element, wand_element
		};
	}
		
	public void spawnElement(){
		GameObject[] eleArr = GameObject.FindGameObjectsWithTag ("Element");
		float countDistance = 0;
		foreach (GameObject i in eleArr) {
			if (Vector3.Distance (entry.position, i.transform.position) > 2.6) {
				countDistance++;
			}
		}

		if (countDistance == eleArr.Length) {
			int rand = Random.Range (0, elementsArr.Length - 1);
			var new_element = Instantiate (elementsArr[rand], entry.position, Quaternion.identity) as GameObject;
			new_element.transform.parent = element.transform;
			new_element.transform.position = entry.position;
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
	
	}
}
