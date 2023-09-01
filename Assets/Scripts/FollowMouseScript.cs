using System.Linq;
using UnityEngine;

public class FollowMouseScript : MonoBehaviour {
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float speed = 1;
    [SerializeField] private float dashMultiplier = 5;
    [SerializeField] private float dashDuration = 2;
    [SerializeField] private int cheeseCounter = 0;
    [SerializeField] private bool isStunned;
    [SerializeField] private bool isDashing;
    [SerializeField] private GameObject trap_holder;
    [SerializeField] private Transform[] points;
    [SerializeField] private ParticleSystem mouse_particle;
    [SerializeField] private Animator mouse_animator;
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private float pointerDistance = 1.3f;

    public const int maxCheese = 3;

    void Start() {
        InvokeRepeating(nameof(SpawnTrap), 3, 4);
    }

    void Update() {
        if (isStunned) return;

        var pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouse_particle.Play();

        if (Vector2.Distance(pointer, transform.position) > pointerDistance)
            transform.position = Vector2.MoveTowards(transform.position, pointer, speed * Time.deltaTime);

        Quaternion rotation = Quaternion.LookRotation(pointer - transform.position, transform.TransformDirection(Vector3.back));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

        if (!isDashing && cheeseCounter == maxCheese && Input.GetKeyDown(KeyCode.Space)) {
            //rigidbody.velocity = Vector2.MoveTowards(transform.position, pointer, dashSpeed) - new Vector2(transform.position.x, transform.position.y);
            speed *= dashMultiplier;
            cheeseCounter = 0;
            isDashing = true;
            RemoveCheese();
            Invoke(nameof(CancelDashing), dashDuration);
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
            gameManagerScript.playerCheese++;
            gameManagerScript.CheeseBarHeal(10);
            gameManagerScript.UpdateCheeseCounter();
            gameManagerScript.AddScore(10);
        }
        else if (obj.CompareTag("Miotla")) {
            gameManagerScript.CheeseBarRemoveHeal(15);
            SetMouseToStun();
        }
    }

    public void CancelDashing() {
        isDashing = false;
        speed /= dashMultiplier;
    }

    public void RemoveCheese(int value = maxCheese) {
        foreach (var item in GetComponentsInChildren<Transform>().Where(child => child.CompareTag("cheese")).Take(value))
            Destroy(item.gameObject);
    }

    public void SetMouseToStun() {
        isStunned = true;
        mouse_animator.Play("Mouse_get_stuned");
        rigidbody.velocity *= 0.1f;
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
