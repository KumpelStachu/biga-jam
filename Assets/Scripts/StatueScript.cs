using TMPro;
using UnityEngine;

public class StatueScript : MonoBehaviour {
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private bool isActive = true;

    public int cheese;

    public void AddCheese(int count) {
        if (!isActive) return;

        cheese += count;
        UpdateCounter();
    }

    public void UpdateCounter() {
        counterText.text = cheese.ToString();
    }

    public void Disable() {
        isActive = false;
    }
}
