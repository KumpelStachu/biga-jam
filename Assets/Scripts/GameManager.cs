using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private GameObject cheesePrefab;

    void Start() {
        GenerateRooms(0);
        Debug.Log(GameObject.FindGameObjectsWithTag("cheese").Length);
    }

    void Update() {

    }

    private void GenerateRooms(int level) {
        var size = (int)Math.Pow(level + 1, 1.1);

        for (int i = -size; i < size + 1; i++) {
            for (int j = 0; j < size * 2; j++) {
                if (i == 0 && j == 0) continue;
                var room = GetRandomRoom();
                Instantiate(room, new Vector2(i * 160f / 9, j * 10), Quaternion.identity);
            }
        }


    }

    private GameObject GetRandomRoom() {
        return roomPrefabs.ElementAt(UnityEngine.Random.Range(0, roomPrefabs.Length));
    }
}