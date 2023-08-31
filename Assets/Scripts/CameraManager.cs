using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private GameObject mouse;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float smoothTime = 0.25f;

    private Vector3 currentVelocity;
    private float time;

    void LateUpdate() {
        var size = 5;
        time += Time.deltaTime * smoothTime;

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyUp(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyUp(KeyCode.X))
            time = 0;

        if (Input.GetKey(KeyCode.X)) size = 11;
        else if (Input.GetKey(KeyCode.Z)) size = 7;

        Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, size, time);

        transform.position = Vector3.SmoothDamp(transform.position, mouse.transform.position + offset, ref currentVelocity, smoothTime);
    }
}
