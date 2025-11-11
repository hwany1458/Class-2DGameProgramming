using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour {

    float speed = 8f;
    Transform highlight;

	// Use this for initialization
	void Awake () {
        highlight = transform.Find("Highlight");
	}
	
	// Update is called once per frame
	void Update () {
        float amount = speed * Time.deltaTime;
        highlight.Translate(Vector3.down * amount);

        Vector3 pos = highlight.position;
        if (pos.y < -30) {
            pos.y = 30;
            highlight.position = pos;
        }
	}
}
