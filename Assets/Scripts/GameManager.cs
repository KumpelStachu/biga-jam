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
        cheeseTextHolder.text = $"{playerCheese}/{FollowMouseScript.maxCheese}";
    }

    public int CheeseBarHeal(int heal) {
        cheeseBarScript.Heal(heal);
        return 0;
    }
    public int CheeseBarRemoveHeal(int heal) {
        cheeseBarScript.Remove(heal);
        return 0;
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
        var miot³a = Instantiate(MiotlaHolder);
        float r = UnityEngine.Random.Range(Camera.main.transform.position.y - 5f, Camera.main.transform.position.y + 5f);
        var dir = Camera.main.transform.position.x < mouse.transform.position.x ? 1 : -1;
        miot³a.transform.position = new Vector2(Camera.main.transform.position.x + 20f * -dir, r);
        miot³a.GetComponent<MiotlaScript>().Dir = dir;
    }
}