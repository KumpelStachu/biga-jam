using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    [SerializeField] private Animator menu_animator;

    [SerializeField] private Animator mouse_animator;
    [SerializeField] private Animator clouds_animator;
    [SerializeField] private Animator Transition_animator;

    public void Start() {
        Time.timeScale = 1.0f;
    }

    public void ButtonStart() {
        menu_animator.Play("Main_menu");
        mouse_animator.Play("Mouse_run");
        clouds_animator.Play("Clouds_exit");
        Invoke("MenuTransition", 1f); 
        Invoke(nameof(Start_game), 1.5f);
    }

    private void MenuTransition()
    {
        Transition_animator.Play("MenuTransition");
    }
    public void Start_game() {
        //SceneManager.UnloadSceneAsync("MainScene");
        SceneManager.LoadScene("SampleScene");
    }
}
