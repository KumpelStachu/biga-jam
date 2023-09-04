using UnityEngine;
using UnityEngine.UI;

public class CheeseBarScript : MonoBehaviour {
    public Image healthBar;

    [SerializeField] private float health, maxhealth = 100;
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private float cheeseBarSpeed = 3f;
    [SerializeField] private float magicNumber = 0.004f;

    public float MaxHealth { get { return maxhealth; } }

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

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxhealth, lerpSpeed);
        lerpSpeed = 3f * Time.deltaTime;
    }

    private void FixedUpdate() {
        cheeseBarSpeed *= 1 + magicNumber * Time.deltaTime;
    }

    public void Heal(float points) {
        health = Mathf.Clamp(health + points, 0, maxhealth);
    }

    public void Remove(float points) {
        Heal(-points);
    }
}
