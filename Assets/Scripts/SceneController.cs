using UnityEngine;
using System.Collections;
using SocketIO;

public class SceneController : MonoBehaviour {
	
	private SocketIOComponent socketIO;

	void Start (){
		GameObject go = GameObject.Find("SocketIO");
		socketIO = go.GetComponent<SocketIOComponent>();
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
		socketIO.Emit ("START_GAME");
	}

	public void changeToStartScene (SocketIOEvent e) {
		Application.LoadLevel (1);
	}
}
