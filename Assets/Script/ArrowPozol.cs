using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPozol : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float spawnRate = 3f; // 쿨타임 (초 단위)
    private float currentTime = 0f; // 현재 경과 시간]
    private Animator ani;
    private Transform target;
    public float speed = 1f;
    private SpriteRenderer render;
    private bool isAttacking = false;

    MonsterSpawner monsterspawner;
    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();


        //spawner
        monsterspawner = GameObject.FindGameObjectWithTag("MonsterSpawner").GetComponent<MonsterSpawner>();
    }

    void Update()
    {
        DirectionEnemy(target.transform.position.x, transform.position.x);
        // 경과 시간 업데이트
        currentTime += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, target.position);

        if ( !isAttacking && distance < 14f && distance > 8f )
        {
            ani.SetBool("isFollow", true);
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else if( distance >= 14f)
        {
            ani.SetBool("isFollow", false);
        }
        else if( distance <= 8f)
        {
            ani.SetBool("isFollow", false);
            // 쿨타임이 지났을 때 화살 생성
            if (currentTime >= spawnRate)
            {
                StartCoroutine(AttackRoutine());
                currentTime = 0f; // 경과 시간 초기화
            }

        }
        
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        ani.SetTrigger("attack1");
        yield return new WaitForSeconds(0.5f); // 공격 애니메이션 재생 시간
        SpawnArrow();
        yield return new WaitForSeconds(1f); // 추가 대기 시간 (조정 가능)
        isAttacking = false;
    }

    public void DirectionEnemy(float target, float baseobj)
    {
        if (target < baseobj)
            render.flipX = true;
        else
            render.flipX = false;
    }

    void SpawnArrow()
    {
        Instantiate(arrowPrefab, transform.position + new Vector3(0f, -0.564207f, 0f), Quaternion.identity);
    }

    private void OnDestroy()
    {
        monsterspawner.monsters.Remove(this.gameObject);
        if (monsterspawner.ListEmptyCheck())
        {
            monsterspawner.ActivePortal();
            Debug.Log("포탈생성!");
        }
        Debug.Log("주금");
    }
}