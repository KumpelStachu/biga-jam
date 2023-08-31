using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_script : MonoBehaviour
{
    [SerializeField] public FollowMouseScript followMouseScript;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Mouse")
        {
            followMouseScript.SetMouseToStun();
            Destroy(gameObject);
        }
       
    }
}
