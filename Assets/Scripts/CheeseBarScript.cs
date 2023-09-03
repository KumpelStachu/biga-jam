using UnityEngine;
using UnityEngine.UI;

public class CheeseBarScript : MonoBehaviour {
    public Image healthBar;

    [SerializeField] private float health, maxhealth = 100;
    [SerializeField] private GameManager gameManagerScript;
<<<<<<< HEAD
    [SerializeField] private float cheeseBarSpeed = 3f;

    public float MaxHealth { get { return maxhealth; } }

=======
    [SerializeField] private float cheeseBarSpeed = 3f;

>>>>>>> 0bfba800774ff71dc5fb8f650cffdff914be4381
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
<<<<<<< HEAD
        }

        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxhealth, lerpSpeed);
=======
        }

        HealthBarFiller();
>>>>>>> 0bfba800774ff71dc5fb8f650cffdff914be4381
        lerpSpeed = 3f * Time.deltaTime;
    }

    private void FixedUpdate() {
        cheeseBarSpeed *= 1 + 0.005f * Time.deltaTime;
    }

    public void Heal(float points) {
        health = Mathf.Clamp(health + points, 0, maxhealth);
    }

<<<<<<< HEAD
    public void Remove(float points) {
        Heal(-points);
    }
=======
    public void Heal(float healingPoints) {
        if (health < maxhealth)
            health += healingPoints;
    }

    public void Remove(float RemovingPoints) {
        if (health < maxhealth)
            health -= RemovingPoints;
    }
>>>>>>> 0bfba800774ff71dc5fb8f650cffdff914be4381
}
