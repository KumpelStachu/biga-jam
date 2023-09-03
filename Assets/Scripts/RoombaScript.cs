using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaScript : MonoBehaviour {
    [SerializeField] private Rigidbody2D roombaRb;
    [SerializeField] private float minSpeed = 100;
    [SerializeField] private float maxSpeed = 150;
    [SerializeField] private bool isActive = true;
    [SerializeField] private float cooldown = 5;
    [Range(0, 1)][SerializeField] private float spawnChance = 0.5f;

    public void Awake() {
        if (Random.value > spawnChance)
            Destroy(gameObject);
    }

    public void Start() {
        var mouse = GameObject.FindGameObjectWithTag("Mouse");

        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), mouse.GetComponent<EdgeCollider2D>());

        var dirX = Random.value < 0.5f ? 1 : -1;
        var dirY = Random.value < 0.5f ? 1 : -1;

        roombaRb.AddForce(new Vector2(Random.Range(minSpeed, maxSpeed) * dirX, Random.Range(minSpeed, maxSpeed) * dirY));
    }

    public bool Roomb() {
        if (!isActive) return false;

        isActive = false;
        // hide_spikes()

        Invoke(nameof(UnRoomb), cooldown);

        return true;
    }

    public void UnRoomb() {
        isActive = true;
        // show_spikes()
    }
}
