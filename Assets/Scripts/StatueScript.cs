using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatueScript : MonoBehaviour {
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private bool isActive = true;
    [SerializeField] private Animator statueAnimator;

    public int cheese;

    public void AddCheese(int count) {
        if (!isActive) return;

        statueAnimator.Play("StatueAddCheese");
        cheese += count;
        UpdateCounter();
    }

    public void UpdateCounter() {
        counterText.text = cheese.ToString();
    }

    public void Disable() {
        isActive = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().color = Color.white.WithAlpha(0.4f);
    }
}
