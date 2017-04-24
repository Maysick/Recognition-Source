using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EndingTex : MonoBehaviour {

	void Awake() {
        GetComponent<Text>().text = GameManager.overText;
    }
}
