﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GaugeController : MonoBehaviour {

	private Vector3 gaugePosition;
	public GameObject element1;
	private GameObject[] elementArr;
	private GameObject[] arr;
	private float timeForCreate;
<<<<<<< HEAD
	public int maxElement;
=======
>>>>>>> master




	// Use this for initialization
	void Start () {
		gaugePosition = this.transform.position;
		elementArr = new GameObject[] {
			element1
		};

	}

	// Update is called once per frame
	void Update () {
		timeForCreate += Random.Range (0,5);
		CreateElement();
	}

	void CreateElement(){
		float randomPosition = Random.Range (-7f, 7f);
		int distanceCheck = 0;
		arr = GameObject.FindGameObjectsWithTag ("Element");

		foreach (GameObject i in arr){
<<<<<<< HEAD
			if (Mathf.Abs (randomPosition - i.transform.position.x) > 0.75f) {
=======
			if (Mathf.Abs (randomPosition - i.transform.position.x) > 1.5f) {
>>>>>>> master
				distanceCheck++;
			}
		}

<<<<<<< HEAD
		if ((distanceCheck == arr.Length || arr.Length == 0) && timeForCreate > 200 && arr.Length < maxElement) {
=======
		if ((distanceCheck == arr.Length || arr.Length == 0) && timeForCreate > 100) {
>>>>>>> master
			GameObject instantElement = Instantiate (element1, new Vector3(randomPosition, this.transform.position.y, -1)
				, Quaternion.identity) as GameObject;
			timeForCreate = 0;
		}
		distanceCheck = 0;
	}
	
}