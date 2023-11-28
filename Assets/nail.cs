using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nail : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    private float nailSpeed = 20f;
    private Bossmouse mouse;
    private int damage;

    void Start()
    {
        mouse = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Bossmouse>();
        damage = mouse.pattern_damage[2];
        if (mouse.phase_state == 2)
        {
            nailSpeed = 20f;
            damage = mouse.pattern_damage[2] + 10;
        }
       
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if ((target.position.x - transform.position.x) < 0)
            transform.Rotate(0f, 180f, 0f);

        rb = GetComponent<Rigidbody2D>();
        Vector3 targetVector = (target.position - transform.position).normalized;
        rb.velocity = targetVector * nailSpeed;
        Invoke("DestroyNail", 4f);
    }

    void DestroyNail()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHit player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHit>();
            player.Hit(damage);
            DestroyNail();
        }

    }
}
