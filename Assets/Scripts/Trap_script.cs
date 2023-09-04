using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_script : MonoBehaviour {
    [SerializeField] private FollowMouseScript followMouseScript;
    [SerializeField] private float trapCooldown;
    [SerializeField] private float trapSuicide;
    [SerializeField] private bool isReady;
    [SerializeField] private Animator trapAnimator;
    [SerializeField] private AudioSource audioSource;

    void Start() {
        followMouseScript = GameObject.FindGameObjectWithTag(Tag.Mouse).GetComponent<FollowMouseScript>();
        Invoke(nameof(SetCoolDownOff), trapCooldown);
        Invoke(nameof(SuicideTrap), trapSuicide);
        trapAnimator.Play("Mouse_trap_drop");
    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.CompareTag(Tag.Mouse) && isReady) {
            isReady = false;
            followMouseScript.SetMouseToStun();
            trapAnimator.Play("Mouse_trap_turn_on");
            audioSource.Play();
            Invoke(nameof(DestroyTrap), 1f);
        }
    }

    void SetCoolDownOff() {
        isReady = true;
    }

    void SuicideTrap() {
        trapAnimator.Play("Mouse_trap_turn_on");
        Invoke(nameof(DestroyTrap), 0.25f);
    }

    void DestroyTrap() {
        Destroy(gameObject);
    }
}
