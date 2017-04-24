using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class YearString {
    public int year;
    public string text;
}

public class Newscaster : MonoBehaviour {

    public Text text;

    public YearString[] sayings;

    private string goalString = "";

    public float speed = 0.1f;
    private float t = 0;
    private bool complete = true;
    private int index = 0;

    public Animator animator;

    public void CheckYear() {
        for (int i = 0; i < sayings.Length; i++) {
            if (sayings[i].year == GameManager.instance.year) {
                Say(sayings[i]);
                return;
            }
        }
    }

    void Update() {
        if (!complete) {
            if (index < goalString.Length) {
                t += Time.deltaTime;
                if (t > speed) {
                    t -= speed;
                    index++;
                    text.text = goalString.Substring(0, index);
                }
            } else {
                complete = true;
                animator.SetBool("Speaking", false);
            }
        }
    }

    void Say(YearString ys) {
        text.text = "";
        goalString = ys.text;
        index = 0;
        complete = false;
        animator.SetBool("Speaking", true);
    }
}
