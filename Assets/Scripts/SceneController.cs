using UnityEngine;
using System.Collections;
using SocketIO;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
	
	private SocketIOComponent socketIO;

	private InputField input;

	void Awake (){
		GameObject go = GameObject.Find("SocketIO");
		socketIO = go.GetComponent<SocketIOComponent>();
		DontDestroyOnLoad (socketIO);
	}

	void Start (){
		socketIO.On("CLICK_PLAY", changeToStartScene);
//		StartCoroutine("CalltoServer");
		GameObject inputGO = GameObject.Find("InputField");
		input = inputGO.GetComponent<InputField> ();
	}

	//Call server
	private IEnumerator CalltoServer(){

		yield return new WaitForSeconds(1f);

		Debug.Log("Send message to the server");
		socketIO.Emit("USER_CONNECT");

	}

	public void onClick(){
		//socketIO.Emit ("START_GAME");
		if (input.text != "") {
			Debug.Log (input.text);
			Application.LoadLevel (1);
		} else {
			input.text = "Please input your name again";
		}

	}

	public void changeToStartScene (SocketIOEvent e) {
		Application.LoadLevel (1);
	}
}
