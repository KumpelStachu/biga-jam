using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private GameObject mouse;
    [SerializeField] private Vector3 offset = new(0, 0, -10);
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private float cameraSize = 6.1f;
    [SerializeField] private float pointerMultiplier = 1.1f;

    private readonly Vector2 roomSize = new(160 / 9f, 10);
    private Vector3 currentVelocity;
    private float time;

    public void OnDrawGizmos() {
        var pointer = (Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(.5f, .5f)) * 2;
        if (pointer.magnitude > 1) pointer.Normalize();

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(mouse.transform.position, .12f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(mouse.transform.position + pointer * pointerMultiplier, .1f);
    }

    public void LateUpdate() {
        time += Time.deltaTime * smoothTime;

        Camera.main.orthographicSize = Mathf.SmoothStep(Camera.main.orthographicSize, cameraSize, time);

        var pointer = (Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(.5f, .5f)) * 2;
        if (pointer.magnitude > 1) pointer.Normalize();

        var mp = mouse.transform.position + pointer * pointerMultiplier;
        var room = new Vector2(Mathf.RoundToInt(mp.x / roomSize.x), Mathf.RoundToInt(mp.y / roomSize.y));

        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(room.x * roomSize.x, room.y * roomSize.y) + offset, ref currentVelocity, smoothTime);
    }
}
