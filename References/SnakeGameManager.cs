using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SnakeGameManager : MonoBehaviour
{

    // UI
    GameObject panelOver;
    //Text txtCoin;
    Text txtTime;
    //Text txtSpeed;
    //Text txtScore;

    float startTime;

    /*
     * 모바일에서는 2인용을 제공하지 않으므로 삭제해도 무방
    // Joystick
    GameObject panelStick;
    Joystick stick;
    bool isMobile;
    */

    // 1분마다 속도가 빨라지는 것을 받아서, Snake 스크립트로 넘겨주기 위해
    public int setTimeM;

    //------------------- Methods

    private void Awake()
    {
        InitGame();
        startTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetTime();
    }

    //-------------------- user defined function
    // 게임 초기화
    void InitGame()
    {

        // UI 위젯
        panelOver = GameObject.Find("PanelOver");
        panelOver.SetActive(false);

        txtTime = GameObject.Find("TxtTime").GetComponent<Text>();

        //txtCoin = GameObject.Find("TxtCoin").GetComponent<Text>();
        //txtSpeed = GameObject.Find("TxtSpeed").GetComponent<Text>();
        //txtScore = GameObject.Find("TxtScore").GetComponent<Text>();

        /*
         * 모바일에서는 2인용을 제공하지 않으므로 삭제해도 무방
        // Mobile Device인가?
        isMobile = Application.platform == RuntimePlatform.Android ||
                   Application.platform == RuntimePlatform.IPhonePlayer;
        // For Testing
        //isMobile = true;

        panelStick = GameObject.Find("PanelStick");
        panelStick.SetActive(isMobile);
        stick = panelStick.transform.GetChild(0).GetComponent<Joystick>();
        */
    }

    // UI Time
    void SetTime()
    {
        float span = Time.time - startTime;
        int h = Mathf.FloorToInt(span / 3600);
        int m = Mathf.FloorToInt(span / 60 % 60);
        float s = span % 60;

        setTimeM = m;

        /*
        txtCoin.text = coinCnt.ToString("Count : 0");
        txtSpeed.text = speedMove.ToString("Speed : 0.0");
        txtScore.text = "Score : " + score.ToString();

        newMin = m;

        // Time 과 Speed Setting(time으로)에 따라 이동 속도(speedMove)를 증가
        if ((oldMin != newMin) && (isSetSpeed == 2))
        {
            speedMove += 0.5f;
            oldMin = newMin;
        }
        */

        txtTime.text = string.Format("Time: {0:0}:{1:0}:{2:00.0}", h, m, s);
    }

    // Button Click
    public void OnButtonClick(Button button)
    {
        switch (button.name)
        {
            case "BtnYes":
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "BtnNo":
                //Application.Quit();
                SceneManager.LoadScene("MainMenuScene");
                break;
        }
    }

    public void GameOver(bool isDead)
    {
        panelOver.SetActive(isDead);
    }

}
