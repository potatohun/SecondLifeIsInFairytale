using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public float throwForce = 6f;      // 던질 힘
    public float explosionRadius = 1f; // 폭발 범위

    private Animator ani;
    public float size;
    public GameObject pos;
    private Rigidbody2D rb;
    private bool isExploded = false;
    private Transform playerTransform; // 플레이어의 Transform

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어를 찾아서 Transform 가져오기
        ThrowBomb();
        Invoke("destroyBomb", 3f);

    }

    void ThrowBomb()
    {
        // 플레이어와 폭탄 간의 방향 벡터 계산
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // 초기 속도 계산 (폭탄을 위로 던지려면 Y 방향 속도가 양수여야 합니다)
        Vector2 velocity = direction * throwForce;
        velocity.y = Mathf.Abs(velocity.y); // Y 방향 속도를 양수로 보정

        // Rigidbody2D에 힘 적용
        rb.velocity = velocity;
    }

    void Explode()
    {
        if (isExploded) return;

        isExploded = true;

        // 폭발 효과를 여기에 추가하세요.
        // 예를 들어, 폭발 사운드를 재생하거나 폭발 이펙트를 생성할 수 있습니다.

        // 주변에 있는 모든 Collider2D 가져오기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos.transform.position, explosionRadius);

        foreach (Collider2D col in colliders)
        {
            // 여기에서 각 Collider에 대한 작업을 수행하세요.
            if (col.CompareTag("Player"))
            {
                // 플레이어에게 데미지 주기 또는 다른 동작 수행
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

    private void OnDrawGizmos() // 컴파일 할 때 자동 실행됨.
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
                ani.SetTrigger("bomb");
                transform.localScale = new Vector3(2f, 2f, 0f);
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                Explode();
            }
        }
    }
}
