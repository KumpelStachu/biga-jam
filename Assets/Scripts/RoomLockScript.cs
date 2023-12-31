using TMPro;
using UnityEngine;

public class RoomLockScript : MonoBehaviour {
    [SerializeField] private TMP_Text[] counters;
    [SerializeField] private Animator roomLockedAnimator;
    [Range(1, 9)][SerializeField] private int goalMin = 3;
    [Range(1, 9)][SerializeField] private int goalMax = 9;

    private AudioManagerScript audioManager;

    private int goal;
    private int current;

    public void Start() {
        audioManager = FindObjectOfType<AudioManagerScript>();
        goal = Random.Range(goalMin, goalMax + 1);
        UpdateCount();
    }

    public int AddCheese(int count) {
        current += count;

        var ret = current > goal ? current - goal : 0;
        if (ret != 0) current = goal;

        UpdateCount();

        return ret;
    }

    public void UpdateCount() {
        if (current >= goal) {
            foreach (var collider in GetComponents<BoxCollider2D>())
                collider.enabled = false;
            audioManager.Play("door");
            roomLockedAnimator.Play(Animations.LockRoomUnlocked);
        }

        foreach (var counter in counters)
            counter.text = $"{current}/{goal}";
    }

    public void RoomUnlockedState() {
        gameObject.GetComponentInParent<RoomScript>().Locked = false;
    }
}
