using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    [SerializeField] private Animator menu_animator;
    [SerializeField] private GameObject OptionsHolder;
    [SerializeField] private GameObject rankingHolder;
    [SerializeField] private Animator mouse_animator;
    [SerializeField] private Animator clouds_animator;
    [SerializeField] private Animator Transition_animator;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private AudioManagerScript audioManager;
    [SerializeField] private TMP_Text score1, score2, scoreU;

    public void Start() {
        musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("music", 0.5f));
        effectsSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("effects", 0.5f));

        Time.timeScale = 1.0f;

        var high = PlayerPrefs.GetInt("HighScore", 0);

        score1.text = $"{(int)(420 + high * 4.2069) / 10}0";
        score2.text = $"{(int)(69 + high * 2.137) / 10}0";
        scoreU.text = high.ToString();
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
        rankingHolder.SetActive(false);
    }

    public void RankingGame() {
        rankingHolder.SetActive(true);
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
