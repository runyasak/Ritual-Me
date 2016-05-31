using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class wizard2Controller : MonoBehaviour {


	public float maxHR = 100;
	public float curHR = 0;
	public GameObject hpText;
	public GameObject hrBar;
	public TextMesh curHp_maxHp;

	void Awake(){

	}

	// Use this for initialization
	void Start () {
		hpText.SetActive (true);
		curHp_maxHp.text = "";
	}
		

	// Update is called once per frame
	void Update () {
		curHp_maxHp.text = curHR+" /"+maxHR;

		if (curHR< 0) {
			curHR = 0;
		}
		if (curHR > maxHR) {
			curHR = maxHR;		
		}
		hrBar.transform.localScale = new Vector3 (curHR / maxHR, hrBar.transform.localScale.y, hrBar.transform.localScale.z);
	}
}
