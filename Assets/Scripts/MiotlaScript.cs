using UnityEngine;

public class MiotlaScript : MonoBehaviour {
    [SerializeField] private float miotlaSpeed = 10f;
    [SerializeField] private Rigidbody2D miotlaRb;

    public int Dir {
        set {
            miotlaRb.velocity = new Vector2(miotlaSpeed * value, 0);
            transform.localScale = new Vector3(-value, transform.localScale.y, transform.localScale.z);
        }
    }

    void Start() {
        Invoke(nameof(DestroyMiotla), 6f);
    }

    void DestroyMiotla() {
        Destroy(gameObject);
    }
}
