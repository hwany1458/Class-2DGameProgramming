using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    // variables
    public TMP_InputField inputUserID;
    public TMP_InputField inputPassword;
    public TMP_InputField inputEmail;
    public TMP_Text displayMessage;
    
    private string username;
    private string password;
    private string email;
    private string playfabId;

    private GameObject loginInfo;
    private string emptyString = "";

    // Start is called before the first frame update
    void Start()
    {
        PlayFabSettings.TitleId = "19A94D";
        loginInfo = GameObject.Find("LoginInfo");
        displayMessage.text = emptyString;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // user defined methods
    public void UsernameValueChanged() { username = inputUserID.text.ToString(); }
    public void PasswordValueChanged() { password = inputPassword.text.ToString(); }
    public void EmailValueChanged() { email = inputEmail.text.ToString(); }

    //----- Login
    public void Login()
    {
        var request = new LoginWithPlayFabRequest { Username = username, Password = password };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }
    private void OnLoginSuccess(LoginResult result)
    {
        displayMessage.text = "Login successfully";
        playfabId = result.PlayFabId;

        // Set User (Login) Info
        SetLoginInfo(username, password, email, playfabId);

        // clear input field
        ClearInputField();
        StartGame();
    }
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning(error.GenerateErrorReport());
        displayMessage.text = error.GenerateErrorReport();
    }

    //------ Register
    public void Register()
    {
        var request = new RegisterPlayFabUserRequest { Username = username, Password = password, Email = email };
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);
    }
    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        displayMessage.text = "Signup successfully";

        // clear input field
        ClearInputField();
    }
    private void RegisterFailure(PlayFabError error)
    {
        Debug.LogWarning(error.GenerateErrorReport());
        displayMessage.text = error.GenerateErrorReport();
    }

    // ---- Start game
    private void StartGame() 
    { 
        Debug.Log("Now, start the game, enjoy it");
        //SceneManager.LoadScene("MainMenuScene");
        //SceneManager.LoadScene("TestDataScene");
    }

    private void ClearInputField()
    {
        inputUserID.text = emptyString;
        inputPassword.text = emptyString;
        inputEmail.text = emptyString;
    }

    public void SetLoginInfo(string u, string p, string e)
    {
        LoginInfo targetScript = loginInfo.GetComponent<LoginInfo>();
        targetScript.username = u;
        targetScript.password = p;
        targetScript.email = e;
        //targetScript.playFabId = myPlayFabId;
    }

    public void SetLoginInfo(string u, string p, string e, string i)
    {
        LoginInfo targetScript = loginInfo.GetComponent<LoginInfo>();
        targetScript.username = u;
        targetScript.password = p;
        targetScript.email = e;
        targetScript.playFabId = playfabId;
    }

    public void PressCancelButton()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
                {"Ancestor", "Arthur"},
                {"Successor", "Fred"}
            }
        },
        result => {
            Debug.Log("Successfully updated user data");
            displayMessage.text = "Successfully updated user data";
        },
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
            displayMessage.text = error.GenerateErrorReport();
        });
    }

    public void GetUserData(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, 
        result => {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("Ancestor"))
            {
                Debug.Log("No Ancestor");
                displayMessage.text = "No Ancestor";
            }
            else
            {
                Debug.Log("Ancestor: " + result.Data["Ancestor"].Value);
                displayMessage.text = "Ancestor: " + result.Data["Ancestor"].Value;
            }
        }, 
        error => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
            displayMessage.text = error.GenerateErrorReport();
        });
    }

}
