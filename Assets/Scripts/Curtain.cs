using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Curtain : MonoBehaviour {

    public float speed = 0.5f;

    private float a = 0;
    public RawImage image;

    public GameObject[] others;

    void Update() {
        a += Time.deltaTime * speed;
        image.color = new Color(1, 1, 1, a);
        if (a >= 1) {
            for (int i = 0; i < others.Length; i++) {
                others[i].SetActive(true);
            }
        }
    }
}
