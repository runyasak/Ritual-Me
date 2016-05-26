using UnityEngine;
using System.Collections;
using System;

// Use this for initialization
[Serializable]
public class ArrayJSON {

	//TIMER
	public int timer;
	//ATK
	public float[] atk_arr;
	//INT
	public float[] int_arr;
	//WIS
	public int wizard_index;
	public int heal_point;
	//WIZARD
	public int[] hp_wizard;

	public static ArrayJSON createFromJson(string json) {

		ArrayJSON info = JsonUtility.FromJson<ArrayJSON>(json);
		return info;
	}
}