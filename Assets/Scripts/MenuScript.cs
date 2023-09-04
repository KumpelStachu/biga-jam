using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    [SerializeField] private Animator menu_animator;
    [SerializeField] private GameObject OptionsHolder;

    [SerializeField] private Animator mouse_animator;
    [SerializeField] private Animator clouds_animator;
    [SerializeField] private Animator Transition_animator;

    public void Start() {
        Time.timeScale = 1.0f;
    }

    public void ButtonStart() {
        menu_animator.Play(Animations.MainMenu);
        mouse_animator.Play(Animations.MouseRun);
        clouds_animator.Play(Animations.CloudsExit);
        Invoke(nameof(MenuTransition), 1f);
        Invoke(nameof(Start_game), 1.5f);
    }

    private void MenuTransition() {
        Transition_animator.Play(Animations.MenuTransition);
    }
    public void Start_game() {
        SceneManager.LoadScene("SampleScene");
    }

    public void OptionsLoad() {
        OptionsHolder.SetActive(true);
    }
    public void OptionsExit() {
        OptionsHolder.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }

}
