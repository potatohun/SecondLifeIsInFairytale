using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody2D coinRigid;
    private  float coinSpeed = 8f;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        coinRigid = GetComponent<Rigidbody2D>();
        Vector3 targetVector = (playerTransform.position + new Vector3(0f, 1f, 0f) - transform.position).normalized;
        coinRigid.velocity = targetVector * coinSpeed;
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
