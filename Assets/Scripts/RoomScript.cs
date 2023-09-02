using System.Linq;
using UnityEngine;

public class RoomScript : MonoBehaviour {
    [SerializeField] private Transform[] cheeseSpawnPoints;
    [SerializeField] private Transform[] powerupSpawnPoints;
    [SerializeField] private GameObject[] optionalDecoration;
    [SerializeField] private GameObject cheesePrefab;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject wallTop;
    [SerializeField] private GameObject wallBottom;
    [SerializeField] private GameObject wallLeft;
    [SerializeField] private GameObject wallRight;
    [SerializeField] private GameObject roomLock;
    [Min(5)][SerializeField] private int minCheese = 5;
    [Min(6)][SerializeField] private int maxCheese = 10;
    [SerializeField] private float maxDistance = 20;

    private GameObject mouse;

    public void Start() {
        mouse = GameObject.FindGameObjectWithTag("Mouse");

        foreach (var item in optionalDecoration.Take(Random.Range(optionalDecoration.Length / 4, optionalDecoration.Length * 3 / 4)))
            Destroy(item);
    }

    public void OnDrawGizmos() {
        if (Vector2.Distance(transform.position, mouse.transform.position) > maxDistance) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, mouse.transform.position);
    }

    private void A() {
        if (Vector2.Distance(transform.position, mouse.transform.position) > maxDistance) return;
        GenerateCheese();
    }

    public void GenerateCheese(int cheeseCount = 1) {
        foreach (var spawnPoint in cheeseSpawnPoints.OrderBy(_ => Random.Range(0f, 1f)).Take(cheeseCount)) {
            var point = Instantiate(cheesePrefab, spawnPoint.localPosition, Quaternion.identity);
            point.transform.Rotate(0, 0, Random.Range(0f, 360f));
            point.transform.SetParent(transform, false);
        }
    }

    public void GenerateCheese1() => GenerateCheese();

    public bool DoorTop { set { wallTop.SetActive(!value); } }
    public bool DoorBottom { set { wallBottom.SetActive(!value); } }
    public bool DoorLeft { set { wallLeft.SetActive(!value); } }
    public bool DoorRight { set { wallRight.SetActive(!value); } }

    public bool Locked {
        set {
            roomLock.SetActive(value);
            if (!value) {
                for (int i = 0; i < Random.Range(minCheese, maxCheese); i++)
                    Invoke(nameof(GenerateCheese1), i * 0.1f);
                InvokeRepeating(nameof(A), 10, 10);
            }
        }
    }
}

