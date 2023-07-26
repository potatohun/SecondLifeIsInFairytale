using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public int jumpPower;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector2.up * jumpPower,ForceMode2D.Impulse);
        }
    }
}
