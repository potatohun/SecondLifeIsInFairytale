using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tiger : Enemy
{
    private bool isAttacking = false; // 공격 중 상태
    private bool isIdleAfterAttack = false; // 공격 후 idle

    public Transform[] pos;
    public Vector2[] boxsizes;
    public float idleTime = 1f; // 공격 후 idle 시간

    private int posIndex = 0; // 패턴 데미지 처리 판별 변수
    public double value;

    public Boss boss;
    void Start()
    {
        OnEnable();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        HP = 300f;
        Health.value = HP;
    }

    void Update()
    {
        if (dead)
            return;

        yDis = target.position.y - transform.position.y;

        if (Mathf.Abs(yDis) > 2f) // y좌표의 차이로 인한 현상 제한
        {
            ani.SetBool("isFollow", false); // 달리고 있는 중 생각
            return;
        }
    }

    public void check_run()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > 5f)
        {
            DirectionEnemy();
            ani.SetBool("isFollow", true);
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, monsterSpeed * Time.deltaTime);
        }
        else
        {
            ani.SetBool("isFollow", false);
        }
    }

    public void IdleState()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > 5f)
        {
            ani.SetBool("isFollow", true);
        }
        else if (distance <= 5f && distance > 4f)
        {
            axeAttackRoutine();
        }
        else
        {
            AttackRoutine();
        }
    }

    void AttackRoutine()
    {
        value = rand.NextDouble();

        if (value > 0.5)
        { 
            ani.SetTrigger("attack1");
            posIndex = 0;
        }
        else
        {
            ani.SetTrigger("bite");
            posIndex = 1;
        }
    }

    void axeAttackRoutine()
    {
        ani.SetTrigger("attack2");
        posIndex = 2;
    }

    protected override void OnDrawGizmos()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(pos[i].transform.position, boxsizes[i]);
        }
    }

    public override void FindAnd()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos[posIndex].transform.position, boxsizes[posIndex], 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                PlayerHit player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHit>();
                if (posIndex == 0)
                {
                    player.Hit(10);
                }
                else if (posIndex == 1)
                {
                    player.Hit(20);
                }
                else if (posIndex == 2)
                {
                    player.Hit(30);
                }
            }
        }
    }
    public override void monsterDestroy()
    {
        boss.IsDie();

        //Destroy(gameObject);
    }
}