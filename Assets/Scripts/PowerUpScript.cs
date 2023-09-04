using UnityEngine;

public enum PowerUpType {
    Speed,
    Timer,
    Points,
    Shield,
}

public class PowerUpScript : MonoBehaviour {
    [SerializeField] private bool randomType = true;
    [SerializeField] private PowerUpType type;
    [SerializeField] private Sprite[] holders;
    [SerializeField] private Sprite[] icons;
    [SerializeField] private SpriteRenderer holderRenderer, iconRenderer;
    [SerializeField] private Animator animator;

    private CheeseBarScript cheeseBarScript;
    private FollowMouseScript mouseScript;
    private GameManager gameManager;

    void Start() {
        if (randomType) {
            var types = PowerUpTypes();
            type = types[Random.Range(0, types.Length)];
        }

        holderRenderer.sprite = holders[(int)type];
        iconRenderer.sprite = icons[(int)type];

        cheeseBarScript = GameObject.FindGameObjectWithTag(Tag.CheeseBar).GetComponent<CheeseBarScript>();
        mouseScript = GameObject.FindGameObjectWithTag(Tag.Mouse).GetComponent<FollowMouseScript>();
        gameManager = GameObject.FindGameObjectWithTag(Tag.GameManager).GetComponent<GameManager>();
    }

    private PowerUpType[] PowerUpTypes() => (PowerUpType[])System.Enum.GetValues(typeof(PowerUpType));

    public void OnValidate() {
        var types = PowerUpTypes();

        if (holders.Length != types.Length) throw new UnityException("sprawdŸ listê holderów");
        if (icons.Length != types.Length) throw new UnityException("sprawdŸ listê iconsów");
    }

    public void Activate() {
        if (!mouseScript.CanIPower(type)) return;

        switch (type) {
            case PowerUpType.Speed:
                mouseScript.SpeeedUp();
                break;
            case PowerUpType.Timer:
                cheeseBarScript.Heal(cheeseBarScript.MaxHealth / 2);
                break;
            case PowerUpType.Points:
                gameManager.AddScore(100);
                break;
            case PowerUpType.Shield:
                mouseScript.GodMode();
                break;
            default: throw new UnityException("AAAAAAAAAAAAAAA!!!!!!!");
        }

        animator.Play("PowerUpExit");
        GetComponent<CircleCollider2D>().enabled = false;
        Invoke(nameof(CommitSuicide), 1 / 3f);
    }

    public void CommitSuicide() {
        Destroy(gameObject);
    }

}
