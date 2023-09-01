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
    

    public int totalCheese;
    public int remainingCheese;
    

    void Start() {
        //GenerateRooms(0);
        //Debug.Log(GameObject.FindGameObjectsWithTag("cheese").Length);
        playerScore = 0;
        playerCheese = 0;
        cheeseTextHolder.text = "0/3";
        UpdateScore();
    }

    void Update() {

    
    }

    private void GenerateRooms(int level) {
        var size = (int)Math.Pow(level + 1, 1.1);

        for (int i = -size; i < size + 1; i++) {
            for (int j = 0; j < size * 2; j++) {
                if (i == 0 && j == 0) continue;
                var room = GetRandomRoom();
                Instantiate(room, new Vector2(i * 160f / 9, j * 10), Quaternion.identity);
            }
        }


    }

    private GameObject GetRandomRoom() {
        return roomPrefabs.ElementAt(UnityEngine.Random.Range(0, roomPrefabs.Length));
    }
    public int AddScore(int score)
    {
        playerScore += score;
        UpdateScore();
        return 0;
    }
    private void UpdateScore()
    {
        scoreTextAnimator.Play("Adding_score");
        scoreText.text = playerScore.ToString();
    }
    public void UpdateCheeseCounter()
    {
        cheeseTextHolder.text = playerCheese.ToString() + "/3";
    }
    public void CheeseBarHeal()
    {
        cheeseBarScript.Heal(10);
    }
    public void GameOverShow()
    {
        GameOverHolder.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResetGame()
    {
        SceneManager.LoadScene("MainScene");

    }
    
}