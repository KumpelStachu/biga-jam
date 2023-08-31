using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using TMPro;

public class FollowMouseScript : MonoBehaviour {
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float speed = 1;
<<<<<<< HEAD
    [SerializeField] private int cheese_counter = 0;
    [SerializeField] private float distance = 10.05f;
    [SerializeField] private GameObject cheese_holder;
    [SerializeField] private GameObject trap_holder;
    [SerializeField] private bool is_stuned;
    [SerializeField] private Transform[] points;

    void Start() {
        InvokeRepeating("SpawnTrap", 3.0f, 3f);
    }

    void Update() {
=======
    [SerializeField] private int cheeseCounter = 0;
    [SerializeField] private bool isStuned;

    void FixedUpdate() {
>>>>>>> 09e601119b297e31e055617e2b3f9d40911b81ff
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isStuned) return;

        if (Input.GetMouseButton(0)) {
            var v = (transform.position - mouse) * speed * Time.deltaTime;
            rigidbody.velocity -= new Vector2(v.x, v.y);
            rigidbody.velocity.Normalize();
            //rigidbody.MovePosition(transform.position + mouse * speed * Time.deltaTime);
            //rigidbody.MovePosition(Vector2.MoveTowards(transform.position, mouse, speed * Time.deltaTime));
        }

        Quaternion rotation = Quaternion.LookRotation(mouse - transform.position, transform.TransformDirection(Vector3.back));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

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
        isStuned = true;
        Invoke(nameof(RemoveMouseStun), 2.0f);
    }
    public void RemoveMouseStun() {
        isStuned = false;
    }
    public void SpawnTrap()
    {
        int rand_num = Random.Range(0,4);
        GameObject trap_object = Instantiate(trap_holder, points[rand_num].position, points[rand_num].transform.rotation);
        
    }
}
