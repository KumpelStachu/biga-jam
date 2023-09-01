using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiotlaScript : MonoBehaviour
{
    [SerializeField] private float miotlaSpeed = 10f;
    [SerializeField] private Rigidbody2D miotlaRb;
    private Vector2 ScreenBounds;
    void Start()
    {
        miotlaRb.velocity = new Vector2(-miotlaSpeed, 0);
        Invoke("DestroyMiotla", 6f);
    }

    
    void Update()
    {
       
    }
    void SpawnMiotla()
    {
       
    }
    void DestroyMiotla()
    {
        Destroy(this.gameObject);
    }
    

}
