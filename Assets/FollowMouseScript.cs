using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowMouseScript : MonoBehaviour {
    [SerializeField] private float speed = 1;
    [SerializeField] private float rspeed = 7200;

    void Start() {

    }

    void Update() {
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //if (Input.GetMouseButton(0))
        transform.position = Vector2.MoveTowards(transform.position, mouse, speed * Time.deltaTime);

        //transform.position = Vector3.RotateTowards(transform.forward, mouse, 100, 0.0f);
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rspeed * Time.deltaTime);
    }
}
