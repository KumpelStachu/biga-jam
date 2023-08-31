using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_script : MonoBehaviour {
    [SerializeField] private FollowMouseScript followMouseScript;
    [SerializeField] private float trap_cooldown;
    [SerializeField] private bool is_ready;
    [SerializeField] private Animator trap_animator;

    void Start() {
        followMouseScript = GameObject.FindGameObjectWithTag("Mouse").GetComponent<FollowMouseScript>();
        Invoke(nameof(SetCoolDownOff), trap_cooldown);
        trap_animator.Play("Mouse_trap_drop");
    }

    void Update() {

    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.CompareTag("Mouse") && is_ready == true) {
            followMouseScript.SetMouseToStun();
            trap_animator.Play("Mouse_trap_turn_on");
            Invoke("DestroyTrap", 1f);
        }
    }

    void SetCoolDownOff() {
        is_ready = true;
    }
    void DestroyTrap()
    {
        Destroy(gameObject);
    }
}
