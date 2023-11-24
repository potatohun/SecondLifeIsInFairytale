using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nail : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody2D rb;
    private float coinSpeed = 10f;
    private SpriteRenderer render;
    private Bossmouse mouse;
    private float damage;

    void Start()
    {
        mouse = GameObject.FindGameObjectWithTag("Bossrat").GetComponent<Bossmouse>();
        damage = mouse.pattern_damage[2];
        if (mouse.phase_state == 2)
        {
            coinSpeed = 15f;
            damage = mouse.pattern_damage[2] + 10f;
        }
        
        render = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        DirectionEnemy(playerTransform.position.x, transform.position.x);
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
            Debug.Log("player에게 coin 맞춤");
            DestroyCoin();
        }

    }

    void DirectionEnemy(float target, float baseobj) // render 좌우 적을 향하도록 조절
    {
        if (target < baseobj)
            render.flipX = true;
        else
            render.flipX = false;
    }
}
