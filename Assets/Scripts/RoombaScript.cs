using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D roombaRb;
    void Start()
    {
        roombaRb.AddForce(new Vector2(20*Time.deltaTime*1, 20*Time.deltaTime*1));
    }

    
    void Update()
    {
        
    }
}
