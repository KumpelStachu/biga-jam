using System.Linq;
using UnityEngine;

public class RoomScript : MonoBehaviour {
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject cheesePrefab;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject wallTop;
    [SerializeField] private GameObject wallBottom;
    [SerializeField] private GameObject wallLeft;
    [SerializeField] private GameObject wallRight;

    private GameManager gameManager;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();

        foreach (var spawnPoint in spawnPoints.OrderBy(_ => Random.Range(0f, 1f)).Take(Random.Range(3, 9))) {
            var point = Instantiate(cheesePrefab, spawnPoint.localPosition, Quaternion.identity);
            point.transform.Rotate(0, 0, Random.Range(0f, 360f));
            point.transform.SetParent(transform, false);
        }
    }

    public bool DoorTop { set { wallTop.SetActive(!value); } }
    public bool DoorBottom { set { wallBottom.SetActive(!value); } }
    public bool DoorLeft { set { wallLeft.SetActive(!value); } }
    public bool DoorRight { set { wallRight.SetActive(!value); } }
}

