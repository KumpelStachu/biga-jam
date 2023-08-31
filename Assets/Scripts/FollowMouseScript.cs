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

    void Start() {
        InvokeRepeating(nameof(SpawnTrap), 3.0f, 3f);
    }

    void Update() {
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isStunned) return;

        Quaternion rotation = Quaternion.LookRotation(mouse - transform.position, transform.TransformDirection(Vector3.back));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

        if (isStunned) return;

        transform.position = Vector2.MoveTowards(transform.position, mouse, speed * Time.deltaTime);
        mouse_particle.Play();

    }

    void OnTriggerEnter2D(Collider2D col) {
        if (cheeseCounter >= 3) return;
        if (!col.gameObject.CompareTag("cheese")) return;

        var cheese = col.gameObject;
        cheese.transform.SetParent(transform, false);
        cheese.transform.position = transform.position;
        cheeseCounter++;

    }
    public void SetMouseToStun() {
        //stun !
        isStunned = true;
        mouse_animator.Play("Mouse_get_stuned");
        rigidbody.velocity *= 0.1f;
        Invoke(nameof(RemoveMouseStun), 2.0f);
    }

    public void RemoveMouseStun() {
        isStunned = false;
    }

    public void SpawnTrap() {
        int rand_num = Random.Range(0, 4);
        GameObject trap_object = Instantiate(trap_holder, points[rand_num].position, points[rand_num].transform.rotation);

    }
}
