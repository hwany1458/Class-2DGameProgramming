using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // Legacy Text
using TMPro;  // Text Mesh Pro

public class GameManager : MonoBehaviour
{
    // ��������
    // Unity���� ���� : public ���ٴ� [SerializeField] private
    // ������(public, private) �뵵�� �°� �����ϰ� �����ϵ���
    [SerializeField] private RawImage imgYou;   // ����� ���������� �̹��� ǥ�ÿ�
    [SerializeField] private RawImage imgCom;  // ��ǻ�� �̹��� ǥ�ÿ�
    //[SerializeField] private Image imgCom;     // ��ǻ�� �̹��� ǥ�ÿ�
    // �̹����� ���� ��, 
    //   Image Ÿ�� -- Image ����
    //   Raw Image Ÿ�� -- RawImage ����

    [SerializeField] private TMP_Text txtYou;    // ����� �¸��� ȸ�� ǥ�ÿ�
    [SerializeField] private TMP_Text txtCom;   // ��ǻ�� �¸��� ȸ�� ǥ�ÿ�
    [SerializeField] private TMP_Text txtResult; // ���� ��� ǥ�ÿ�

    int cntYou = 0;   // ����� �¸� ȸ��
    int cntCom = 0;  // ��ǻ�� �¸� ȸ��

    //--------------- �޼ҵ�

    void Start() {     }

    private void Awake()
    {
        InitGame();
    }


    void Update()
    {
        // ESC key�� ���� ����
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // ���߿� ���ӵ��� ��Ƽ�(?) �ϳ��� ������Ʈ�� ������ ���� �ʿ�
            Application.Quit();
        }
    }

    // ��ư Ŭ�� (���� ���� �� �̹����� Ŭ������ ��)
    // �ٸ� ������Ʈ���� ������ �޼ҵ带 ȣ���ϱ� ������ public ���� �����ؾ� ��
    public void OnButtonClick(GameObject button)
    {
        Debug.Log(button.name);
        // Ŭ���� ��ư ��ȣ�� �̾ƿ´� (�̸����� 8��° ���ڿ��� 1�� ���ڸ�)
        int you = int.Parse(button.name.Substring(7, 1));  // 1,2,3
        
        // ���翡�� ��ǻ�Ͱ� �̴� �������ڸ� CheckResult()���� ������
        // (���⼭��) ���� ���� ��, ��ǻ�͵� ���� ����
        int com = Random.Range(1, 4);  // 1,2,3

        // �̰�� ���� ���ٸ� �Ǵ� (���翡��)
        // CheckResult(you, com);

        // ���� (�¹��� ������ �ϰ�, ��� �ѷ��ִ� �Լ����� �Ѳ����� ȭ�� ǥ�ø� ��� ó��)
        // ���ƿ��� res�� �̱�� 1, ���� 0, ���� -1
        int res = CheckResult1(you, com);
        
        string str = "";
        switch(res)
        {
            case 1:  // �̱� ���
                cntYou++;
                //txtResult.text = "Win";
                str = "Win";
                break;
            case 0:  // ��� ���
                //txtResult.text = "Draw";
                str = "Draw";
                break;
            case -1:  // �� ���
                cntCom++;
                //txtResult.text = "Lose";
                str = "Lose";
                break;
            default:
                break;
        }
        Debug.Log("Check is " + res);

        // ���� ����� UI�� �ݿ� (������)
        //SetResult(you, com);
        // ���� (��� ����� �Ѳ����� ǥ���ϵ��� ������)
        SetResult(you, com, str);
    }

    // Pointer Exit
    public void OnMouseExit(GameObject button)
    {
        Animator anim = button.GetComponent<Animator>();
        anim.Play("Normal");
    }

    // -------------- User-defined method
    
    // ���� �ʱ�ȭ �Լ�
    void InitGame()
    {
        // (1���) ���翡�� ó��, GameObject.Find() ó���ص� ��
        /*  (��� component Ÿ���� �����ϰ� ���� ��)
          imgYou = GameObject.Find(��ImgYou��).GetComponent<Image>();
          imgCom = GameObject.Find(��ImgCom��).GetComponent<Image>();

          txtYou = GameObject.Find(��TxtYou��).GetComponent<Text>();
          txtCom = GameObject.Find(��TxtCom��).GetComponent<Text>();
          txtResult = GameObject.Find(��TxtResult��).GetComponent<Text>();

         * */
        // (2���) ������ public �����ؼ�, �巡�׷� �Ҵ�
    }

    // �̰�� ���ٸ� �Ǻ��ϴ� �Լ� (���� ����)
    void CheckResult(int you, int com)
    {
        /*
        //--------------------------------
        // ���� �������� �� (1)
        if (you == 1) {
            if (com == 1) { // ���� ���� (1)
                // ��� (��)
            }  
            else if (com == 2) { // ���� ���� (2)
                // ���� (��)
            }
            else {  // ���� ��(3)
                // �̰�� (��)
            }  
        }

        // ���� �������� �� (2)
        else if (you == 2) {
            if (com == 1)
            { // ���� ���� (1)
            }
            else if (com == 2)
            { // ���� ���� (2)
            }
            else
            {  // ���� ��(3)
            }

        }

        // ���� ������ �� (3)
        else {
            if (com == 1)
            { // ���� ���� (1)
            }
            else if (com == 2)
            { // ���� ���� (2)
            }
            else
            {  // ���� ��(3)
            }

        }
        //--------------------------------
        */

    }

    // ���� ������ �Ǻ��Ͽ� ��ȯ
    int CheckResult1(int you, int com)
    {
        int res;  // ��ȯ�� (�̱�� 1, ���� 0, ���� -1)

        int k = you - com;
        Debug.Log("CheckResult1() method is called ...[" + you + "],[" + com + "]");

        /*
        if (k == 0) 
        { txtResult.text = "Draw"; }
        else if (k == 1 || k == -2) 
        { txtResult.text = "Win"; cntYou++; }
        else   // k=2 �Ǵ� k=-1
        { txtResult.text = "Lose"; cntCom++; }
         * �Ʒ��� ���� ������
         */

        if (k == 0) { res = 0; }
        else if ((k == 1) || (k == -2)) { res = 1; }
        else { res = -1; }

        return res;
    }

    // ���������� ����� ȭ�鿡 ǥ��
    // ���� ����� UI�� �ݿ� -- (���� ���)
    void SetResult(int you, int com)
    {
        Debug.Log("SetResult() method is called ...[" + you + "],[" + com + "]");

        // Image Ÿ������ ����� ���
        //imgYou.sprite = Resources.Load("img_" + you, typeof(Sprite)) as Sprite;
        // Raw Image Ÿ������ ����� ���
        imgYou.texture = Resources.Load("img_" + you) as Texture;
        imgCom.texture = Resources.Load("img_" + com) as Texture;

        imgCom.transform.localScale = new Vector3(-1, 1, 1);

        // ���� ȸ�� ǥ��
        txtYou.text = cntYou.ToString();
        txtCom.text = cntCom.ToString();

        // txtResult�� �ִϸ��̼� ���� (���⼭�� ��� ���Ƴ���)
        //txtResult.GetComponent<Animantor>().Play("TextScale", -1, 0);

    }

    // ���� ����� UI�� �ݿ�
    void SetResult(int you, int com, string str)
    {
        Debug.Log("SetResult() method is called ...[" + you + "],[" + com + "]");

        // �̹��� �ٲٱ�
        // Image Ÿ������ ����� ���
        //imgYou.sprite = Resources.Load("img_" + you, typeof(Sprite)) as Sprite;
        // Raw Image Ÿ������ ����� ���
        imgYou.texture = Resources.Load("img_" + you) as Texture;
        imgCom.texture = Resources.Load("img_" + com) as Texture;
        
        // ��ǻ�� �̹����� X������ ���� (���⼭�� �̹����� ��׷����� ��� ���Ƴ���)
        //imgCom.transform.localScale = new Vector3(-1, 1, 1);

        // �¸��� ȸ��(���� ȸ��) ǥ��
        txtYou.text = cntYou.ToString();
        txtCom.text = cntCom.ToString();
        // ��� ǥ��
        txtResult.text = str;

    }

}
