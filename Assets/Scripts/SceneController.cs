using UnityEngine;
using System.Collections;
using SocketIO;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class SceneController : MonoBehaviour {
	
	private SocketIOComponent socketIO;

	private InputField input;

	public string PlayFabId;

	void Awake (){

		PlayFabSettings.TitleId = "9C78";

		GameObject go = GameObject.Find("SocketIO");
		socketIO = go.GetComponent<SocketIOComponent>();
		DontDestroyOnLoad (socketIO);
		Debug.Log(SystemInfo.graphicsDeviceID);
	}

	void Start (){
		socketIO.On("CLICK_PLAY", changeToStartScene);
//		StartCoroutine("CalltoServer");
		GameObject inputGO = GameObject.Find("InputField");
		input = inputGO.GetComponent<InputField> ();
		Login (PlayFabSettings.TitleId);
	}

	void Login(string titleId) {
		LoginWithCustomIDRequest request = new LoginWithCustomIDRequest() {
			TitleId = titleId,
			CreateAccount = false,
			CustomId = SystemInfo.deviceUniqueIdentifier
		};

		PlayFabClientAPI.LoginWithCustomID(request, (result) => {
			PlayFabId = result.PlayFabId;
			Debug.Log("Got PlayFabID: " + PlayFabId);

			if(result.NewlyCreated)
			{
				Debug.Log("(new account)");
			}
			else
			{
				Debug.Log("(existing account)");
			}
		},
			(error) => {
				Debug.Log("Error logging in player with custom ID:");
				Debug.Log(error.ErrorMessage);
			});
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
