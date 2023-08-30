using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    [SerializeField] private Animator menu_animator;
    public void ButtonStart() {
        menu_animator.Play("Main_menu");
        Invoke("Start_game", 1.5f);
    }

    public void ButtonOptions() {
        SceneManager.LoadScene("OptionsScene");
    }

    public void ButtonExit() {
        Application.Quit(69);
    }
    public void Start_game()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
