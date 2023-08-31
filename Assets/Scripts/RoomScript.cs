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
    [SerializeField] private GameObject roomLock;

    private GameManager gameManager;
    private int cheeseCount;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        cheeseCount = Random.Range(5, 9);

        GenerateCheese();
    }

    private void GenerateCheese() {
        gameManager.totalCheese += cheeseCount;
        gameManager.remainingCheese += cheeseCount;

        foreach (var spawnPoint in spawnPoints.OrderBy(_ => Random.Range(0f, 1f)).Take(cheeseCount)) {
            var point = Instantiate(cheesePrefab, spawnPoint.localPosition, Quaternion.identity);
            point.transform.Rotate(0, 0, Random.Range(0f, 360f));
            point.transform.SetParent(transform, false);
        }
    }

    public bool DoorTop { set { wallTop.SetActive(!value); } }
    public bool DoorBottom { set { wallBottom.SetActive(!value); } }
    public bool DoorLeft { set { wallLeft.SetActive(!value); } }
    public bool DoorRight { set { wallRight.SetActive(!value); } }

    public bool Locked { set { roomLock.SetActive(value); } }
}

