using UnityEngine;

public class RoombaCollider2D : MonoBehaviour {
    void Start() {
        var mouse = GameObject.FindGameObjectWithTag(Tag.Mouse);

        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), mouse.GetComponent<EdgeCollider2D>());
    }
}
