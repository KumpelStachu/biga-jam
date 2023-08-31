using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public class RoomLockScript : MonoBehaviour {
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private int goal = 5;
    [SerializeField] private FollowMouseScript mouseScript;

    private GameManager gameManager;
    private int current;

    public void Start() {
        gameManager = FindObjectOfType<GameManager>();
        mouseScript = GameObject.FindGameObjectWithTag("Mouse").GetComponent<FollowMouseScript>();
        UpdateCount();
    }

    public int AddCheese(int count) {
        current += count;
        gameManager.remainingCheese -= Mathf.Max(current - goal, 0);
        mouseScript.ClearCheese();
        UpdateCount();

        return current > goal ? current - goal : 0;
    }

    public void UpdateCount() {
        if (current >= goal) gameObject.SetActive(false);

        tmpText.text = $"{current}/{goal}";
    }
}
