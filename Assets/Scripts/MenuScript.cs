using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    [SerializeField] private Animator menu_animator;
    [SerializeField] private GameObject OptionsHolder;
    [SerializeField] private Animator mouse_animator;
    [SerializeField] private Animator clouds_animator;
    [SerializeField] private Animator Transition_animator;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private AudioManagerScript audioManager;

    public void Start() {
        musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("music", 0.5f));
        effectsSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("effects", 0.5f));

        Time.timeScale = 1.0f;
    }

    public void ButtonStart() {
        menu_animator.Play(Animations.MainMenu);
        mouse_animator.Play(Animations.MouseRun);
        clouds_animator.Play(Animations.CloudsExit);
        Invoke(nameof(MenuTransition), 1f);
        Invoke(nameof(Start_game), 1.5f);
        FindObjectOfType<AudioManagerScript>().Play("start");
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

    public void MusicSliderChange() {
        PlayerPrefs.SetFloat("music", musicSlider.value);
        PlayerPrefs.Save();

        audioManager.UpdateVolume();
    }

    public void EffectsSliderChange() {
        PlayerPrefs.SetFloat("effects", effectsSlider.value);
        PlayerPrefs.Save();

        audioManager.UpdateVolume();

        CancelInvoke(nameof(TestSound));
        Invoke(nameof(TestSound), 0.2f);
    }

    public void TestSound() {
        audioManager.Play("start");
    }
}
