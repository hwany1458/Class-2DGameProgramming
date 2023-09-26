using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // Legacy Text
using TMPro;  // Text Mesh Pro

public class GameManager : MonoBehaviour
{
    // 변수선언
    // Unity에서 권장 : public 보다는 [SerializeField] private
    // 접근자(public, private) 용도에 맞게 적절하게 선언하도록
    [SerializeField] private RawImage imgYou;   // 사용자 가위바위보 이미지 표시용
    [SerializeField] private RawImage imgCom;  // 컴퓨터 이미지 표시용
    //[SerializeField] private Image imgCom;     // 컴퓨터 이미지 표시용
    // 이미지를 만들 때, 
    //   Image 타입 -- Image 선언
    //   Raw Image 타입 -- RawImage 선언

    [SerializeField] private TMP_Text txtYou;    // 사용자 승리한 회수 표시용
    [SerializeField] private TMP_Text txtCom;   // 컴퓨터 승리한 회수 표시용
    [SerializeField] private TMP_Text txtResult; // 판정 결과 표시용

    int cntYou = 0;   // 사용자 승리 회수
    int cntCom = 0;  // 컴퓨터 승리 회수

    //--------------- 메소드

    void Start() {     }

    private void Awake()
    {
        InitGame();
    }


    void Update()
    {
        // ESC key로 게임 종료
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // 나중에 게임들을 모아서(?) 하나의 프로젝트로 묶을때 수정 필요
            Application.Quit();
        }
    }

    // 버튼 클릭 (가위 바위 보 이미지를 클릭했을 때)
    // 다른 오브젝트에서 여기의 메소드를 호출하기 때문에 public 으로 선언해야 함
    public void OnButtonClick(GameObject button)
    {
        Debug.Log(button.name);
        // 클릭한 버튼 번호를 뽑아온다 (이름에서 8번째 문자에서 1개 문자를)
        int you = int.Parse(button.name.Substring(7, 1));  // 1,2,3
        
        // 교재에서 컴퓨터가 뽑는 램덤숫자를 CheckResult()에서 뽑지만
        // (여기서는) 내가 뽑을 때, 컴퓨터도 같이 뽑음
        int com = Random.Range(1, 4);  // 1,2,3

        // 이겼다 졌다 비겼다를 판단 (교재에서)
        // CheckResult(you, com);

        // 수정 (승무패 판정만 하고, 결과 뿌려주는 함수에서 한꺼번에 화면 표시를 모두 처리)
        // 돌아오는 res가 이기면 1, 비기면 0, 지면 -1
        int res = CheckResult1(you, com);
        
        string str = "";
        switch(res)
        {
            case 1:  // 이긴 경우
                cntYou++;
                //txtResult.text = "Win";
                str = "Win";
                break;
            case 0:  // 비긴 경우
                //txtResult.text = "Draw";
                str = "Draw";
                break;
            case -1:  // 진 경우
                cntCom++;
                //txtResult.text = "Lose";
                str = "Lose";
                break;
            default:
                break;
        }
        Debug.Log("Check is " + res);

        // 게임 결과를 UI에 반영 (교재대로)
        //SetResult(you, com);
        // 수정 (경기 결과를 한꺼번에 표시하도록 수정함)
        SetResult(you, com, str);
    }

    // Pointer Exit
    public void OnMouseExit(GameObject button)
    {
        Animator anim = button.GetComponent<Animator>();
        anim.Play("Normal");
    }

    // -------------- User-defined method
    
    // 게임 초기화 함수
    void InitGame()
    {
        // (1방법) 교재에서 처럼, GameObject.Find() 처리해도 됨
        /*  (대신 component 타입을 적절하게 맞출 것)
          imgYou = GameObject.Find(“ImgYou”).GetComponent<Image>();
          imgCom = GameObject.Find(“ImgCom”).GetComponent<Image>();

          txtYou = GameObject.Find(“TxtYou”).GetComponent<Text>();
          txtCom = GameObject.Find(“TxtCom”).GetComponent<Text>();
          txtResult = GameObject.Find(“TxtResult”).GetComponent<Text>();

         * */
        // (2방법) 변수를 public 선언해서, 드래그로 할당
    }

    // 이겼다 졌다를 판별하는 함수 (승패 판정)
    void CheckResult(int you, int com)
    {
        /*
        //--------------------------------
        // 내가 가위냈을 때 (1)
        if (you == 1) {
            if (com == 1) { // 컴이 가위 (1)
                // 비김 (무)
            }  
            else if (com == 2) { // 컴이 바위 (2)
                // 졌네 (패)
            }
            else {  // 컴이 보(3)
                // 이겼네 (승)
            }  
        }

        // 내가 바위냈을 때 (2)
        else if (you == 2) {
            if (com == 1)
            { // 컴이 가위 (1)
            }
            else if (com == 2)
            { // 컴이 바위 (2)
            }
            else
            {  // 컴이 보(3)
            }

        }

        // 내가 보냈을 때 (3)
        else {
            if (com == 1)
            { // 컴이 가위 (1)
            }
            else if (com == 2)
            { // 컴이 바위 (2)
            }
            else
            {  // 컴이 보(3)
            }

        }
        //--------------------------------
        */

    }

    // 승패 판정만 판별하여 반환
    int CheckResult1(int you, int com)
    {
        int res;  // 반환값 (이기면 1, 비기면 0, 지면 -1)

        int k = you - com;
        Debug.Log("CheckResult1() method is called ...[" + you + "],[" + com + "]");

        /*
        if (k == 0) 
        { txtResult.text = "Draw"; }
        else if (k == 1 || k == -2) 
        { txtResult.text = "Win"; cntYou++; }
        else   // k=2 또는 k=-1
        { txtResult.text = "Lose"; cntCom++; }
         * 아래와 같이 수정함
         */

        if (k == 0) { res = 0; }
        else if ((k == 1) || (k == -2)) { res = 1; }
        else { res = -1; }

        return res;
    }

    // 가위바위보 결과를 화면에 표시
    // 게임 결과를 UI에 반영 -- (교재 대로)
    void SetResult(int you, int com)
    {
        Debug.Log("SetResult() method is called ...[" + you + "],[" + com + "]");

        // Image 타입으로 선언된 경우
        //imgYou.sprite = Resources.Load("img_" + you, typeof(Sprite)) as Sprite;
        // Raw Image 타입으로 선언된 경우
        imgYou.texture = Resources.Load("img_" + you) as Texture;
        imgCom.texture = Resources.Load("img_" + com) as Texture;

        imgCom.transform.localScale = new Vector3(-1, 1, 1);

        // 승패 회수 표시
        txtYou.text = cntYou.ToString();
        txtCom.text = cntCom.ToString();

        // txtResult의 애니메이션 실행 (여기서는 잠시 막아놓음)
        //txtResult.GetComponent<Animantor>().Play("TextScale", -1, 0);

    }

    // 게임 결과를 UI에 반영
    void SetResult(int you, int com, string str)
    {
        Debug.Log("SetResult() method is called ...[" + you + "],[" + com + "]");

        // 이미지 바꾸기
        // Image 타입으로 선언된 경우
        //imgYou.sprite = Resources.Load("img_" + you, typeof(Sprite)) as Sprite;
        // Raw Image 타입으로 선언된 경우
        imgYou.texture = Resources.Load("img_" + you) as Texture;
        imgCom.texture = Resources.Load("img_" + com) as Texture;
        
        // 컴퓨터 이미지를 X축으로 반전 (여기서는 이미지가 찌그러져서 잠시 막아놓음)
        //imgCom.transform.localScale = new Vector3(-1, 1, 1);

        // 승리한 회수(승패 회수) 표시
        txtYou.text = cntYou.ToString();
        txtCom.text = cntCom.ToString();
        // 결과 표시
        txtResult.text = str;

    }

}
