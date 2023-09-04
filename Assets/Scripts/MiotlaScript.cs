using UnityEngine;

public class MiotlaScript : MonoBehaviour {
    [SerializeField] private float miotlaSpeed = 10f;
    [SerializeField] private Rigidbody2D miotlaRb;
    [SerializeField] private float lifespan = 6f;

    public int Dir {
        set {
            miotlaRb.velocity = new Vector2(miotlaSpeed * value, 0);
            transform.localScale = new Vector3(-value, transform.localScale.y, transform.localScale.z);
        }
    }

    void Start() {
        FindObjectOfType<AudioManagerScript>().Play("szur");
        Invoke(nameof(DestroyMiotla), lifespan);
    }

    void DestroyMiotla() {
        Destroy(gameObject);
    }
}
