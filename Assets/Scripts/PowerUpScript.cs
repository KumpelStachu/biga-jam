using System;
using UnityEngine;

public enum PowerUpType {
    Speed,
    Health,
    Timer,
    Points,
    Shield,
}

public class PowerUpScript : MonoBehaviour {
    [SerializeField] private PowerUpType type;
    [SerializeField] private Sprite[] holders;
    [SerializeField] private Sprite[] icons;

    void Start() {
        GetComponent<SpriteRenderer>().sprite = holders[(int)type];
        GetComponentInChildren<SpriteRenderer>().sprite = icons[(int)type];
    }

    void Update() {

    }

    public void OnValidate() {
        var types = Enum.GetValues(typeof(PowerUpType));

        if (holders.Length != types.Length) throw new Exception("sprawdŸ listê holderów");
        if (icons.Length != types.Length) throw new Exception("sprawdŸ listê iconsów");
    }
}
