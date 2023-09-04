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
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text cheeseTextHolder;
    [SerializeField] private Animator scoreTextAnimator;
    [SerializeField] private CheeseBarScript cheeseBarScript;
    [SerializeField] private float miotlaInitialDelay = 6;
    [SerializeField] private float miotlaDelay = 8;
    [SerializeField] private float miotlaDelayMultiplier = 0.9f;
    [SerializeField] private bool canIDie = true;

    private FollowMouseScript mouseScript;
    public int playerScore;
    public Dictionary<Vector2, RoomScript> roomScripts = new();

    void Start() {
        Transition_animator.Play("MenuTransition_back");
        cheeseTextHolder.text = $"0/{FollowMouseScript.maxCheese}";
        mouseScript = mouse.GetComponent<FollowMouseScript>();
        FindObjectOfType<AudioManagerScript>().Play("Bum");
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

        Invoke(nameof(HideSpeeed), time);
    }

    public void ShowGod(float time) {
        // TODO: show god bar

        Invoke(nameof(HideGod), time);
    }

    public void HideSpeeed() {
        // TODO: hide speeed bar
    }

    public void HideGod() {
        // TODO: hide god bar
    }

    public void GameOverShow() {
        if (!canIDie) return;

        GameOverHolder.SetActive(true);
        mouseScript.SetMouseToStun();
        Time.timeScale = 0;
    }

    public void ResetGame() {
        Transition_animator.Play(Animations.MenuTransition);
        StartCoroutine(nameof(ResetScene));
    }

    public IEnumerator ResetScene() {
        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene("MainScene");
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