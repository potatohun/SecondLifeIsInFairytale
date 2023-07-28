using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tiger : MonoBehaviour
{
    private Animator ani;
    private float currentTime = 0f;
    private Transform target;
    private bool isAttacking = false; // 공격 중 상태
    private bool isIdleAfterAttack = false; // 공격 후 idle
    private SpriteRenderer render;
    public float speed = 1f;
    public GameObject[] pos;
    public Vector2[] boxsize;
    public float idleTime = 1f; // 공격 후 idle 시간
    private System.Random rand;
    private int posIndex = 0;
    public float HP = 100f;
    public Scrollbar Health;
    public bool dead { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();

        ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        Health.value = HP;

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, target.position);

        if (!isAttacking && distance > 5f)
        {
            if (!isIdleAfterAttack)
            {
                DirectionEnemy(target.transform.position.x, transform.position.x);
                ani.SetBool("isFollow", true);
                Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
        else if (distance <= 5f && distance > 4f)
        {
            ani.SetBool("isFollow", false);
            if (!isAttacking && !isIdleAfterAttack)
            {
                ani.SetBool("isFollow", false);
                Debug.Log("중거리 도끼찍기!!!!");
                DirectionEnemy(target.transform.position.x, transform.position.x);
                StartCoroutine(axeAttackRoutine());
            }
        }
        else
        {
            ani.SetBool("isFollow", false);
            if (!isAttacking && !isIdleAfterAttack)
            {
                DirectionEnemy(target.transform.position.x, transform.position.x);
                StartCoroutine(AttackRoutine());
            }
        }
    }

        IEnumerator AttackRoutine()
        {
            isAttacking = true;
            if (rand.NextDouble() > 0.5)
            {
                ani.SetTrigger("attack1");
                posIndex = 0;
            }
            else
            {
                ani.SetTrigger("bite");
                posIndex = 1;
            }
            yield return new WaitForSeconds(0.5f); // 공격 애니메이션 재생 시간
            isAttacking = false;
            isIdleAfterAttack = true;
            yield return new WaitForSeconds(idleTime); // 수정된 부분: 일정 시간 동안 idle 상태로 대기
            isIdleAfterAttack = false;
        }

        IEnumerator axeAttackRoutine()
        {
            isAttacking = true;
            ani.SetTrigger("attack2");
            posIndex = 2;
            yield return new WaitForSeconds(1f); // 공격 애니메이션 재생 시간
            isAttacking = false;
            isIdleAfterAttack = true;
            yield return new WaitForSeconds(idleTime); // 수정된 부분: 일정 시간 동안 idle 상태로 대기
            isIdleAfterAttack = false;
        }

        void OnDrawGizmosSelected() // 에디터에서 해당 오브젝트를 선택했을 때에만 실행되도록 변경
        {

            for (int i = 0; i < pos.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(pos[i].transform.position, boxsize[i]);
            }
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
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos[posIndex].transform.position, boxsize[posIndex], 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Player")
                    UnityEngine.Debug.Log(collider.tag + posIndex + "!!");
            }
        }

    public void TakeDamage(int damage)
    { 
        HP -= damage;
        Health.value = HP;
        Debug.Log(damage);

        if (HP == 0)
        {
            dead = true;
            ani.SetTrigger("die");
        }
    }
}
