using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewNolbu : MonoBehaviour
{
    public bool dead { get; protected set; }
    public Animator bossAni;
    public int arrowCount = 0;
    private System.Random rand;
    private SpriteRenderer render;
    private Transform playerTransform;
    private Collider2D coll;

    public int patternIndex;
    private Vector3[] attackPositions;
    private GameObject prefab_instance;
    public GameObject RectWarning;
    public GameObject circleWarning;
    public GameObject coin;
    public GameObject arrowRains;
    public GameObject coinBomb;

    public List<GameObject> activePrefabs; // 화면 상의 프리팹들
    public GameObject[] money; // 금은보화와 사망 시 나오는 상자
    public Coroutine currentPatternCoroutine = null;

    public float radius;

    //hp와 damageText에 관한 변수
    public GameObject damageText;
    public Transform textPos;
    public Slider Health;
    public float HP = 7f;
    private bool oneTime = true;

    void Start()
    {
        //초기 설정 사용
        oneTime = true;
        dead = false;
        Health.value = HP;
        bossAni = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        rand = new System.Random();
        coll = GetComponent<Collider2D>();
        arrowCount = 0; // arrowRain 3번 반복을 위한 카운터 변수

        activePrefabs = new List<GameObject>();
        attackPositions = new Vector3[3];
        attackPositions[0] = new Vector3(0.03f, -0.55f, 0f); // 가운데 위치
        attackPositions[1] = new Vector3(-5.92f, -0.55f, 0f); // 왼쪽 위치
        attackPositions[2] = new Vector3(5.95f, -0.55f, 0f); // 오른쪽 위치
    }

    void Update()
    {
        if (dead) return;

        DirectionEnemy(playerTransform.position.x, transform.position.x);
    }

    //idle 상태에서의 작동방식 조절
    public void IdleState()
    {
        // Bomb script에서 포물선의 각도 구하기 공식은 인터넷 참고
        if(Health.value == 1 && oneTime == true)
        {
            bossAni.SetTrigger("Bomb");
            oneTime = false;
        }

        double value = rand.NextDouble();
        if (value > 0 && value <= 0.4)
        {
            bossAni.SetTrigger("arrowUP");
        }
        else if (0.7 >= value && value > 0.4)
        {
            bossAni.SetTrigger("patternTwo");
        }
        else
        {
            bossAni.SetTrigger("patternThree");
        }
    }


    //사망 시 동작하는 함수
    void Die()
    {
        dead = true;
        pattern_check_stop();
        bossAni.SetTrigger("die");
        //애니메이션 종료후 Boss.IsDie() 함수 발동
    }

    void bossDelete()
    {
        SetMoney(1, 1);
        Destroy(gameObject);
    }

    //공격 패턴 1
    public IEnumerator RangeAll() // 전범위 공격
    {
        patternIndex = Random.Range(0, 3);
        prefab_instance = Instantiate(RectWarning, attackPositions[patternIndex], Quaternion.identity);
        activePrefabs.Add(prefab_instance);
        yield return new WaitForSeconds(0.5f);
        Destroy(prefab_instance);
        activePrefabs.Remove(prefab_instance);
        bossAni.SetBool("arrowRe", true);
        arrowRain(patternIndex);
    }

    void arrowRain(int patternIndex)
    {
        GameObject rain = Instantiate(arrowRains, attackPositions[patternIndex] + new Vector3(0f, 1.12f, 0f), Quaternion.identity);
    }

    //공격 패턴 2
    public IEnumerator warningCircle() // 원 공격 주의 + 데미지 인식은 애니메이션에 데미지 함수 FInd 추가
    {
        prefab_instance = Instantiate(circleWarning, transform.position, Quaternion.identity);
        activePrefabs.Add(prefab_instance);
        yield return new WaitForSeconds(1f);
        Destroy(prefab_instance);
        activePrefabs.Remove(prefab_instance);
        bossAni.SetTrigger("noise");
    }

    //공격 패턴 3
    public IEnumerator ShootCoin() // 투사체 발사, coin 스크립트에서 데미지 및 삭제 자동 실행
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.2f);
            prefab_instance = Instantiate(coin, transform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
            if (i != 4)
            {
                yield return new WaitForSeconds(0.6f);
            }
        }
        bossAni.SetTrigger("endCoin");
    }

    //즉사 패턴 ( 그냥 폭탄 소환 => 폭탄 자체의 스크립트로 날라감 )
    public void addBomb()
    {
        prefab_instance = Instantiate(coinBomb, transform.position, Quaternion.identity);
    }


    //hit 시 동작하는 함수. 함수 내에 전체적인 스턴 상태 변수 수정, Transform 위치 이동 및 금은보화 소환
    // hit 시에 나오는 금은보화 상자와 함께 몬스터의 무적상태를 위한 collider2D box 비활성화, 위치 조정 함수 필요
    public IEnumerator hitAndGold()
    {
        transform.position += new Vector3(0f, 1.5f, 0f);
        SetMoney(0, 1);
        yield return new WaitForSeconds(3f);
        while (currentPatternCoroutine != null)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        SetCollider(1);
        SetMoney(0, 0);
        transform.position -= new Vector3(0f, 1.5f, 0f);
    }

    //데미지 입는 함수(= takeDamage) 일단 예비용 가져옴
    public void TakeDamage(int damage)
    {
        if (dead) return;
        textOut(damage);

        if (Health.value % 2 == 0 && Health.value < 5 && Health.value != 0)
        {
            bossAni.SetTrigger("hit");
        }
        else if (Health.value == 0)
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

    public void RemovePrefabs() // 화면 상의 프리팹들 삭제
    {
        foreach (var prefab in activePrefabs)
        {
            Destroy(prefab);
        }
        activePrefabs.Clear();
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

    public void SetCollider(int set)
    {
        if (set == 0)
            coll.enabled = false;
        else
            coll.enabled = true;
    }

    public void SetMoney(int index, int set)
    {
        if (set == 0)
            money[index].SetActive(false);
        else
            money[index].SetActive(true);
    }

    public void pattern_check_stop()
    {
        if (currentPatternCoroutine != null)
        {
            StopCoroutine(currentPatternCoroutine);
            arrowCount = 0; // idle 상태에서 바로 arrowUP으로 갈때 hit에 남은 숫자가 있으면 화살공격이 1, 2번으로 끝날 수 있기에 초기화 진행
            currentPatternCoroutine = null; // 현재 코루틴을 멈췄으니 초기화
            RemovePrefabs(); // 화면상의 모든 저장된 프리팹들 삭제
        }
    }
}
