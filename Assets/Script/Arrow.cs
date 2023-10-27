using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 3f;
    private Transform target;
    private Vector3 direction;
    private SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        direction = new Vector3(target.position.x - transform.position.x, 0f, 0f).normalized;
        if (target.position.x - transform.position.x > 0)
        {
            render.flipX = false;
        }
        else
            render.flipX = true;

        Invoke("DestroyArrow", 6f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("player¿¡°Ô arrow ¸ÂÃã");
            DestroyArrow();

        }

    }
}
