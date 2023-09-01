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
    [SerializeField] public int playerScore;
    [SerializeField] public int playerCheese;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text cheeseTextHolder;
    [SerializeField] private Animator scoreTextAnimator;
    [SerializeField] private CheeseBarScript cheeseBarScript;

    private FollowMouseScript mouseScript;

    void Start() {
        playerScore = 0;
        playerCheese = 0;
        cheeseTextHolder.text = $"0/{FollowMouseScript.maxCheese}";
        mouseScript = GameObject.FindGameObjectWithTag("Mouse").GetComponent<FollowMouseScript>();
        UpdateScore();
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
        cheeseTextHolder.text = $"{playerCheese}/{FollowMouseScript.maxCheese}";
    }

    public void CheeseBarHeal() {
        cheeseBarScript.Heal(10);
    }

    public void GameOverShow() {
        GameOverHolder.SetActive(true);
        mouseScript.SetMouseToStun();
        Time.timeScale = 0;
    }

    public void ResetGame() {
        SceneManager.LoadScene("MainScene");
    }
}