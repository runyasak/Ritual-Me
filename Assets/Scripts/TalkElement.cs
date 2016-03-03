using UnityEngine;
using System.Collections;

public class TalkElement : MonoBehaviour {

	public GameObject talkLeft;
	public GameObject talkRight;
	public GameObject Center;
	public float cLenght;

	// Use this for initialization
	void Start () {
		Center.transform.localScale = new Vector3 (cLenght, Center.transform.localScale.y, Center.transform.localScale.z);
		talkRight.transform.position = new Vector3 (cLenght, transform.localPosition.y, talkRight.transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
