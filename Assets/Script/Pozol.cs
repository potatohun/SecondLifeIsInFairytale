using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pozol : MonoBehaviour // 일단 해당 포졸에 hp 감소와 데미지 출력 테스트
{ 
    private Animator ani;
    private float currentTime = 0f;
    private Transform target;
    private bool isAttacking = false; // 공격 중 상태
    private bool isIdleAfterAttack = false; // 공격 후 idle
    private SpriteRenderer render;
    public float speed = 1f;
    public Transform pos;
    public Vector2 boxsize;
    public float idleTime = 1f; // 공격 후 idle 시간
    public GameObject hudDamageText;
    public Transform hudPos;
    public Slider Health;
    public Transform HPPos;
    public float HP = 100f;
    public float SetTime;
    private bool takeAttack = false;

    public bool dead { get; protected set; }

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        Health.value = HP;
    }

    void Update()
    {
        if(dead) return;

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
            if (!isAttacking && !isIdleAfterAttack && !takeAttack) // 공격 중이거나 이미 idle 중이라면 실행하지 않음
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

    void DirectionEnemy(float target, float baseobj)
    {
        if (target < baseobj)
            render.flipX = true;
        else
            render.flipX = false;
    }

    void FindAnd()
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

    public void TakeDamage(int damage)
    {
        if (dead) return;

        if (takeAttack)
            return;

        StartCoroutine(TakeHit(damage));
    }

    IEnumerator TakeHit(int damage)
    {
        takeAttack = true;
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = damage;
        HP -= damage;
        Health.value = HP;
        Debug.Log(damage);

        if (HP == 40)
        {
            ani.SetTrigger("Hit");
        }

        if (HP <= 0)
        {
            dead = true;
            ani.SetTrigger("die");
            Invoke("SetFalse", SetTime);
        }
        takeAttack = false;
        yield return new WaitForEndOfFrame();
    }

    private void SetFalse()
    {
        Destroy(gameObject);
    }
}
