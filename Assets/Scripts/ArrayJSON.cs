using UnityEngine;
using System.Collections;
using System;

// Use this for initialization
[Serializable]
public class ArrayJSON {
	public float[] atk_arr;

	public static ArrayJSON createFromJson(string json) {

		ArrayJSON info = JsonUtility.FromJson<ArrayJSON>(json);
		return info;
	}
}