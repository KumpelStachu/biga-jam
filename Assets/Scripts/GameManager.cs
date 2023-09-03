using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private GameObject cheesePrefab;
    [SerializeField] private GameObject GameOverHolder;
    [SerializeField] private GameObject MiotlaHolder;
    [SerializeField] private GameObject mouse;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text cheeseTextHolder;
    [SerializeField] private Animator scoreTextAnimator;
    [SerializeField] private CheeseBarScript cheeseBarScript;
    public int playerScore;

    private FollowMouseScript mouseScript;

    void Start() {
        cheeseTextHolder.text = $"0/{FollowMouseScript.maxCheese}";
        mouseScript = mouse.GetComponent<FollowMouseScript>();

        UpdateScore();
        InvokeRepeating(nameof(SpawnMiotla), 6f, 5f);
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

    public void GameOverShow() {
        //GameOverHolder.SetActive(true);
        //mouseScript.SetMouseToStun();
        //Time.timeScale = 0;
    }

    public void ResetGame() {
        SceneManager.LoadScene("MainScene");
    }

    public void SpawnMiotla() {
        var miotla = Instantiate(MiotlaHolder);
        float r = UnityEngine.Random.Range(Camera.main.transform.position.y - 5f, Camera.main.transform.position.y + 5f);
        var dir = Camera.main.transform.position.x < mouse.transform.position.x ? 1 : -1;
        miotla.transform.position = new Vector2(Camera.main.transform.position.x + 20f * -dir, r);
        miotla.GetComponent<MiotlaScript>().Dir = dir;
    }
}