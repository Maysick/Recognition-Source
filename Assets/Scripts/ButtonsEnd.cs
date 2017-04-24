using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsEnd : MonoBehaviour {

    public void Replay() {
        SceneManager.LoadScene(0);
    }

	public void Quit() {
        Application.Quit();
    }
}
