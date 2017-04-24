using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour {

    private Text text;

    void Awake() {
        text = GetComponent<Text>();
    }

    void Update() {
        int year = 2020 + GameManager.instance.year;
        text.text = year.ToString();
    }
}
