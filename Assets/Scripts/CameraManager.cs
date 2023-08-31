using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private GameObject mouse;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float smoothTime = 0.25f;

    private Vector3 currentVelocity;
    private int cameraSize = 5;
    private float time;

    void LateUpdate() {
        time += Time.deltaTime * smoothTime;

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
            time = 0;

        if (Input.GetKeyDown(KeyCode.Alpha1)) cameraSize = 5;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) cameraSize = 8;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) cameraSize = 13;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) cameraSize = 21;

        Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, cameraSize, time);

        transform.position = Vector3.SmoothDamp(transform.position, mouse.transform.position + offset, ref currentVelocity, smoothTime);
    }
}
