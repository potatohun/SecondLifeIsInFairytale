using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalMonster : Enemy
{ 
    public bool isAttacking = false; // 공격 중 상태
    public float idleTime = 2f; // 공격 후 idle 시간
    private bool isIdleAfterAttack = false; // 공격 후 idle 지정 변수
    private bool takeAttack = false; //사망 시 더이상 공격 못하도록 설정

    public AudioSource attack;
    public AudioSource mouse;
    void Start()
    {
        OnEnable();
        isAttacking = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        monsterSpeed = 4f;
    }

    void Update()
    {
        if (dead) return;

        yDis = target.position.y - transform.position.y;

        if (Mathf.Abs(yDis) > 2f) // y좌표의 차이로 인한 현상 제한
        {
            ani.SetBool("isFollow", false); // 달리고 있는 중 생각
            return;
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (!isAttacking && distance < 8f && distance > 1.3f)
        {
            if (!isIdleAfterAttack)
            {
                DirectionEnemy();
                ani.SetBool("isFollow", true);
                Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, monsterSpeed * Time.deltaTime);
            }
        }
        else if (distance >= 8f)
        {
            ani.SetBool("isFollow", false);
        }
        else if (distance <= 1.3f)
        {
            ani.SetBool("isFollow", false);
            if (!isAttacking && !isIdleAfterAttack) // 공격 중이거나 이미 idle 중이라면 실행하지 않음
            {
                DirectionEnemy();
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
        StartCoroutine(stop_time());
    }

    IEnumerator stop_time()
    {
        isIdleAfterAttack = true;
        yield return new WaitForSeconds(idleTime);
        isIdleAfterAttack = false;
    }

    public override void TakeDamage(int damage)
    {
        if (dead) return;

        takeAttack = true;
        textOut(damage);

        if (Health.value <= 0)
        {
            Die();
        }
        else
        {
            ani.SetTrigger("Hit");
            StartCoroutine(stop_time());
            takeAttack = false;
        }
    }

    public void AttackSound()
    {
        attack.Play();
    }

    public void mousePlay()
    {
        mouse.Play();
    }
}
