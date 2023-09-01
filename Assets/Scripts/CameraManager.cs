using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private GameObject mouse;
    [SerializeField] private Vector3 offset = new(0, 0, -10);
    [SerializeField] private float smoothTime = 0.25f;

    private readonly Vector2 roomSize = new(160 / 9f, 10);
    private Vector3 currentVelocity;
    private int cameraSize = 6;
    private float time;

    void LateUpdate() {
        time += Time.deltaTime * smoothTime;

        //if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
        //    time = 0;

        //if (Input.GetKeyDown(KeyCode.Alpha1)) cameraSize = 6;
        //else if (Input.GetKeyDown(KeyCode.Alpha2)) cameraSize = 8;

        Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, cameraSize, time);

        var mp = mouse.transform.position;
        var room = new Vector3(Mathf.RoundToInt(mp.x / roomSize.x) * roomSize.x, Mathf.RoundToInt(mp.y / roomSize.y) * roomSize.y);

        transform.position = Vector3.SmoothDamp(transform.position, room + offset, ref currentVelocity, smoothTime);
    }

    public bool CloseCamera { set { time = 0; cameraSize = value ? 5 : 6; } }
}
