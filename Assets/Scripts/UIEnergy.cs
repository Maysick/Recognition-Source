using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnergy : MonoBehaviour {

    private Text text;

    void Awake() {
        text = GetComponent<Text>();
    }

    void Update() {
        text.text = GameManager.instance.energy.ToString();
    }
}
