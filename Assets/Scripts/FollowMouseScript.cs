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

    void Start() {
        InvokeRepeating(nameof(SpawnTrap), 3.0f, 4f);
    }

    void Update() {
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isStunned) return;

        Quaternion rotation = Quaternion.LookRotation(mouse - transform.position, transform.TransformDirection(Vector3.back));
        Debug.Log(rotation);
        transform.SetPositionAndRotation(Vector2.MoveTowards(transform.position, mouse, speed * Time.deltaTime), new Quaternion(0, 0, rotation.z, rotation.w));

        mouse_particle.Play();
    }

    void OnTriggerEnter2D(Collider2D col) {
        var obj = col.gameObject;

        if (obj.CompareTag("roomLock")) {
            cheeseCounter = obj.GetComponent<RoomLockScript>().AddCheese(cheeseCounter);

            foreach (var item in GetComponentsInChildren<Transform>().Where(child => child.CompareTag("cheese")).Take(3 - cheeseCounter))
                Destroy(item.gameObject);
        }
        else if (obj.CompareTag("cheese") && cheeseCounter < 3) {
            obj.transform.SetParent(transform, false);
            obj.transform.position = transform.position;
            obj.GetComponent<CircleCollider2D>().enabled = false;
            cheeseCounter++;
        }

    }
    public void SetMouseToStun() {
        //stun !
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
        int rand_num = Random.Range(0, 4);
        GameObject trap_object = Instantiate(trap_holder, points[rand_num].position, points[rand_num].transform.rotation);

    }
}
