using UnityEngine;

public class PopScript : MonoBehaviour {
    [SerializeField] private float maxTime = 1f;

    private float time;

    public void Start() {
        Invoke(nameof(Suicide), maxTime);
    }

    public void Update() {
        time += Time.deltaTime;
    }

    public void OnBecameVisible() {
        if (time <= maxTime) Invoke(nameof(Pop), time);
    }

    public void Suicide() {
        Destroy(this);
    }

    public void Pop() {
        FindObjectOfType<AudioManagerScript>().Play("pop");
    }
}
