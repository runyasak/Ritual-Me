using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using PlayFab;
using PlayFab.ClientModels;

public class SceneController : MonoBehaviour {
	
	private SocketIOComponent socketIO;

	public PlayerData playerData;

	private InputField input;

	private string player_name;
	private bool isNewPlayer;
	public string PlayFabId;

	private JSONObject j = new JSONObject(JSONObject.Type.OBJECT);

	void Awake (){

		PlayFabSettings.TitleId = "9C78";
		GameObject go = GameObject.Find("SocketIO");
		socketIO = go.GetComponent<SocketIOComponent>();

		GameObject playerObj = GameObject.Find ("PlayerData");
		playerData = playerObj.GetComponent<PlayerData> ();

		DontDestroyOnLoad (socketIO);
	}

	void Start (){
		socketIO.On("CLICK_PLAY", changeToStartScene);
//		StartCoroutine("CalltoServer");
		GameObject inputGO = GameObject.Find("InputField");
		input = inputGO.GetComponent<InputField> ();

		isNewPlayer = false;

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
				isNewPlayer = true;
				StartCoroutine (Ready());
			}
			else
			{
				Debug.Log("(existing account)");
				GetUserData ();
				StartCoroutine (Ready());
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

	void GetUserData() {
		GetUserDataRequest request = new GetUserDataRequest() {
			PlayFabId = PlayFabId,
			Keys = null
		};

		PlayFabClientAPI.GetUserData(request,(result) => {
			Debug.Log("Got user data:");
			if ((result.Data == null) || (result.Data.Count == 0)) {
				Debug.Log("No user data available");
			}
			else {
//				foreach (var item in result.Data) {
//					Debug.Log("    " + item.Key + " == " + item.Value.Value);
//				}
//
//				Debug.Log("    " + result.Data["win"].Value);
				player_name = result.Data["name"].Value;
			}
		}, (error) => {
			Debug.Log("Got error retrieving user data:");
			Debug.Log(error.ErrorMessage);
		});
	}



	public void onClick(){
		
		if (input.text != "") {
			Debug.Log ("Player: " + input.text);
			Login (PlayFabSettings.TitleId);
		} else {
			input.text = "Please input your name again";
		}
	}

	IEnumerator Ready(){

		yield return new WaitForSeconds(3f);
		if(isNewPlayer){
			j.AddField ("name", input.text);
		} else {
			Debug.Log (player_name);
			j.AddField ("name", player_name);
		}
		socketIO.Emit ("USER_READY", j);

	}

	public void changeToStartScene (SocketIOEvent e) {
		Application.LoadLevel (1);
	}

	//Manual Play
	IEnumerator ChangeScene(){

		yield return new WaitForSeconds(3f);

		Application.LoadLevel (1);

	}
}
