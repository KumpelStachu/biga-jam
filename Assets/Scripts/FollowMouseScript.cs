using System.Linq;
using UnityEngine;

public class FollowMouseScript : MonoBehaviour {
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float speed = 1;
    [SerializeField] private int cheeseCounter = 0;
    [SerializeField] private bool isStunned;
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
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isStunned) return;

        mouse_particle.Play();

        if (Vector2.Distance(mouse, transform.position) > pointerDistance)
            transform.position = Vector2.MoveTowards(transform.position, mouse, speed * Time.deltaTime);

        Quaternion rotation = Quaternion.LookRotation(mouse - transform.position, transform.TransformDirection(Vector3.back));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

    void OnTriggerEnter2D(Collider2D col) {
        var obj = col.gameObject;

        if (obj.CompareTag("roomLock")) {
            cheeseCounter = obj.GetComponent<RoomLockScript>().AddCheese(cheeseCounter);

            foreach (var item in GetComponentsInChildren<Transform>().Where(child => child.CompareTag("cheese")).Take(maxCheese - cheeseCounter))
                Destroy(item.gameObject);

            gameManagerScript.UpdateCheeseCounter();
        }
        else if (obj.CompareTag("cheese") && cheeseCounter < maxCheese) {
            obj.transform.SetParent(transform, false);
            obj.transform.position = transform.position;
            obj.GetComponent<CircleCollider2D>().enabled = false;
            cheeseCounter++;
            gameManagerScript.playerCheese++;
            gameManagerScript.CheeseBarHeal();
            gameManagerScript.UpdateCheeseCounter();
            gameManagerScript.AddScore(10);
        }

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
