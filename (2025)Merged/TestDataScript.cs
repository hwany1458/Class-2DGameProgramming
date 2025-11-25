using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataScript : MonoBehaviour
{
    private string username;
    private string password;
    private string email;

    private GameObject loginInfo;
    private string emptyString = "";

    private int savedSnakeCoin;
    private int currentSnakeCoin;
    private int savingSnakeCoin;

    private int savedSnakeSpanTime;
    private int currentSnakeSpanTime;
    private string savedSnakeTime;
    private string currentSnakeTime;

    // Start is called before the first frame update
    void Start()
    {
        PlayFabSettings.TitleId = "19A94D";
        loginInfo = GameObject.Find("LoginInfo");

        // ---------
        currentSnakeCoin = Random.Range(10, 20);
        currentSnakeSpanTime = Random.Range(100, 200);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingSnakeCoin(string myPlayFabId)
    {
        GetSnakeCoin(myPlayFabId);
        int snakeCoinValue = CheckSnakeCoin();
        SetSnakeCoin(snakeCoinValue.ToString());
    }

    public void GettingSnakeCoin()
    {

    }

    private int CheckSnakeCoin()
    {
        if (currentSnakeCoin > savedSnakeCoin) { 
            savingSnakeCoin = currentSnakeCoin;
            return currentSnakeCoin;
        }
        else { 
            savingSnakeCoin = savedSnakeCoin;
            return savedSnakeCoin;
        }
        //return savingSnakeCoin;
    }

    private int GetSnakeCoin(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("SnakeCoin"))
            { Debug.Log("No SnakeCoin"); }
            else
            {
                savedSnakeCoin = int.Parse(result.Data["SnakeCoin"].Value);
                Debug.Log("SnakeCoin: " + result.Data["SnakeCoin"].Value);
            }
        }, (error) => {
            savedSnakeCoin = -1;
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });

        return savedSnakeCoin;
    }


    public void SetSnakeCoin(string savingSnakeCoinValue)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
                {"SnakeCoin", savingSnakeCoinValue},
            }
        },
        result => Debug.Log("Successfully updated user data, SnakeCoin"),
        error => {
            Debug.Log("Got error setting user data (Snake Game - Coin)");
            Debug.Log(error.GenerateErrorReport());
        });
    }


    //----------
    public void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
                {"SnakeCoin", "100"},
                {"SnakeTime", "0:0:1.3"}
            }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data (Snake Game)");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void GetUserData(string myPlayFabId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("SnakeCoin")) Debug.Log("No SnakeCoin");
            else Debug.Log("SnakeCoin: " + result.Data["SnakeCoin"].Value);
            if (result.Data == null || !result.Data.ContainsKey("SnakeTime")) Debug.Log("No SnakeTime");
            else Debug.Log("SnakeTime: " + result.Data["SnakeTime"].Value);
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
