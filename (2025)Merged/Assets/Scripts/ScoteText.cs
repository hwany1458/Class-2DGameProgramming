using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoteText : MonoBehaviour {

    public Text txtScore;   // Text Widget
    float speed = 1f;       // 이동 속도

  	// Use this for initialization
	void Start () {
        StartCoroutine(Fadeout());
	}
	
	// 위로 이동
	void Update () {
        float amount = speed * Time.deltaTime;
        transform.Translate(Vector3.up * amount);
	}

    // 투명하게 사라지기
    IEnumerator Fadeout() {
        yield return new WaitForSeconds(1f);
        Color color = txtScore.color;

        // 투명하게 사라지기
        for (float alpha = 1; alpha > 0; alpha -= 0.02f) {
            color.a = alpha;
            txtScore.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }

    // 점수 설정 - 외부 호출
    void SetScore (int score) {
        txtScore.text = score.ToString("+0; -0");

        if (score < 0) {
            txtScore.color = Color.red;
        }
    }
}
