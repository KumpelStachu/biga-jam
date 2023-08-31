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
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

        if (isStunned) return;

        transform.position = Vector2.MoveTowards(transform.position, mouse, speed * Time.deltaTime);
        mouse_particle.Play();

    }

    void OnTriggerEnter2D(Collider2D col) {
        var obj = col.gameObject;

        if (obj.CompareTag("roomLock")) {
            cheeseCounter = obj.GetComponent<RoomLockScript>().AddCheese(cheeseCounter);
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
        Invoke(nameof(RemoveMouseStun), 2.0f);
    }

    public void RemoveMouseStun() {
        isStunned = false;
    }

    public void SpawnTrap() {
        int rand_num = Random.Range(0, 4);
        GameObject trap_object = Instantiate(trap_holder, points[rand_num].position, points[rand_num].transform.rotation);

    }
    public void ClearCheese()
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
                {
                    if(transform.GetChild(i).gameObject.tag == "cheese")
                    {
                        GameObject holder_child = transform.GetChild(i).gameObject;
                        Destroy(holder_child);
                    }
                   
                }
    }
}
