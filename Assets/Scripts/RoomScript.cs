using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomScript : MonoBehaviour {
    [SerializeField] private Transform[] cheeseSpawnPoints;
    [SerializeField] private Transform[] powerUpSpawnPoints;
    [SerializeField] private GameObject[] optionalDecoration;
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private GameObject cheesePrefab;
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject wallTop;
    [SerializeField] private GameObject wallBottom;
    [SerializeField] private GameObject wallLeft;
    [SerializeField] private GameObject wallRight;
    [SerializeField] private GameObject roomLock;
    [SerializeField] private int minCheese = 5;
    [SerializeField] private int maxCheese = 10;
    [SerializeField] private float maxDistance = 20;
    [SerializeField] private float cheeseDelay = 10;
    [SerializeField] private float cheeseRate = 7;
    [Range(0, 1)][SerializeField] private float roombaChance = 0.5f;
    [SerializeField] private float spawnDelay = 0.5f;

    private GameObject mouse;
    private AudioManagerScript audioManager;

    public void Start() {
        audioManager = FindObjectOfType<AudioManagerScript>();
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
        if (cheeseSpawnPoints == null || cheeseSpawnPoints.Length == 0) return;
        StartCoroutine(nameof(GenerateCheeseInner), count);
    }

    private IEnumerator GenerateCheeseInner(int count = 1) {
        foreach (var spawnPoint in RandomFreeSpaces(cheeseSpawnPoints, FindTakenPositions(Tag.Cheese), count)) {
            yield return new WaitForSeconds(spawnDelay);

            var cheese = Instantiate(cheesePrefab, spawnPoint.localPosition, Quaternion.identity);

            cheese.transform.Rotate(0, 0, Random.Range(0f, 360f));
            cheese.transform.SetParent(transform, false);
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
        foreach (var spawnPoint in powerUpSpawnPoints) {
            yield return new WaitForSeconds(spawnDelay);

            var powerUp = Instantiate(powerUpPrefab, spawnPoint.localPosition, Quaternion.identity);
            powerUp.transform.SetParent(transform, false);
        }
    }

    private void DealWithRoomba() {
        var roomba = Children.Find(e => e.CompareTag(Tag.Roomba));

        if (roomba == null) return;

        if (Random.value < roombaChance)
            roomba.gameObject.SetActive(true);
        else Destroy(roomba.gameObject);
    }

    public void GenerateOneCheese() => GenerateCheese();

    public bool DoorTop { get { return !wallTop.activeInHierarchy; } set { wallTop.SetActive(!value); } }
    public bool DoorBottom { get { return !wallBottom.activeInHierarchy; } set { wallBottom.SetActive(!value); } }
    public bool DoorLeft { get { return !wallLeft.activeInHierarchy; } set { wallLeft.SetActive(!value); } }
    public bool DoorRight { get { return !wallRight.activeInHierarchy; } set { wallRight.SetActive(!value); } }

    public bool Locked {
        get { return roomLock.activeSelf; }
        set {
            roomLock.SetActive(value);

            if (value) return;

            GenerateCheese(Random.Range(minCheese, maxCheese));
            InvokeRepeating(nameof(GenerateCheeseIfPlayerIsCloseEnough), cheeseDelay, cheeseRate);
            StartCoroutine(nameof(ShowPowerUps));
            DealWithRoomba();
        }
    }
}

