using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableCost : MonoBehaviour {

    public int cost;
    private Image image;
    private Button button;

    public bool energy;

    void Awake() {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    void Update() {
        if (!energy) {
            if (GameManager.instance.rsrc >= cost) {
                image.color = Color.white;
                button.interactable = true;
            } else {
                image.color = Color.red;
                button.interactable = false;
            }
        } else if (energy) {
            if (GameManager.instance.energy >= cost) {
                image.color = Color.white;
                button.interactable = true;
            } else {
                image.color = Color.red;
                button.interactable = false;
            }
        }

    }
}
