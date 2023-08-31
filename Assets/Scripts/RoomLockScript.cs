using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public class RoomLockScript : MonoBehaviour {
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private int goal = 5;
    private int current;

    public void Start() {
        UpdateCount();
    }

    public int AddCheese(int count) {
        current += count;

        UpdateCount();

        return current > goal ? current - goal : 0;
    }

    public void UpdateCount() {
        if (current >= goal) gameObject.SetActive(false);

        tmpText.text = $"{current}/{goal}";
    }
}
