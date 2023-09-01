using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private GameObject mouse;
    [SerializeField] private Vector3 offset = new(0, 0, -10);
    [SerializeField] private float smoothTime = 0.25f;

    private readonly Vector2 roomSize = new(160 / 9f, 10);
    private readonly float cameraSize = 6f;
    private Vector3 currentVelocity;
    private float time;

    void LateUpdate() {
        time += Time.deltaTime * smoothTime;

        Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, cameraSize, time);

        var mp = mouse.transform.position;
        var room = new Vector3(Mathf.RoundToInt(mp.x / roomSize.x) * roomSize.x, Mathf.RoundToInt(mp.y / roomSize.y) * roomSize.y);

        transform.position = Vector3.SmoothDamp(transform.position, room + offset, ref currentVelocity, smoothTime);
    }
}
