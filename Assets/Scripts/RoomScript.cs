using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomScript : MonoBehaviour {
    [SerializeField] private Transform[] cheeseSpawnPoints;
    [SerializeField] private Transform[] powerUpSpawnPoints;
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
    [SerializeField] private float cheeseDelay = 10;
    [SerializeField] private float cheeseRate = 7;
    [SerializeField] private float powerUpDelay = 30;
    [SerializeField] private float powerUpRate = 20;
    [Range(0, 1)][SerializeField] private float roombaChance = 0.5f;
    [SerializeField] private float spawnDelay = 0.5f;

    private GameObject mouse;
    private GameObject[] powerUps;

    public void Awake() {
        powerUps = FindChildrenWithTag(Tag.PowerUp).Select(e => e.gameObject).ToArray();
        foreach (var powerUp in powerUps) powerUp.SetActive(false);
    }

    public void Start() {
        mouse = GameObject.FindGameObjectWithTag(Tag.Mouse);

        foreach (var item in optionalDecoration.OrderBy(_ => Random.value).Take(Random.Range(optionalDecoration.Length / 4, optionalDecoration.Length * 3 / 4)))
            Destroy(item);
    }

    public void OnDrawGizmos() {
        if (mouse == null || Vector2.Distance(transform.position, mouse.transform.position) > maxDistance) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, mouse.transform.position);
    }

    private void GenerateCheeseIfPlayerIsCloseEnough() {
        if (Vector2.Distance(transform.position, mouse.transform.position) > maxDistance) return;
        GenerateCheese();
    }

    public void GenerateCheese(int count = 1) {
        StartCoroutine(nameof(GenerateCheeseInner), count);
    }

    private IEnumerator GenerateCheeseInner(int count = 1) {
        foreach (var spawnPoint in RandomFreeSpaces(cheeseSpawnPoints, FindTakenPositions(Tag.Cheese), count)) {
            var cheese = Instantiate(cheesePrefab, spawnPoint.localPosition, Quaternion.identity);

            cheese.transform.Rotate(0, 0, Random.Range(0f, 360f));
            cheese.transform.SetParent(transform, false);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerable<Transform> RandomFreeSpaces(IEnumerable<Transform> list, IEnumerable<Vector3> taken, int count = 1) =>
        list.Where(e => taken.FirstOrDefault(t => e.position == t) == Vector3.zero).OrderBy(_ => Random.value).Take(count);

    public IEnumerable<Vector3> FindTakenPositions(string tag) => FindChildrenWithTag(tag).Select(e => e.transform.position);

    public IEnumerable<Transform> FindChildrenWithTag(string tag) => Children.Where(e => e.CompareTag(tag));

    public List<Transform> Children {
        get {
            List<Transform> list = new();
            foreach (Transform item in transform) list.Add(item);
            return list;
        }
    }

    private IEnumerator ShowPowerUps() {
        yield return new WaitForSeconds(spawnDelay * 2.5f);

        foreach (var powerUp in powerUps) {
            powerUp.SetActive(true);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void GenerateOneCheese() => GenerateCheese();

    public bool DoorTop { set { wallTop.SetActive(!value); } }
    public bool DoorBottom { set { wallBottom.SetActive(!value); } }
    public bool DoorLeft { set { wallLeft.SetActive(!value); } }
    public bool DoorRight { set { wallRight.SetActive(!value); } }

    public bool Locked {
        get { return roomLock.activeSelf; }
        set {
            roomLock.SetActive(value);

            if (value) return;

            GenerateCheese(Random.Range(minCheese, maxCheese));
            //for (int i = 0; i < Random.Range(minCheese, maxCheese); i++)
            //    Invoke(nameof(GenerateOneCheese), i * spawnDelay);

            InvokeRepeating(nameof(GenerateCheeseIfPlayerIsCloseEnough), cheeseDelay, cheeseRate);
            StartCoroutine(nameof(ShowPowerUps));

            var roomba = Children.Find(e => e.CompareTag(Tag.Roomba)).gameObject;
            if (Random.value > roombaChance)
                roomba.SetActive(true);
            else Destroy(roomba);
        }
    }
}

