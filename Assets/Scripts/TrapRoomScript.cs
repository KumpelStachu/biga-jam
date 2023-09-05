using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoomScript : MonoBehaviour {
    [SerializeField] private float roomDuration = 10;
    [SerializeField] private GameObject statuePrefab;
    [SerializeField] private GameObject roombaPrefab;
    [SerializeField] private Transform[] roombaSpawnPoints;
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private Transform[] powerUpSpawnPoints;
    [SerializeField] private float spawnDelay = 0.2f;

    private AudioManagerScript audioManager;
    private GameManager gameManager;
    private RoomScript roomScript;
    private StatueScript statueScript;
    private bool left, right, top, bottom;
    private List<GameObject> roombas = new();

    void Start() {
        roomScript = GetComponent<RoomScript>();
        audioManager = FindObjectOfType<AudioManagerScript>();
        gameManager = GameObject.FindGameObjectWithTag(Tag.GameManager).GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag(Tag.Mouse)) return;

        CloseRoom();
        GetComponent<BoxCollider2D>().enabled = false;
        Invoke(nameof(OpenRoom), roomDuration);
        StartCoroutine(nameof(SpawnRoombas));

        statueScript = Instantiate(statuePrefab, transform, false).GetComponent<StatueScript>();
    }

    public void CloseRoom() {
        left = roomScript.DoorLeft;
        right = roomScript.DoorRight;
        top = roomScript.DoorTop;
        bottom = roomScript.DoorBottom;

        roomScript.DoorLeft = false;
        roomScript.DoorRight = false;
        roomScript.DoorTop = false;
        roomScript.DoorBottom = false;
    }

    public void OpenRoom() {
        roomScript.DoorLeft = left;
        roomScript.DoorRight = right;
        roomScript.DoorTop = top;
        roomScript.DoorBottom = bottom;

        statueScript.Disable();
        gameManager.AddScore(statueScript.cheese * 10);
        audioManager.Play("door");

        StartCoroutine(nameof(ShowPowerUps));

        foreach (var roomba in roombas) Destroy(roomba);
    }

    private IEnumerator ShowPowerUps() {
        foreach (var spawnPoint in powerUpSpawnPoints) {
            yield return new WaitForSeconds(spawnDelay);

            var powerUp = Instantiate(powerUpPrefab, spawnPoint.localPosition, Quaternion.identity);
            powerUp.transform.SetParent(transform, false);
        }
    }

    private IEnumerator SpawnRoombas() {
        foreach (var spawnPoint in roombaSpawnPoints) {
            yield return new WaitForSeconds(spawnDelay);

            var roomba = Instantiate(roombaPrefab, spawnPoint.localPosition, Quaternion.identity);
            roomba.transform.SetParent(transform, false);
            roomba.SetActive(true);
            roombas.Add(roomba);
        }
    }
}
