using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doryang_throwObject : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody2D rb;
    private float objectSpeed = 20f;
    public int damage;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        Vector3 targetVector = (playerTransform.position - transform.position).normalized;
        rb.velocity = targetVector * objectSpeed;
        Invoke("DestroyObject", 3f);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHit player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHit>();
            player.Hit(damage);
            DestroyObject();
        }

    }
}
