using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Snake2Person : MonoBehaviour
{
    public float speedMove = 2.5f; // 이동속도
    float speedRot = 150f;  // 회전속도

    bool isDead = false;  // 사망 여부 체크

    List<Transform> tails = new List<Transform>();

    Transform coin;
    Transform plusItem;
    Transform minusItem;

    Text txtCoin;
    Text txtSpeed;
    Text txtScore;

    /*  SnakeGameManager로 이동
    // UI
    GameObject panelOver;
    Text txtTime;

    float startTime;

    // Joystick (모바일에서는 2인용을 제공하지 않으므로 삭제해도 무방)
    GameObject panelStick;
    bool isMobile;
    Joystick stick;
    */


    int coinCnt = 0;

    // Game Controll
    int isSetSpeed = 1;  // 1 (Coin), 2 (Time)
    int oldMin = 0, newMin = 0;
    int score = 0;

    AudioSource[] soundEffect;  // 0 (Get item), 1 (Game over)

    // 2인용으로
    public GameObject snakeGameManager;
    public int playerNum = 1;  // 뱀 번호
    string movAxisName;  // 이동축의 이름 저장
    string rotAxisName;  // 회전축의 이름 저장
    string txtCoinName;
    string txtSpeedName;
    string txtScoreName;

    // ------------------ methods
    private void Awake() 
    { 
        InitGame();
        // SnakeGameManager로 이동
        //startTime = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) 
        { 
            MoveHead();
            MoveTail();
            SetTime();
        }

    }

    //-------------------- user defined function
    // 게임 초기화
    void InitGame()
    {
        // 2인용으로
        snakeGameManager = GameObject.Find("GameManager");
        movAxisName = "Vertical" + playerNum;
        rotAxisName = "Horizontal" + playerNum;

        coin = GameObject.Find("Coin").transform;
        plusItem = GameObject.Find("Hexgon").transform;
        minusItem = GameObject.Find("Cuboid").transform;

        txtCoinName = "TxtCoin" + playerNum;
        txtSpeedName = "TxtSpeed" + playerNum;
        txtScoreName = "TxtScore" + playerNum;
        txtCoin = GameObject.Find(txtCoinName).GetComponent<Text>();
        txtSpeed = GameObject.Find(txtSpeedName).GetComponent<Text>();
        txtScore = GameObject.Find(txtScoreName).GetComponent<Text>();

        /* SnakeGameManager 로 이동
        // UI  위젯
        panelOver = GameObject.Find("PanelOver");
        panelOver.SetActive(false);

        txtTime = GameObject.Find("TxtTime").GetComponent<Text>();

        //----- 모바일에서는 2인용을 제공하지 않으므로 삭제해도 무방
        // Mobile Device인가?
        isMobile = Application.platform == RuntimePlatform.Android ||
                   Application.platform == RuntimePlatform.IPhonePlayer;
        // For Testing
        //isMobile = true;

        panelStick = GameObject.Find("PanelStick");
        panelStick.SetActive(isMobile);
        stick = panelStick.transform.GetChild(0).GetComponent<Joystick>();
        */

        // Audio sound 가져오기
        // 현재 뱀에 2개의 AudioSound가 걸려 있음 - 배열로 선언하고 GetComponents()로 가져옴
        soundEffect = GetComponents<AudioSource>();

    }

    void MoveHead()
    {
        // 이동
        float amount = speedMove * Time.deltaTime;
        transform.Translate(Vector3.forward * amount);

        // 회전
        //amount = Input.GetAxis("Horizontal") * speedRot;
        /* 모바일에서는 2인용을 제공하지 않으므로 삭제해도 무방
        if (!isMobile)
        {
            //amount = Input.GetAxis("Horizontal") * speedRot;
            // 2인용으로
            amount = Input.GetAxis(rotAxisName) * speedRot;
        }
        else
        {
            amount = stick.Horizontal() * speedRot;
        }
        */

        amount = Input.GetAxis(rotAxisName) * speedRot;
        transform.Rotate(Vector3.up * amount * Time.deltaTime);
    }

    // 충돌 판정와 처리
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("충돌물체=" + collision.gameObject.name);
        switch (collision.gameObject.tag)
        {
            // 뱀(Snake)과 충돌체가 무엇인가에 따라
            /*
            case "Coin":
                GetCoinAndMoveIt();
                //MoveCoin();
                AddTail();
                break;
            case "ItemPlus":
                GetPlusItemAndMoveIt();
                break;
            case "ItemMinus":
                GetMinusItemAndMoveIt();
                break;
            */
            case "Tail":
                //break;
            case "Wall":
                isDead = true;
                soundEffect[1].Play();  // 게임오버 사운드

                //panelOver.SetActive(isDead);  // 게임오버되어 화면 띄움
                // 2인용으로 변경
                snakeGameManager.SendMessage("GameOver", isDead);
                break;
        }
    }

    // OnCollisionEnter() 에서 OnTriggerEnter() 로 변경
    // 충돌 판정 및 처리 (IsTrigger checked)
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("트리거 충돌물체 :" + other.tag);
        switch(other.tag)
        {
            case "Coin":
                GetCoinAndMoveIt();
                //MoveCoin();
                AddTail();
                break;
            case "ItemPlus":
                GetPlusItemAndMoveIt();
                AddTail();
                break;
            case "ItemMinus":
                GetMinusItemAndMoveIt();
                break;

            /*
            case "Tail":
            //break;
            case "Wall":
                isDead = true;
                soundEffect[1].Play();  // 게임오버 사운드
                panelOver.SetActive(isDead);  // 게임오버되어 화면 띄움
                break;
            */
        }
    }

    // Coin 이동 (MoveCoin)
    //void MoveCoin()
    Vector3 MoveCoin()
    {
        // 코인 카운트를 증가
        coinCnt++;
        soundEffect[0].Play();

        // Coin count와 Speed Setting(coin으로)에 따라 이동 속도(speedMove)를 증가
        if (((coinCnt % 3) == 0) && (isSetSpeed == 1))
        {
            speedMove += 0.5f;
        }

        // Coin을 이동할 범위 - 벽의 안쪽
        float x = Random.Range(-9f, 9f);
        float z = Random.Range(-4f, 4f);

        //coin.position = new Vector3(x, 0, z);
        return new Vector3(x, 0, z);
    }

    void GetCoinAndMoveIt()
    {
        score += 5;

        Vector3 location = MoveCoin();
        coin.position = location;
    }

    void GetPlusItemAndMoveIt()
    {
        score += 20;

        Vector3 location = MoveCoin();
        plusItem.position = location;
    }

    void GetMinusItemAndMoveIt()
    {
        score -= 10;
        Vector3 location = MoveCoin();
        minusItem.position = location;
    }

    // 꼬리 추가
    void AddTail()
    {
        // (붙일) 꼬리 오브젝트 생성, 위치를 pos로 받아냄
        GameObject tail = Instantiate(Resources.Load("Tail")) as GameObject;
        Vector3 pos = transform.position;

        // 꼬리의 개수 구하기
        int cnt = tails.Count;

        if (cnt == 0) 
        { 
            tail.tag = "Untagged"; 
        }
        else 
        { 
            pos = tails[cnt - 1].position; 
        }

        tail.transform.position = pos;

        // 꼬리 색
        Color[] colors = { new Color(0, 0.5f, 0, 1), new Color(0, 0.5f, 1, 1) };
        int n = cnt / 3 % 2;
        tail.GetComponent<Renderer>().material.color = colors[n];

        tails.Add(tail.transform);
    }

    // 꼬리 이동 및 회전
    void MoveTail()
    {
        Transform target = transform;
        foreach (Transform tail in tails)
        {
            Vector3 pos = target.position;
            Quaternion rot = target.rotation;

            tail.position = Vector3.Lerp(tail.position, pos, (speedMove+1) * Time.deltaTime);
            tail.rotation = Quaternion.Lerp(tail.rotation, rot, (speedMove + 1) * Time.deltaTime);

            target = tail;
        }
    }

    // UI Time
    void SetTime()
    {
        txtCoin.text = coinCnt.ToString("Count : 0");
        txtSpeed.text = speedMove.ToString("Speed : 0.0");
        txtScore.text = "Score : " + score.ToString();

        /* 2인용으로 이동
        float span = Time.time - startTime;
        int h = Mathf.FloorToInt(span / 3600);
        int m = Mathf.FloorToInt(span / 60 % 60);
        float s = span % 60;
        */

        //newMin = m;
        // 2인용으로 변경
        newMin = snakeGameManager.GetComponent<SnakeGameManager>().setTimeM;

        // Time 과 Speed Setting(time으로)에 따라 이동 속도(speedMove)를 증가
        if ((oldMin != newMin) && (isSetSpeed == 2))
        {
            speedMove += 0.5f;
            oldMin = newMin;
        }

        // 2인용으로 이동
        //txtTime.text = string.Format("Time: {0:0}:{1:0}:{2:00.0}", h, m, s);
    }

    /* 2인용으로 이동
    // Button Click
    public void OnButtonClick(Button button)
    {
        switch (button.name)
        {
            case "BtnYes" :
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
                break;
            case "BtnNo":
                //Application.Quit();
                SceneManager.LoadScene("MainMenuScene");
                break;
        }
    }
    */
}
