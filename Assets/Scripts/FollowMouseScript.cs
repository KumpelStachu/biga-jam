using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowMouseScript : MonoBehaviour {
    [SerializeField] private float speed = 1;
    [SerializeField] private float distance = 0.05f;

    void Start() {

    }

    void Update() {
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //if (Vector3.Distance(transform.position, mouse) < distance + 10) return;

        if (Input.GetMouseButton(0)) {
            transform.position = Vector2.MoveTowards(transform.position, mouse, speed * Time.deltaTime * (Input.GetKey(KeyCode.Space) ? 3 : 1));
        }

        Quaternion rotation = Quaternion.LookRotation(mouse - transform.position, transform.TransformDirection(Vector3.back));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }
}
