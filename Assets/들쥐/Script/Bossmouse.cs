using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//고려사항
// 들쥐의 경우, phase 2, 3 같은 경우는 같이 애니메이션을 공유하기 때문에 그에 따른 색깔을 고정하도록 해야됨. -> changeColor() 를 통한 해결
// 각 phase 마다 달리기 보유( 도령은 달리기, 들쥐는 달리기 및 점프 필요 ) -> 구간은 특정 패턴 시에만 달려가서 소음공격 실행
// 각 패턴마다 데미지 설정과 범위 변경요소 -> 공통되는 패턴은 한 프리팹에 넣어놨으며 해당 switch문을 사용하여 크기의 변경과 데미지의 변경을 요함.
// collider2D에 의한 데미지

public class Bossmouse : MonoBehaviour
{
    public Animator bossAni;
    public Rigidbody2D rb;
    public bool dead { get; protected set; }
    public SpriteRenderer render;
    private BoxCollider2D coll;
    public CircleCollider2D ccoll;
    public CapsuleCollider2D pcoll;
    public Vector3 initialPosition = new Vector3(7.23999977f, -3.10665798f, 0f);

    public GameObject warningBottom;
    public GameObject circleWarning;
    public GameObject[] throwPrefab;
    private GameObject choiceObject;
    private GameObject prefab_instance;
    public List<GameObject> activePrefabs; // 화면 상의 프리팹들

    public float[] pattern_damage;
    public Transform warningBottom_pos; // x축 돌진 경고창 위치
    private Transform playerTransform;
    public System.Random rand;
    // y축 3번 찍기를 위한 새로운 물리 재질
    public PhysicsMaterial2D newPhysicsMaterial;

    //현재 phase 상태를 판별하기 위한 변수, 0 = normal, 1 = 1 phase, 2 = 2 phase
    public float phase_state = 0f;
    public Coroutine currentPatternCoroutine = null;
    float radius = 1f;

    //move 함수를 위한 변수
    public float nextMove = 4f;
    public bool move_attack = false;
    public float distance = 3f;

    //한자 날리기 개수 체크
    public int check_gal = 0;

    //hp와 damageText에 관한 변수
    public GameObject damageText;
    public Transform textPos;
    public Slider Health;
    public float HP = 100f;

    //구르는 중 표시 변수
    private bool rolling = false;

    public Boss boss;
    //초기 설정
    void Start()
    {
        rolling = false;
        Health.value = HP;
        phase_state = 0f;
        check_gal = 0;
        pattern_damage = new float[5];
        pattern_damage[0] = 10f; // 한자
        pattern_damage[1] = 12f; // 목탁
        pattern_damage[2] = 15f; // 발톱 & 손톱 통일
        pattern_damage[3] = 20f; // noise는 범위 안에 있으면 지속적으로 데미지 입기
        pattern_damage[4] = 30f; // x축, y축, 부딪히는 데미지 => 모두 몸박 데미지 이기 때문에 30으로 통일

        bossAni = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rand = new System.Random();
        render = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        coll = this.GetComponent<BoxCollider2D>();
        ccoll = this.GetComponent<CircleCollider2D>();
        pcoll = this.GetComponent<CapsuleCollider2D>();
    }

    //update로 좌우판단 및 사망 상태 확인
    void Update()
    {
        if (dead) return;

        if(!rolling)
            DirectionEnemy(playerTransform.position.x, transform.position.x);
    }

    // 들쥐 내의 2->3 phase 변경을 위한 색 변경( 애니메이션 자체의 색깔까지 변경하는 걸 확인함)
    void ChangeColor()
    {
        render.color = new Color(0.83f, 0.37f, 0.37f, 1f);
    }

    //idle 상태에서의 작동방식 조절
    public void IdleState()
    {
        //phase 변수에 따른 패턴 세분화 작성요망

        if (phase_state == 0f)
        {
            if (move_attack)
            {
                bossAni.SetBool("run", true);
            }
            else
            {
                ShootObject(); // 패턴에 따라 수정 필요
                move_attack = true;
            }
        }
        else if (phase_state == 1f)
        {
            if (move_attack)
            {
                bossAni.SetBool("run", true);
            }
            else
            {
                double value = rand.NextDouble();
                if (value > 0 && value <= 0.7)
                {
                    ShootObject();
                }
                else
                {
                    pcoll.enabled = false;
                    ccoll.enabled = true;
                    bossAni.SetTrigger("goInitial");
                }
                move_attack = true;
            }
        }
        else if (phase_state == 2f)
        {
            if (move_attack)
            {
                bossAni.SetBool("run", true);
            }
            else
            {
                double value = rand.NextDouble();
                if (value > 0 && value <= 0.4)
                {
                    //오브젝트 발사
                    ShootObject();
                }
                else if(value > 0.4 && value <= 0.8)
                {
                    //x축 구르기
                    pcoll.enabled = false;
                    ccoll.enabled = true;
                    bossAni.SetTrigger("goInitial");
                }
                else if(value >0.8 && value <= 0.9)
                {
                    //noise
                    bossAni.SetTrigger("noisePattern");
                }
                else
                {
                    //updown
                    bossAni.SetTrigger("ready");
                }
                move_attack = true;
            }
        }
    }

    // 도령 상태의 한자 날리기와 목탁 던지기 두 패턴 작성
    // 만약 2phase 상태가 되면 프리팹을 교체하여 발톱 및 손톱 던지기 사용( 매개변수로 지정하거나 if문을 통한 구분 요망 )
    // 현재 상태에서 던지기 프리팹을 배열 4개로 구성하였기 때문에 phase_state 변수에 따른 if나 switch 문 구현 요망
    public void throw_gal() // 한자 날리기 -> 애니메이션 반복에 이상이 생겨 직접 애니메이션 함수로 추가하여 중지하도록 함.
    {
        prefab_instance = Instantiate(throwPrefab[0], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
        check_gal++;
        if (check_gal == 3)
            bossAni.SetTrigger("endThrow");
    }

    public IEnumerator throw_mok() // 목탁 던지기 
    {
        yield return new WaitForSeconds(0.5f);
        prefab_instance = Instantiate(throwPrefab[1], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
    }

    public IEnumerator throw_nail()
    {
        if (phase_state == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForSeconds(0.4f);
                prefab_instance = Instantiate(throwPrefab[2], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
                if (i != 3)
                    yield return new WaitForSeconds(0.3f);
            }
            bossAni.SetTrigger("endThrow");
        }
        else if(phase_state == 2)
        {
            for (int i = 0; i < 6; i++)
            {
                yield return new WaitForSeconds(0.4f);
                prefab_instance = Instantiate(throwPrefab[2], transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
                if (i != 3)
                    yield return new WaitForSeconds(0.3f);
            }
            bossAni.SetTrigger("endThrow");
        }
    }

    //move 함수 추가
    //주위 거리에서 rayCast 막힌 상태를 인식해서 무작위로 좌우 설정하는 변수 특정 거리마다 이동하도록 특정함..
    public void Check_distance()
    {
        if (phase_state == 0)
            distance = 3f;
        else if (phase_state == 1)
            distance = 4f;
        else
        {
            distance = 5f;
        }

        RaycastHit2D frayHit = Physics2D.Raycast(transform.position + new Vector3(1f, 0.1f, 0f), Vector2.right, distance); // right
        RaycastHit2D brayHit = Physics2D.Raycast(transform.position + new Vector3(-1f, 0.1f, 0f), Vector2.left, distance); // left

        if (frayHit.collider != null)
        {
            nextMove = -1f * distance;
        }
        else if (frayHit.collider == null && brayHit.collider == null)
        {
            double value = rand.NextDouble();
            if (value > 0.5)
            {
                nextMove = distance;
            }
            else
            {
                nextMove = -1f *distance;
            }
        }
        else if(frayHit.collider != null && brayHit.collider != null)
        {
            bossAni.SetBool("run", false);
        }
    }

    public void ShootObject() // 투사체 발사
    {
        double value = rand.NextDouble();

        switch(phase_state)
        {
            case 0:
                if (value > 0.5)
                    bossAni.SetTrigger("moktak");
                else
                    bossAni.SetTrigger("gal");
                break;
            case 1:
            case 2:
                bossAni.SetTrigger("throw_nail");
                break;
        }
    }

    public IEnumerator bottomWarning()
    {
        prefab_instance = Instantiate(warningBottom, warningBottom_pos.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Destroy(prefab_instance);
    }
   
    //x축 전범위 구르기
    public IEnumerator bottomAll() // x축 전범위 공격
    {
        float moveDuration = 1.5f;
        float moveSpeed = 20f;
        float exitTime = 0f;
        if (phase_state == 2)
            moveSpeed = 30f;

        while (exitTime < moveDuration)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            exitTime += Time.deltaTime;
            yield return null;
        }
        rb.velocity = Vector2.zero;
        transform.position = initialPosition;
        
        bossAni.SetTrigger("bottom_all");
    }

    //소음 공격
    public IEnumerator warningCircle() // 원 공격 주의 + 데미지 인식은 애니메이션에 데미지 함수 FInd 추가
    {
        prefab_instance = Instantiate(circleWarning, transform.position, Quaternion.identity);
        activePrefabs.Add(prefab_instance);
        yield return new WaitForSeconds(1f);
        Destroy(prefab_instance);
        activePrefabs.Remove(prefab_instance);
        bossAni.SetTrigger("noise");
    }

    public void ChangeMaterial(PhysicsMaterial2D material)
    {
        // Rigidbody2D의 sharedMaterial 속성을 사용하여 물리 재질 변경
        rb.sharedMaterial = material;
    }

    //y축 내려찍기 ( 위에 천장이 없는 상태이므로 맵을 포물선으로 바운스 함수
    public IEnumerator pattern_updown()
    {
        bool isRight = playerTransform.position.x > transform.position.x;
        float gravityScale = 1f;

        float initialSpeed = Calculate_speed(Physics2D.gravity.y);
        float angle = isRight ? 80f : 100f;

        float radianAngle = angle * Mathf.Deg2Rad;

        float VelocityX = initialSpeed * Mathf.Cos(radianAngle);
        float VelocityY = initialSpeed * Mathf.Sin(radianAngle);

        rb.velocity = new Vector2(VelocityX, VelocityY);
        rb.gravityScale = gravityScale;
        yield return new WaitForSeconds(1f);
        rb.gravityScale = 5* gravityScale;
    }

    //초기 속도 계산
    float Calculate_speed(float gravity)
    {
        return Mathf.Sqrt(2f* 6f * Mathf.Abs(gravity) / Mathf.Pow(Mathf.Sin(80f * Mathf.Deg2Rad), 2f));
    }

    private void OnDrawGizmos() // 원 공격 범위 표시
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void DirectionEnemy(float target, float baseobj) // render 좌우 적을 향하도록 조절
    {
        if (target < baseobj)
            render.flipX = true;
        else
            render.flipX = false;
    }

    public void FindAnd() // Noise 전용 범위 만들기 및 데미지 입히기
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D col in colliders)
        {
            if (col.tag == "Player")
                UnityEngine.Debug.Log(col.tag);
        }
    }

    //hit를 위한 현재 동작중인 패턴 중단 및 초기화
    public void pattern_check_stop()
    {
        if (currentPatternCoroutine != null)
        {
            StopCoroutine(currentPatternCoroutine);
            currentPatternCoroutine = null; // 현재 코루틴을 멈췄으니 초기화
            RemovePrefabs(); // 화면상의 모든 저장된 프리팹들 삭제
        }
    }

    public void RemovePrefabs() // 화면 상의 프리팹들 삭제
    {
        foreach (var prefab in activePrefabs)
        {
            Destroy(prefab);
        }
        activePrefabs.Clear();
    }

    //들쥐 상태 부터 collider2D를 활성화 시켜서 닿일 때마다 player hp 감소
    public void SetCollider(int set)
    {
        if (set == 0)
            coll.enabled = false;
        else
            coll.enabled = true;
    }

    //사망 시 동작하는 함수
    void Die()
    {
        pattern_check_stop();
        if (phase_state == 0)
        {
            coll.enabled = false;
            bossAni.SetTrigger("phase0_die");
            HP = 100f;
            Health.value = HP;
            pcoll.enabled = true;
        }
        else if (phase_state == 1)
        {
            bossAni.SetTrigger("phase1_die");
            HP = 100f;
            Health.value = HP;
            ChangeColor(); ;
        }
        else
        {
            dead = true;
            boss.isDie = true;
            bossAni.SetTrigger("die");
        }
        phase_state++;
    }

    public void bossDelete()
    {
        //Destroy(gameObject);
    }

    //데미지 입는 함수(= takeDamage) 일단 예비용 가져옴
    public void TakeDamage(int damage)
    {
        if (dead) return;
        textOut(damage);

        if (Health.value <= 0)
        {
            Die();
        }
    }

    // hp 텍스트 및 최신화 과정 함수 작성
    void textOut(int damage)
    {
        GameObject hitText = Instantiate(damageText);
        hitText.transform.position = textPos.position;
        hitText.GetComponent<DamageText>().damage = damage;
        HP -= damage;
        Health.value = HP;
    }

    //일단 충돌 안되는데 나중에 생각하기
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && (phase_state == 1 || phase_state == 2))
        {
            Debug.Log("충돌");
        }
    }

    //    1 Phase
    //  플레이어 size의 도령(변신 상태) -> 조금씩 움직이는 걸 목표로 해야겠음
    //  - 한자 날리기
    //  - 목탁 던지기
    // ---> shootObject 함수로 해결

    //2 Phase
    //  큰 들쥐(ver 1)
    //  - 발톱, 손톱 던지기
    // ---> shootObject 함수로 해결
    //  - x축 전범위 구르기
    // ---> bottomAll 함수로 해결

    // 점프도 만들어달라하면 좋을듯?
    //  큰 들쥐(ver 2) 전체 피 20% 이하가 남았을 때, Range 상태 돌입(들쥐 색깔 바뀜 )
    //  - 발톱, 손톱 던지기 + 데미지 증가 + 범위 증가
    //  - x축 전범위 구르기 + 데미지 증가 + 속도 향상
    //  - 맵을 3등분한 y축 내려찍기 -> 변경 -> 포물선으로 바운스하면서 튕기기
    // ---> attackUpDown 함수로 해결
    //  - 원을 범위로 한 소음공격 -> 플레이어 에게 달려가서 스킬 사용
    // ---> noise 함수로 해결
}
