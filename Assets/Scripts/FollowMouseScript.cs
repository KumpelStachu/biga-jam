using System.Linq;
using TMPro;
using UnityEngine;

public class FollowMouseScript : MonoBehaviour {
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float speed = 300;
    [SerializeField] private float dashMultiplier = 3;
    [SerializeField] private float dashDuration = 1;
    [SerializeField] public int cheeseCounter = 0;
    [SerializeField] private bool isStunned;
    [SerializeField] private bool isDashing;
    [SerializeField] private GameObject trap_holder;
    [SerializeField] private Transform[] points;
    [SerializeField] private ParticleSystem mouse_particle;
    [SerializeField] private Animator mouse_animator;
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private float pointerDistance = 0.1f;

    public const int maxCheese = 3;
    private Vector2 moveDirection = Vector2.zero;

    void Start() {
        InvokeRepeating(nameof(SpawnTrap), 3, 4);
    }

    void Update() {
        if (isStunned) return;

        mouse_particle.Play();

        if (!isDashing && cheeseCounter == maxCheese && Input.GetKeyDown(KeyCode.Space)) {
            cheeseCounter = 0;
            isDashing = true;
            RemoveCheese();
            Invoke(nameof(CancelDashing), dashDuration);
        }
    }

    private void FixedUpdate() {
        if (isStunned) return;

        var pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isDashing) {
            rigidbody.velocity = dashMultiplier * speed * Time.deltaTime * moveDirection;
        }
        else {
            moveDirection = (new Vector2(pointer.x, pointer.y) - rigidbody.position).normalized;

            if (Vector2.Distance(rigidbody.position, pointer) > pointerDistance) {
                rigidbody.velocity = speed * Time.deltaTime * moveDirection;
            }
            else {
                rigidbody.velocity = Vector2.zero;
            }

        }
        if (moveDirection != Vector2.zero) {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            rigidbody.rotation = angle - 90;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        var obj = col.gameObject;

        if (obj.CompareTag("roomLock")) {
            cheeseCounter = obj.GetComponent<RoomLockScript>().AddCheese(cheeseCounter);
            gameManagerScript.UpdateCheeseCounter();
            RemoveCheese(maxCheese - cheeseCounter);
        }
        else if (obj.CompareTag("cheese") && cheeseCounter < maxCheese) {
            obj.transform.SetParent(transform, false);
            obj.transform.position = transform.position;
            obj.GetComponent<CircleCollider2D>().enabled = false;
            cheeseCounter++;
            gameManagerScript.CheeseBarHeal(10);
            gameManagerScript.UpdateCheeseCounter();
            gameManagerScript.AddScore(10);
        }
        else if (obj.CompareTag("Miotla")) {
            gameManagerScript.CheeseBarRemoveHeal(15);
            SetMouseToStun();
        }
        else if (obj.CompareTag("roomba") && obj.GetComponent<RoombaScript>().Roomb()) {
            gameManagerScript.CheeseBarRemoveHeal(5);
            SetMouseToStun();
        }
    }

    public void CancelDashing() {
        isDashing = false;
    }

    public void RemoveCheese(int value = maxCheese) {
        foreach (var item in GetComponentsInChildren<Transform>().Where(child => child.CompareTag("cheese")).Take(value))
            Destroy(item.gameObject);
    }

    public void SetMouseToStun() {
        isStunned = true;
        mouse_animator.Play("Mouse_get_stuned");
        rigidbody.velocity = Vector2.zero;
        CancelInvoke(nameof(RemoveMouseStun));
        Invoke(nameof(RemoveMouseStun), 1.41f);
    }

    public void RemoveMouseStun() {
        isStunned = false;
    }

    public void SpawnTrap() {
        var point = points.ElementAt(Random.Range(0, points.Length));
        Instantiate(trap_holder, point.position, point.transform.rotation);
    }
}
