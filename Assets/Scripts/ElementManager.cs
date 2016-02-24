using UnityEngine;
using System.Collections;

public class ElementManager : MonoBehaviour {

	public Transform entry;

	public GameObject element;
	public GameObject cat_element;
	public static ElementManager instance;

	void Awake () {
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
		
	public void spawnElement(){
		Debug.Log ("kuy");
		var new_element = Instantiate (cat_element, entry.position, Quaternion.identity) as GameObject;
		new_element.transform.parent = element.transform;
		new_element.transform.position = entry.position;

//		var instantElement = Instantiate (allElement[choice], new Vector3(spawnLocate, this.transform.position.y, -1), Quaternion.identity) as GameObject;
//		var instantElement_script = instantElement.GetComponent<ElementController>();
//		instantElement_script.isMove = true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
