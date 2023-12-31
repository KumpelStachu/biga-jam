using UnityEngine;

public class Trap_script : MonoBehaviour {
    [SerializeField] private float trapCooldown;
    [SerializeField] private float trapSuicide;
    [SerializeField] private bool isReady;
    [SerializeField] private Animator trapAnimator;

    private FollowMouseScript followMouseScript;
    private AudioManagerScript audioManger;

    void Start() {
        followMouseScript = GameObject.FindGameObjectWithTag(Tag.Mouse).GetComponent<FollowMouseScript>();
        audioManger = FindObjectOfType<AudioManagerScript>();
        Invoke(nameof(SetCoolDownOff), trapCooldown);
        Invoke(nameof(SuicideTrap), trapSuicide);
        trapAnimator.Play(Animations.MouseTrapDrop);
    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.CompareTag(Tag.Mouse) && isReady) {
            isReady = false;
            followMouseScript.SetMouseToStun();
            trapAnimator.Play(Animations.MouseTrapTurnOn);
            audioManger.Play("trap");
            Invoke(nameof(DestroyTrap), 1f);
        }
    }

    void SetCoolDownOff() {
        isReady = true;
    }

    void SuicideTrap() {
        trapAnimator.Play(Animations.MouseTrapTurnOn);
        Invoke(nameof(DestroyTrap), 0.25f);
    }

    void DestroyTrap() {
        Destroy(gameObject);
    }
}
