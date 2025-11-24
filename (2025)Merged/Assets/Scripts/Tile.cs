using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    float speed = 30;       // 이동 속도
    bool canMove = false;   // 이동이 가능한가?

    Vector3 target;         // 이동할 위치

	// Update is called once per frame
	void Update () {
        if (canMove) MoveTile();
	}

    // Move Tile
    void MoveTile () {
        Vector3 pos = transform.position;
        pos = Vector3.MoveTowards(pos, target, speed * Time.deltaTime);  
        transform.position = pos;

        // 목적지와 근접하면 이동 종료
        if (Vector3.Distance(pos, target) < 0.05f) {
            transform.position = target;
            GameObject.Find("PuzzleGameManager").SendMessage("SetCalc");
            canMove = false;
        }
    }

    // SetMove <- GameManager
    void SetMove (Vector3 _target) {
        target = _target;
        canMove = true;

        if (Settings.canSound) {
            GetComponent<AudioSource>().Play();
        }
    }

    // On Click
    void OnMouseDown () {
        int n = int.Parse(name.Substring(4));
        GameObject.Find("PuzzleGameManager").SendMessage("SetTouch", n);
    }
}
