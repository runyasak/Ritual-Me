using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	}

	void Start (){
		socketIO.On("CLICK_PLAY", changeToStartScene);
//		StartCoroutine("CalltoServer");
		GameObject inputGO = GameObject.Find("InputField");
		input = inputGO.GetComponent<InputField> ();

		Dictionary<string, string> data = new Dictionary<string, string>();
		data["device_id"] = SystemInfo.deviceUniqueIdentifier + "";
		socketIO.Emit ("CHECK_DEVICE", new JSONObject (data));
	}
		
	//Playfab login
	void Login(string titleId) {
		LoginWithCustomIDRequest request = new LoginWithCustomIDRequest() {
			TitleId = titleId,
			CreateAccount = true,
			CustomId = SystemInfo.deviceUniqueIdentifier
		};

		PlayFabClientAPI.LoginWithCustomID(request, (result) => {
			PlayFabId = result.PlayFabId;
			Debug.Log("Got PlayFabID: " + PlayFabId);

			if(result.NewlyCreated)
			{
				Debug.Log("(new account)");
				registerNewAccount (PlayFabSettings.TitleId);
				SetUserData();
				StartCoroutine (ChangeScene());
			}
			else
			{
				Debug.Log("(existing account)");
				StartCoroutine (ChangeScene());
			}
		},
			(error) => {
				Debug.Log("Error logging in player with custom ID:");
				Debug.Log(error.ErrorMessage);
			});
	}

	void registerNewAccount (string titleId){
		Debug.Log ("Register new account");
		LoginWithCustomIDRequest request = new LoginWithCustomIDRequest() {
			TitleId = titleId,
			CustomId = SystemInfo.deviceUniqueIdentifier
		};
	}

	void SetUserData() {
		
		UpdateUserDataRequest request = new UpdateUserDataRequest() {
			Data = new Dictionary<string, string>(){
				{"name", input.text},
				{"win", "0"},
				{"lose", "0"}
			}
		};

		PlayFabClientAPI.UpdateUserData(request, (result) =>
			{
				Debug.Log("Successfully updated user data");
			}, (error) =>
			{
				Debug.Log("Got error setting user data Ancestor to Arthur");
				Debug.Log(error.ErrorDetails);
			});
	}

	public void onClick(){
		
		if (input.text != "") {
			Debug.Log ("Player: " + input.text);
			JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
			j.AddField ("name", input.text);

			Login (PlayFabSettings.TitleId);

			socketIO.Emit ("USER_READY", j);
		} else {
			input.text = "Please input your name again";
		}
	}

	IEnumerator ChangeScene(){

		yield return new WaitForSeconds(1f);

		Application.LoadLevel (1);

	}

	public void changeToStartScene (SocketIOEvent e) {
		Application.LoadLevel (1);
	}
}
