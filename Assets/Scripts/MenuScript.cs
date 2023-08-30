using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    public void ButtonStart() {
        SceneManager.LoadScene("SampleScene");
    }

    public void ButtonOptions() {
        SceneManager.LoadScene("OptionsScene");
    }

    public void ButtonExit() {
        Application.Quit(69);
    }
}
