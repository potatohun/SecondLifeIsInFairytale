using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public float explosionRadius = 1f; // 폭발 범위
    private Rigidbody2D rb;
    private Animator ani;

    public GameObject pos;
    public float size;
    private bool isExploded;
    private Transform playerTransform; // 플레이어의 Transform

    float distance;
    bool isRight;
    float gravityScale = 1f;
    
    void Start()
    {
        isExploded = false;
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어를 찾아서 Transform 가져오기
        isRight = playerTransform.position.x > transform.position.x;
        distance = Mathf.Sqrt((playerTransform.position.x - transform.position.x)*(playerTransform.position.x - transform.position.x) + (playerTransform.position.y - transform.position.y) * (playerTransform.position.y - transform.position.y)); // 둘 사이 거리
        Launch();
    }

    void Launch()
    {
        float initialSpeed = Calculate_speed(distance, Physics2D.gravity.y);
        float angle = isRight ? 45f : 135f;

        float radianAngle = angle * Mathf.Deg2Rad;

        float VelocityX = initialSpeed * Mathf.Cos(radianAngle);
        float VelocityY = initialSpeed * Mathf.Sin(radianAngle);

        rb.velocity = new Vector2(VelocityX, VelocityY);
        rb.gravityScale = gravityScale;
    }

    float Calculate_speed(float distance, float gravity)
    {
        return Mathf.Sqrt(distance * Mathf.Abs(gravity) / Mathf.Sin(2f * 45f * Mathf.Deg2Rad));
    }

    void Explode()
    {
        if (isExploded) return;

        isExploded = true;

        Destroy(rb);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.transform.position, explosionRadius);

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log("플레이어에게 데미지 주기");
            }
            else
            {
                Debug.Log("오브젝트 충돌");
            }
        }
    }

    void destroyBomb()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pos.transform.position, size);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isExploded)
        {
            if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Player"))
            {
                transform.localScale = new Vector3(2f, 2f, 0f);
                ani.SetTrigger("bomb");
                Explode();
            }
        }
    }
}
