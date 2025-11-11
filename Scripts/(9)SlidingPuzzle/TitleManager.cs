using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        InitGame();
	}

    // OnToggle Change
    public void OnToggleChange (Toggle toggle) {
        switch (toggle.name) {
        case "ToggleMusic" :
            Settings.canMusic = toggle.isOn;
            if (toggle.isOn) {
                GetComponent<AudioSource>().Play();
            } else {
                GetComponent<AudioSource>().Pause();
            }
            break;
        case "ToggleSound" :
            Settings.canSound = toggle.isOn;
            break;
        }
    }

    // OnDropdown Change
    public void OnDropdownChange (Dropdown dropdown) {
        Settings.sliceCnt = dropdown.value + 3;
    }

    // OnButton Click
    public void OnButtonClick (Button button) {
        switch (button.name) {
        case "BtnStart" :
            SceneManager.LoadScene("MainGame");
            break;
        case "BtnQuit" :
            Application.Quit();
            break;
        }

        // 사진 선택 버튼
        if (button.name.Contains("BtnPicture")) {
            Settings.imgNum = int.Parse(button.name.Substring(10));

            // 클릭한 버튼 위치로 이동
            Transform imgCursor = GameObject.Find("ImgCursor").transform;
            imgCursor.position = button.transform.position;
        }
    }

    // Init Game
    void InitGame () {
        Toggle music = GameObject.Find("ToggleMusic").GetComponent<Toggle>();
        music.isOn = Settings.canMusic;

        music = GameObject.Find("ToggleMusic2").GetComponent<Toggle>();
        music.isOn = !Settings.canMusic;

        Toggle sound = GameObject.Find("ToggleSound").GetComponent<Toggle>();
        sound.isOn = Settings.canSound;

        sound = GameObject.Find("ToggleSound2").GetComponent<Toggle>();
        sound.isOn = !Settings.canSound;

        Dropdown drop = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        drop.value = Settings.sliceCnt - 3;

        Transform imgCursor = GameObject.Find("ImgCursor").transform;
        Transform button = GameObject.Find("BtnPicture" + Settings.imgNum).transform;
        imgCursor.position = button.position;

        GetComponent<AudioSource>().playOnAwake = Settings.canMusic;
    }
}
