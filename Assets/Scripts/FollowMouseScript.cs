using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using TMPro;

public class FollowMouseScript : MonoBehaviour {
    [SerializeField] private float speed = 1;
    [SerializeField] private int cheese_counter = 0;
    [SerializeField] private float distance = 10.05f;
    [SerializeField] private GameObject cheese_holder;

    void Start() {

    }

    void Update() {
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector3.Distance(transform.position, mouse) < distance) return;

        if (Input.GetMouseButton(0)) {
            transform.position = Vector2.MoveTowards(transform.position, mouse, speed * Time.deltaTime * (Input.GetKey(KeyCode.Space) ? 3 : 1));
        }

        Quaternion rotation = Quaternion.LookRotation(mouse - transform.position, transform.TransformDirection(Vector3.back));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (cheese_counter < 3) {
            cheese_holder = col.gameObject;
            cheese_holder.transform.SetParent(transform, false);
            cheese_holder.transform.position = transform.position;
            cheese_counter++;
        }
    }
}
