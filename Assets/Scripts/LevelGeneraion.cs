using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGeneraion : MonoBehaviour {
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private Vector2 worldSize = new(100, 100);
    [SerializeField] private int numberOfRooms = 200;

    private Room[,] rooms;
    readonly List<Vector2> takenPositions = new();
    private int gridSizeX, gridSizeY;
    private GameManager gameManager;

    void Start() {
        gameManager = GetComponent<GameManager>();

        if (numberOfRooms >= worldSize.x * 2 * worldSize.y * 2) {
            numberOfRooms = Mathf.RoundToInt(worldSize.x * 2 * worldSize.y * 2);
        }

        gridSizeX = Mathf.RoundToInt(worldSize.x);
        gridSizeY = Mathf.RoundToInt(worldSize.y);

        CreateRooms();
        SetRoomDoors();
        DrawMap();
    }

    void CreateRooms() {
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new(Vector2.zero, 0);
        takenPositions.Insert(0, Vector2.zero);

        rooms[gridSizeX - 1, gridSizeY] = new(Vector2.left, 1);
        takenPositions.Insert(0, Vector2.left);

        float randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

        for (int i = 0; i < numberOfRooms - 1; i++) {
            float randomPercent = i / ((float)numberOfRooms - 1);
            float randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPercent);
            var checkPos = NewPosition();

            if (NumberofNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare) {
                int iterations = 0;

                do {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberofNeighbors(checkPos, takenPositions) > 1 && iterations < 100);

                if (iterations >= 100) {
                    //Debug.LogError("Could not create with fewer neighbors than: " + NumberofNeighbors(checkPos, takenPositions));
                }
            }

            rooms[(int)(checkPos.x + gridSizeX), (int)(checkPos.y + gridSizeY)] = new Room(checkPos, Random.Range(1, roomPrefabs.Length));
            takenPositions.Insert(0, checkPos);
        }
    }

    void SetRoomDoors() {
        for (int x = 0; x < gridSizeX * 2; x++) {
            for (int y = 0; y < gridSizeY * 2; y++) {
                if (rooms[x, y] == null) continue;
                if (y - 1 < 0) rooms[x, y].doorBottom = false;
                else rooms[x, y].doorBottom = (rooms[x, y - 1] != null);

                if (y + 1 >= gridSizeY * 2) rooms[x, y].doorTop = false;
                else rooms[x, y].doorTop = (rooms[x, y + 1] != null);

                if (x - 1 < 0) rooms[x, y].doorLeft = false;
                else rooms[x, y].doorLeft = (rooms[x - 1, y] != null);

                if (x + 1 >= gridSizeX * 2) rooms[x, y].doorRight = false;
                else rooms[x, y].doorRight = (rooms[x + 1, y] != null);
            }
        }
    }

    void DrawMap() {
        foreach (Room room in rooms) {
            if (room == null) continue;

            var roomScript = Instantiate(roomPrefabs[room.type], room.position * new Vector2(160 / 9f, 10), Quaternion.identity)
                .GetComponent<RoomScript>();

            roomScript.DoorTop = room.doorTop;
            roomScript.DoorBottom = room.doorBottom;
            roomScript.DoorRight = room.doorRight;
            roomScript.DoorLeft = room.doorLeft;
            roomScript.Locked = room.type != 0;

            gameManager.roomScripts.Add(room.position, roomScript);
        }
    }

    private Vector2 NewPosition() {
        Vector2 checkingPos;
        int x, y;

        do {
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;

            bool direction = Random.value < 0.5f;

            if (Random.value < 0.5f) {
                y += direction ? 1 : -1;
            }
            else {
                x += direction ? 1 : -1;
            }

            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);

        return checkingPos;
    }

    private Vector2 SelectiveNewPosition() {
        Vector2 checkingPos;
        int x, y, index, inc;

        do {
            inc = 0;

            do {
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberofNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);

            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;

            bool direction = Random.value < 0.5f;

            if (Random.value < 0.5f) {
                y += direction ? 1 : -1;
            }
            else {
                x += direction ? 1 : -1;
            }

            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);

        if (inc >= 100) {
            //Debug.LogError("Could could not find position with only one neighbor");
        }

        return checkingPos;
    }

    private int NumberofNeighbors(Vector2 checkingPos, List<Vector2> takenPositions) {
        int ret = 0;

        if (takenPositions.Contains(checkingPos + Vector2.right)) ret++;
        if (takenPositions.Contains(checkingPos + Vector2.left)) ret++;
        if (takenPositions.Contains(checkingPos + Vector2.up)) ret++;
        if (takenPositions.Contains(checkingPos + Vector2.down)) ret++;

        return ret;
    }
}
