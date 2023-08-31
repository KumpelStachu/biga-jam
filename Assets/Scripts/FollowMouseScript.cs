using UnityEngine;

public class FollowMouseScript : MonoBehaviour {
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float speed = 1;
    [SerializeField] private int cheeseCounter = 0;
    [SerializeField] private bool isStuned;

    void Update() {
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isStuned) return;

        Quaternion rotation = Quaternion.LookRotation(mouse - transform.position, transform.TransformDirection(Vector3.back));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

        if (Input.GetMouseButton(0)) {
            //var v = (speed * Time.deltaTime * (transform.position - mouse)).normalized;
            //rigidbody.velocity -= new Vector2(v.x, v.y);
            //rigidbody.velocity.Normalize();
            //rigidbody.MovePosition(transform.position - v);
            //rigidbody.MovePosition(Vector2.MoveTowards(transform.position, mouse, speed * Time.deltaTime));
            rigidbody.AddRelativeForce(speed * Time.deltaTime * Vector3.forward, ForceMode2D.Force);
        }


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
        rigidbody.velocity *= 0.1f;
        Invoke(nameof(RemoveMouseStun), 2.0f);
    }

    public void RemoveMouseStun() {
        isStuned = false;
    }
}
