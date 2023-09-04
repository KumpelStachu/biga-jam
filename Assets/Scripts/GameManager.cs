using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    [SerializeField] private Animator Transition_animator;
    [SerializeField] private GameObject GameOverHolder;
    [SerializeField] private GameObject MiotlaHolder;
    [SerializeField] private GameObject mouse;
    [SerializeField] private ParticleSystem mouse_particle;
    [SerializeField] private Color mouse_particle_color_1;
    [SerializeField] private Color mouse_particle_color_2;
    [SerializeField] private Sprite mouse_shielded;
    [SerializeField] private Sprite mouse_normal_sprite;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text cheeseTextHolder;
    [SerializeField] private Animator scoreTextAnimator;
    [SerializeField] private CheeseBarScript cheeseBarScript;
    [SerializeField] private AudioManagerScript audioManager;
    [SerializeField] private float miotlaInitialDelay = 6;
    [SerializeField] private float miotlaDelay = 8;
    [SerializeField] private float miotlaDelayMultiplier = 0.9f;
    [SerializeField] private bool canIDie = true;

    private FollowMouseScript mouseScript;
    public int playerScore;
    public Dictionary<Vector2, RoomScript> roomScripts = new();

    void Start() {
        Transition_animator.Play("GameSceneTransition");
        cheeseTextHolder.text = $"0/{FollowMouseScript.maxCheese}";
        mouseScript = mouse.GetComponent<FollowMouseScript>();

        UpdateScore();
        StartCoroutine(nameof(SpawnMiotla));
    }

    public int AddScore(int score) {
        playerScore += score;
        UpdateScore();
        return 0;
    }

    private void UpdateScore() {
        scoreTextAnimator.Play("Adding_score");
        scoreText.text = playerScore.ToString();
    }

    public void UpdateCheeseCounter() {
        cheeseTextHolder.text = $"{mouseScript.cheeseCounter}/{FollowMouseScript.maxCheese}";
    }

    public void CheeseBarHeal(int heal) {
        cheeseBarScript.Heal(heal);
    }

    public void CheeseBarRemoveHeal(int heal) {
        cheeseBarScript.Remove(heal);
    }

    public void ShowSpeeed(float time) {
        // TODO: show speeed bar
        var main = mouse_particle.main;
        main.startColor = mouse_particle_color_1;
        var em = mouse_particle.emission;
        em.rateOverTime = 70f;
        Invoke(nameof(HideSpeeed), time);
    }

    public void ShowGod(float time) {
        mouse.GetComponent<Animator>().Play("Mouse_god_mode");
        var main = mouse_particle.main;
        main.startColor = Color.magenta;

        Invoke(nameof(HideGod), time);
    }

    public void HideSpeeed() {
        var main = mouse_particle.main;
        main.startColor = mouse_particle_color_2;
        var em = mouse_particle.emission;
        em.rateOverTime = 20f;
    }

    public void HideGod() {
        mouse.GetComponent<Animator>().Play("Mouse_empty");
        var main = mouse_particle.main;
        main.startColor = mouse_particle_color_2;
    }

    public void GameOverShow() {
        if (!canIDie) return;

        var high = PlayerPrefs.GetInt("HighScore", 0);

        if (playerScore > high) {
            high = playerScore;
            PlayerPrefs.SetInt("HighScore", high);
            PlayerPrefs.Save();
        }

        highScoreText.text = $"High Score: {high}";

        audioManager.StopMusic();
        GameOverHolder.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResetGame() {
        Transition_animator.Play("GameSceneTransition_back");
        StartCoroutine(nameof(ResetScene));
    }

    public IEnumerator ResetScene() {
        yield return new WaitForSecondsRealtime(0.1f);

        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public IEnumerator BackToMainMenu() {
        Transition_animator.Play("GameSceneTransition_back");
        yield return new WaitForSecondsRealtime(0.1f);

        SceneManager.LoadScene("MainScene");
    }

    public void LoadMainMenu() {
        StartCoroutine(nameof(BackToMainMenu));
    }

    public IEnumerator SpawnMiotla() {
        yield return new WaitForSeconds(miotlaInitialDelay);

        while (true) {
            var miotla = Instantiate(MiotlaHolder);
            float r = Random.Range(Camera.main.transform.position.y - 5f, Camera.main.transform.position.y + 5f);
            var dir = Camera.main.transform.position.x < mouse.transform.position.x ? 1 : -1;
            miotla.transform.localPosition = new Vector2(Camera.main.transform.position.x + 20f * -dir, r);
            miotla.GetComponent<MiotlaScript>().Dir = dir;

            yield return new WaitForSeconds(miotlaDelay);
            miotlaDelay *= miotlaDelayMultiplier;
        }
    }
}