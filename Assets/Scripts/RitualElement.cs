using UnityEngine;
using System.Collections;

public class RitualElement : MonoBehaviour {

	private float rPosition;
	private int elementID;
	// Use this for initialization

	public RitualElement(int elementID, float rPosition){
		this.rPosition = rPosition;
		this.elementID = elementID;
	}
	void Start () {
		rPosition = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float getRPosition(){
		return rPosition;
	}

	public int getElementID(){
		return elementID;
	}
}
