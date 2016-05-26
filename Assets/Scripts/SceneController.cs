using UnityEngine;
using System.Collections;
using SocketIO;

public class SceneController : MonoBehaviour {
	
	private SocketIOComponent socketIO;

	void Awake (){
		GameObject go = GameObject.Find("SocketIO");
		socketIO = go.GetComponent<SocketIOComponent>();
		DontDestroyOnLoad (socketIO);
	}

	void Start (){
		socketIO.On("CLICK_PLAY", changeToStartScene);
//		StartCoroutine("CalltoServer");
	}

	//Call server
	private IEnumerator CalltoServer(){

		yield return new WaitForSeconds(1f);

		Debug.Log("Send message to the server");
		socketIO.Emit("USER_CONNECT");

	}

	public void onClick(){
		socketIO.Emit ("USER_READY");
	}

	public void changeToStartScene (SocketIOEvent e) {
		Application.LoadLevel (1);
	}
}
