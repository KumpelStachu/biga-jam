using UnityEngine;

public class TrapRoomScript : MonoBehaviour {
    [SerializeField] private float roomDuration = 10;
    [SerializeField] private GameObject statuePrefab;

    private RoomScript roomScript;
    private bool left, right, top, bottom;

    void Start() {
        roomScript = GetComponent<RoomScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag(Tag.Mouse)) return;

        CloseRoom();
        GetComponent<BoxCollider2D>().enabled = false;
        Invoke(nameof(OpenRoom), roomDuration);

        Instantiate(statuePrefab, transform, false);
    }

    public void CloseRoom() {
        left = roomScript.DoorLeft;
        right = roomScript.DoorRight;
        top = roomScript.DoorTop;
        bottom = roomScript.DoorBottom;

        roomScript.DoorLeft = false;
        roomScript.DoorRight = false;
        roomScript.DoorTop = false;
        roomScript.DoorBottom = false;
    }

    public void OpenRoom() {
        roomScript.DoorLeft = left;
        roomScript.DoorRight = right;
        roomScript.DoorTop = top;
        roomScript.DoorBottom = bottom;
    }
}
