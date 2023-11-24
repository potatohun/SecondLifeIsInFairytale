using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doryang_throwObject : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody2D rb;
    private float coinSpeed = 10f;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        Vector3 targetVector = (playerTransform.position - transform.position).normalized;
        rb.velocity = targetVector * coinSpeed;
        Invoke("DestroyCoin", 3f);
    }

    void DestroyCoin()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("player¿¡°Ô coin ¸ÂÃã");
            DestroyCoin();
        }

    }
}
