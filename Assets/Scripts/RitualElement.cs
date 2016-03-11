using UnityEngine;
using System.Collections;

public class RitualElement : MonoBehaviour {

	private float rPosition;
	private int elementID;

	public RitualElement(int elementID, float rPosition){
		this.rPosition = rPosition;
		this.elementID = elementID;
	}

	void Start () {
		rPosition = 0;
	}

	public float getRPosition(){
		return rPosition;
	}

	public int getElementID(){
		return elementID;
	}
}
