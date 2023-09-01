using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public class RoomLockScript : MonoBehaviour {
    [SerializeField] private TMP_Text[] counters;
    [SerializeField] private int goalMin = 3;
    [SerializeField] private int goalMax = 10;

    private int goal;
    private int current;

    public void Start() {
        goal = Random.Range(goalMin, goalMax + 1);
        UpdateCount();
    }

    public int AddCheese(int count) {
        current += count;

        UpdateCount();

        return current > goal ? current - goal : 0;
    }

    public void UpdateCount() {
        if (current >= goal) gameObject.GetComponentInParent<RoomScript>().Locked = false;

        foreach (var counter in counters)
            counter.text = $"{current}/{goal}";
    }
}
