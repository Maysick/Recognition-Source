using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour {

    public float width;
    public float height;
    public int amount;
    public GameObject starPrefab;

    void Awake() {
        for (int i = 0; i < amount; i++) {
            GameObject newStar = Instantiate(starPrefab, transform, true) as GameObject;
            newStar.transform.position = new Vector3(Random.Range(-width, width), Random.Range(-height, height), 0);
            newStar.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360f));
        }
    }
}
