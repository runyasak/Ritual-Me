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

	public void onClick(){
		//socketIO.Emit ("START_GAME");
		if (input.text != "") {
			Debug.Log ("Player: " + input.text);
//			Application.LoadLevel (1);
			JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
			j.AddField ("name", input.text);
			socketIO.Emit ("USER_READY", j);
		} else {
			input.text = "Please input your name again";
		}
	}

	public void changeToStartScene (SocketIOEvent e) {
		Application.LoadLevel (1);
	}
}
