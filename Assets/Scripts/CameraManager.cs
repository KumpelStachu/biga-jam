using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private GameObject mouse;
    [SerializeField] private Vector3 offset = new(0, 0, -10);
    [SerializeField] private float smoothTime = 0.25f;

    private readonly Vector2 roomSize = new(160 / 9, 10);
    private Vector3 currentVelocity;
    private int cameraSize = 5;
    private float time;

    void LateUpdate() {
        time += Time.deltaTime * smoothTime;

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
            time = 0;

        if (Input.GetKeyDown(KeyCode.Alpha1)) cameraSize = 5;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) cameraSize = 8;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) cameraSize = 13;

        Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, cameraSize, time);

        var room = new Vector3(roomSize.x, roomSize.y);

        transform.position = Vector3.SmoothDamp(transform.position, room + offset, ref currentVelocity, smoothTime);
    }
}
