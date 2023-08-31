using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomScript : MonoBehaviour {
    [SerializeField] private GameObject cheesePrefab;
    [SerializeField] private Transform[] spawnPoints;

    void Start() {
        foreach (var spawnPoint in spawnPoints.OrderBy(_ => Random.Range(0f, 1f)).Take(Random.Range(3, 9))) {
            var point = Instantiate(cheesePrefab, spawnPoint.localPosition, Quaternion.identity);
            point.transform.Rotate(0, 0, Random.Range(0f, 360f));
            point.transform.SetParent(transform, false);
        }
    }
}

