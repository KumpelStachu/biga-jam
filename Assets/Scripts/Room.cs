using UnityEngine;

public class Room {
    public Vector2 position;
    public int type;
    public bool doorTop, doorBottom, doorLeft, doorRight;

    public Room(Vector2 position, int type) {
        this.position = position;
        this.type = type;
    }
}
