using System.Collections;
using System.Linq;
using UnityEngine;

public class FollowMouseScript : MonoBehaviour {
    [SerializeField] private new Rigidbody2D rigidbody;
    [SerializeField] private float speed = 300;
    [SerializeField] private float dashMultiplier = 3;
    [SerializeField] private float dashDuration = 1;
    [SerializeField] public int cheeseCounter = 0;
    [SerializeField] private bool isStunned;
    [SerializeField] private bool isDashing;
    [SerializeField] private GameObject trap_holder;
    [SerializeField] private Transform[] points;
    [SerializeField] private ParticleSystem mouse_particle;
    [SerializeField] private Animator mouse_animator;
    [SerializeField] private GameManager gameManagerScript;
    [SerializeField] private float pointerDistance = 0.1f;
    [SerializeField] private float speeedUpTime = 5;
    [SerializeField] private float speeedUpMultiplier = 2;
    [SerializeField] private float godModeTime = 5;
    [SerializeField] private float trapElevator = 10;
    [SerializeField] private float trapDelay = 5;
    [SerializeField] private float trapMultiplier = 0.975f;

    public const int maxCheese = 3;
    private Vector2 moveDirection = Vector2.zero;
    public bool isGod, isSpeeed;

    void Start() {
        StartCoroutine(nameof(SpawnTrap));
    }

    void Update() {
        if (isStunned) return;

        mouse_particle.Play();

        if (!isDashing && cheeseCounter == maxCheese && Input.GetKeyDown(KeyCode.Space)) {
            cheeseCounter = 0;
            isDashing = true;
            RemoveCheese();
            Invoke(nameof(CancelDashing), dashDuration);
        }
    }

    private void FixedUpdate() {
        if (isStunned) return;

        var pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var speeed = isSpeeed ? speeedUpMultiplier : 1;

        if (isDashing) {
            rigidbody.velocity = dashMultiplier * speed * speeed * Time.deltaTime * moveDirection;
        }
        else {
            moveDirection = (new Vector2(pointer.x, pointer.y) - rigidbody.position).normalized;

            if (Vector2.Distance(rigidbody.position, pointer) > pointerDistance) {
                rigidbody.velocity = speed * speeed * Time.deltaTime * moveDirection;
            }
            else {
                rigidbody.velocity = Vector2.zero;
            }

        }
        if (moveDirection != Vector2.zero) {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            rigidbody.rotation = angle - 90;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        var obj = col.gameObject;

        if (obj.CompareTag(Tag.RoomLock)) {
            cheeseCounter = obj.GetComponent<RoomLockScript>().AddCheese(cheeseCounter);
            gameManagerScript.UpdateCheeseCounter();
            RemoveCheese(maxCheese - cheeseCounter);
        }
        else if (obj.CompareTag(Tag.Cheese) && cheeseCounter < maxCheese) {
            obj.transform.SetParent(transform, false);
            obj.transform.position = transform.position;
            obj.GetComponent<PolygonCollider2D>().enabled = false;
            cheeseCounter++;
            gameManagerScript.CheeseBarHeal(10);
            gameManagerScript.UpdateCheeseCounter();
            gameManagerScript.AddScore(10);

        }
        else if (obj.CompareTag(Tag.Miotla) && !isStunned && !isGod) {
            gameManagerScript.CheeseBarRemoveHeal(15);
            SetMouseToStun();
        }
        else if (obj.CompareTag(Tag.Roomba) && !isStunned && !isGod && obj.GetComponent<RoombaScript>().Roomb()) {
            gameManagerScript.CheeseBarRemoveHeal(5);
            SetMouseToStun();
        }
        else if (obj.CompareTag(Tag.PowerUp)) {
            obj.GetComponent<PowerUpScript>().Activate();
        }
    }

    public bool CanIPower(PowerUpType type) {
        if (isStunned) return false;

        if (type == PowerUpType.Speed) return !isSpeeed;
        if (type == PowerUpType.Shield) return !isGod;

        return true;
    }

    public void SpeeedUp() {
        isSpeeed = true;

        gameManagerScript.ShowSpeeed(speeedUpTime);
        Invoke(nameof(SpedDown), speeedUpTime);
    }

    public void GodMode() {
        isGod = true;

        gameManagerScript.ShowGod(godModeTime);
        Invoke(nameof(MouseMode), godModeTime);
    }

    public void SpedDown() {
        isSpeeed = false;
    }

    public void MouseMode() {
        isGod = false;
    }

    public void CancelDashing() {
        isDashing = false;
    }

    public void RemoveCheese(int value = maxCheese) {
        foreach (var item in GetComponentsInChildren<Transform>().Where(child => child.CompareTag(Tag.Cheese)).Take(value))
            Destroy(item.gameObject);
    }

    public void SetMouseToStun() {
        if (isGod || isStunned) return;
        isStunned = true;
        mouse_animator.Play(Animations.MouseGetStuned);
        rigidbody.velocity = Vector2.zero;
        CancelInvoke(nameof(RemoveMouseStun));
        Invoke(nameof(RemoveMouseStun), 1.41f);
    }

    public void RemoveMouseStun() {
        isStunned = false;
    }

    public IEnumerator SpawnTrap() {
        yield return new WaitForSeconds(trapElevator);

        while (true) {
            var point = points.ElementAt(Random.Range(0, points.Length));
            Instantiate(trap_holder, point.position, point.transform.rotation);

            yield return new WaitForSeconds(trapDelay);
            trapDelay *= trapMultiplier;
        }
    }
}
