using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_script : MonoBehaviour
{
    [SerializeField] private FollowMouseScript followMouseScript;
    [SerializeField] private float trap_cooldown;
    [SerializeField] private bool is_ready;
    void Start()
    {
        followMouseScript = GameObject.FindGameObjectWithTag("Mouse").GetComponent<FollowMouseScript>();
        Invoke("SetCoolDownOff", trap_cooldown);
    }

    
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Mouse" && is_ready == true)
        {
            followMouseScript.SetMouseToStun();
            Destroy(gameObject);
        }
       
    }
    void SetCoolDownOff()
    {
        is_ready = true;
    }
}
