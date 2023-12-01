using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public SpriteRenderer render;
    public Rigidbody2D rb;
    public System.Random rand;
    public Animator ani;
    public bool dead { get; protected set; }
    public BoxCollider2D bColl;

    public Vector2 boxsize;
    public Vector3 attackOffset;
    public bool isFlipped = false;

    public float monsterSpeed;
    protected float yDis = 0f;

    //hp와 damageText에 관한 변수
    public GameObject damageText;
    public Transform textPos;
    public Slider Health;
    public float HP = 100f;

    public Collider2D targetCollider;

    public Color originalColor;
    Coroutine iceCoroutine;
    Coroutine fireCoroutine;

    //초기 상태 설정
    protected virtual void OnEnable()
    {
        isFlipped = true;
        HP = 100f;
        dead = false;
        rand = new System.Random();
        render = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        ani = GetComponent<Animator>();
        bColl = GetComponent<BoxCollider2D>();
        targetCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        Health.value = HP;
        monsterSpeed = 2f;
        originalColor = render.color;
        render.flipX = true;
        IgnoreCollisions(targetCollider);
    }

    void IgnoreCollisions(Collider2D collider1)
    {
        Physics2D.IgnoreCollision(collider1, bColl, true);
    }

    //공격 범위 표시
    protected virtual void OnDrawGizmos()
    {
        Vector3 posO = transform.position;
        posO += transform.right * attackOffset.x;
        posO += transform.up * attackOffset.y;

        Gizmos.DrawWireCube(posO, boxsize);
    }

    // 데미지 주는 함수
    public virtual void FindAnd()
    {
        Vector3 posO = transform.position;
        posO += transform.right * attackOffset.x;
        posO += transform.up * attackOffset.y;

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(posO, boxsize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                PlayerHit player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHit>();
                player.Hit(20, this.gameObject);
            }
        }
    }

    //몬스터 애니메이션 방향 전환
    public virtual void DirectionEnemy()
    {
        if (target.position.x > transform.position.x && isFlipped)
        {
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (target.position.x < transform.position.x && !isFlipped)
        {
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    //데미지 입는 함수(= takeDamage)
    public virtual void TakeDamage(int damage)
    {
        if (dead) return;
        textOut(damage);

        if (Health.value <= 0)
        {
            Die();
        }
    }

    // hp 텍스트 및 최신화 과정 함수 작성
    protected void textOut(int damage)
    {
        HP -= damage;
        Health.value = HP;
        GameObject hitText = Instantiate(damageText);
        hitText.transform.position = textPos.position;
        hitText.GetComponent<DamageText>().damage = damage;
    }

    protected virtual void Die()
    {
        dead = true;
        ani.SetTrigger("die");
    }

    //몬스터 제거
    public virtual void monsterDestroy()
    {
        GameObject item = ItemManager.Instance.DropItem(this.gameObject.transform.position);
        if (item != null)
        {
            item.transform.SetParent(this.transform.parent);
            item.transform.position = this.gameObject.transform.position;
        }

        Destroy(this.gameObject);
    }

    public void StartIceEffect()
    {
        // 만약 이전에 실행 중이던 얼음 효과 코루틴이 있다면 중지
        if (iceCoroutine != null)
        {
            StopCoroutine(iceCoroutine);
            monsterSpeed *= 2;
        }

        // 얼음 효과 코루틴을 시작하고 참조 저장
        iceCoroutine = StartCoroutine(ice_effects());
    }

    public void StartFireEffect()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }

        fireCoroutine = StartCoroutine(Fired());
    }

    //얼음 효과 ->  데미지 처리는 takeDamage 사용해서 조절해야될듯
    public IEnumerator ice_effects()
    { 
        render.color = new Color(0.23f, 0.23f, 1f);
        monsterSpeed /= 2;

        yield return new WaitForSeconds(2f);

        render.color = originalColor;
        monsterSpeed *= 2;
        iceCoroutine = null;
    }

    public IEnumerator Fired() // 불 효과 -> 도트뎀 
    {
        render.color = Color.red;

        for (int i = 0; i < 5; i++)
        {
            TakeDamage(5);
            yield return new WaitForSeconds(1f);
        }
        render.color = Color.white;
        fireCoroutine = null;
    }
}
