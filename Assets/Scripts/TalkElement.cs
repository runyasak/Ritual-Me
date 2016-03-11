using UnityEngine;
using System.Collections;

public class TalkElement : MonoBehaviour {

	public GameObject talkLeft;
	public GameObject talkRight;
	public GameObject center;
	public float cLenght;

	public float getLeft(){
		return talkLeft.transform.position.x;
	} 

	public float getRight(){
		return talkRight.transform.position.x;
	}
}
