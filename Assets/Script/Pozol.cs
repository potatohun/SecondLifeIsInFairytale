using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pozol : MonoBehaviour
{ 
    private Animator ani;
    private float currentTime = 0f;
    private Transform target;
    private bool isAttacking = false; // 공격 중 상태
    private bool isIdleAfterAttack = false; // 공격 후 idle
    private SpriteRenderer render;
    public float speed = 1f;
    public Transform pos;
    public BoxCollider2D box;
    public Vector2 boxsize;
    public float idleTime = 1f; // 공격 후 idle 시간

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (!isAttacking && distance < 8f && distance > 2f)
        {
            
            if (!isIdleAfterAttack)
            {
                DirectionEnemy(target.transform.position.x, transform.position.x);
                ani.SetBool("isFollow", true);
                Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
        else if (distance >= 8f)
        {
            ani.SetBool("isFollow", false);
        }
        else if (distance <= 2f)
        {
            ani.SetBool("isFollow", false);
            if (!isAttacking && !isIdleAfterAttack) // 공격 중이거나 이미 idle 중이라면 실행하지 않음
            {
                DirectionEnemy(target.transform.position.x, transform.position.x);
                ani.SetTrigger("attack");
                StartCoroutine(ResumeAttack());
            }
        }
    }

    IEnumerator ResumeAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f); // 공격 애니메이션 재생 시간 (1초)
        isAttacking = false;
        isIdleAfterAttack = true;
        yield return new WaitForSeconds(idleTime); // 수정된 부분: 일정 시간 동안 idle 상태로 대기
        isIdleAfterAttack = false;
    }


    private void OnDrawGizmos() // 컴파일 할 때 자동 실행됨.
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxsize);
    }

    public void DirectionEnemy(float target, float baseobj)
    {
        if (target < baseobj)
            render.flipX = true;
        else
            render.flipX = false;
    }

    public void FindAnd()
    {
        if (render.flipX == false)
        {
            pos.localPosition = new Vector3(0.357f, pos.localPosition.y, 0f);
        }
        else
        {
            pos.localPosition = new Vector3(-0.357f, pos.localPosition.y, 0f);
        }

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxsize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
                UnityEngine.Debug.Log(collider.tag);
        }

    }
}
