using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheeseBarScript : MonoBehaviour {
    public Image healthBar;

    [SerializeField] private float health, maxhealth = 100;
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private float cheeseBarSpeed = 3f;

    float lerpSpeed;

    private void Start() {
        health = maxhealth;
    }

    private void Update() {
        health -= cheeseBarSpeed * Time.deltaTime;
        if (health > maxhealth) health = maxhealth;
        if (health <= 0) {
            health = 0;
            gameManagerScript.GameOverShow();
        }

        HealthBarFiller();
        lerpSpeed = 3f * Time.deltaTime;
    }

    private void FixedUpdate() {
        cheeseBarSpeed *= 1 + 0.005f * Time.deltaTime;
    }

    void HealthBarFiller() {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxhealth, lerpSpeed);
    }

    public void Heal(float healingPoints) {
        if (health < maxhealth)
            health += healingPoints;
    }

    public void Remove(float RemovingPoints) {
        if (health < maxhealth)
            health -= RemovingPoints;
    }
}
