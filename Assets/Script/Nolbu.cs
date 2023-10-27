    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

public class Nolbu : MonoBehaviour
{
    public Transform bossTransform;
    public float patternInterval = 2f;
    private float currentTime = 0f;
    private int patternIndex = 0;
    private Vector3[] attackPositions;
    private Animator bossAnimator;
    public System.Random rand;
    private SpriteRenderer render;
    public float SetTime;
    private bool takeAttack = false;
    public bool dead { get; protected set; }
    private BoxCollider2D coll;
    private int count = 0;
    private Vector3 direction;
    public GameObject hudDamageText;
    public Transform hudPos;
    public Slider Health;
    public Transform HPPos;
    public float HP = 7f;
    public float speed = 2f;
    private List<GameObject> activePrefabs;
    public GameObject[] money;
    public GameObject warningEffectPrefab;
    private GameObject warningEffectInstance;
    public GameObject warningCircle;
    public GameObject arrowRains;

    public GameObject coinBomb;
    public GameObject projectilePrefab;    // 발사할 오브젝트 프리팹
    public float projectileSpeed = 10f;     // 발사할 오브젝트의 속도
    private Transform playerTransform;      // 플레이어의 Transform
    private Coroutine currentPatternCoroutine = null;

    public Boss boss;

    void Start()
    {
        Health.value = HP;
        activePrefabs = new List<GameObject>();
        rand = new System.Random();
        bossAnimator = GetComponent<Animator>();
        attackPositions = new Vector3[3];
        attackPositions[0] = new Vector3(0.03f, -0.55f, 0f); // 가운데 위치
        attackPositions[1] = new Vector3(-5.92f, -0.55f, 0f); // 왼쪽 위치
        attackPositions[2] = new Vector3(5.95f, -0.55f, 0f); // 오른쪽 위치
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (dead) return;

        DirectionEnemy(playerTransform.position.x, transform.position.x);
        bossAnimator.SetBool("back", false);
        currentTime += Time.deltaTime;
        if (currentTime >= patternInterval)
        {
            activePrefabs.Clear();
            double value = rand.NextDouble();
            if (value > 0 && value <= 0.4)
            {
                currentPatternCoroutine = StartCoroutine(ShootWarningLand());
            }
            else if (0.7 >= value && value > 0.4)
            {
                currentPatternCoroutine = StartCoroutine(ExecutePatternAll());
            }
            else
            {
                currentPatternCoroutine = StartCoroutine(ExecurePatternCircle());
            }
            currentTime = 0f;
        }
        bossAnimator.SetBool("back", false);
    }

    public void DirectionEnemy(float target, float baseobj)
    {
        if (target < baseobj)
            render.flipX = true;
        else
            render.flipX = false;
    }

    public void arrowRain(int patternIndex)
    {
        GameObject rain = Instantiate(arrowRains, attackPositions[patternIndex] + new Vector3(0f, 1.12f, 0f), Quaternion.identity);
        activePrefabs.Add(rain);
    }

    IEnumerator ExecutePatternAll() // 전범위 공격
    {
        activePrefabs.Clear();
        for (int i = 0; i < 3; i++)
        {
            patternIndex = Random.Range(0, 3);
            warningEffectInstance = Instantiate(warningEffectPrefab, attackPositions[patternIndex], Quaternion.identity);
            activePrefabs.Add(warningEffectInstance);
            yield return new WaitForSeconds(0.5f);
            Destroy(warningEffectInstance);
            activePrefabs.Remove(warningEffectInstance);
            bossAnimator.SetTrigger("arrowUP");
            yield return new WaitForSeconds(0.5f);
            arrowRain(patternIndex);

            yield return new WaitForSeconds(0.5f);
            foreach (var prefab in activePrefabs)
            {
                Destroy(prefab);
            }
            activePrefabs.Clear();

            currentTime = 0f;
        }
        currentPatternCoroutine = null;
    }

    IEnumerator ExecurePatternCircle() // 원 공격
    {
        if (currentPatternCoroutine != null)
        {
            yield return null; // 다른 코루틴이 실행 중이면 아무 작업도 하지 않음
        }
        else
        {
            activePrefabs.Clear();
            bossAnimator.SetBool("nono", false);
            warningEffectInstance = Instantiate(warningCircle, bossTransform.position, Quaternion.identity);
            activePrefabs.Add(warningEffectInstance);
            yield return new WaitForSeconds(1f);
            Destroy(warningEffectInstance);
            activePrefabs.Remove(warningEffectInstance);
            bossAnimator.SetTrigger("noise");
            currentTime = 0f;
            yield return new WaitForSeconds(0.5f);
            bossAnimator.SetBool("nono", true);

            currentPatternCoroutine = null;
        }
    }

    IEnumerator ShootWarningLand() // 투사체 발사
    {
        for (int i = 0; i < 5; i++)
        {
            bossAnimator.SetTrigger("throw");
            yield return new WaitForSeconds(0.2f);
            GameObject land = Instantiate(projectilePrefab, bossTransform.position - new Vector3(0f, 0.55f, 0f), Quaternion.identity);
            Rigidbody2D laserRigidbody = land.GetComponent<Rigidbody2D>();
            Vector3 targetPosition = (playerTransform.position + new Vector3(0f, 1f, 0f) - bossTransform.position).normalized;

            laserRigidbody.velocity = targetPosition * projectileSpeed;

            Destroy(land, 3f);
            yield return new WaitForSeconds(0.5f);
            currentTime = 0f;
        }
        bossAnimator.SetBool("back", true);
        currentPatternCoroutine = null;
    }

    IEnumerator BombPattern()
    {
        if (currentPatternCoroutine != null)
        {
            StopCoroutine(currentPatternCoroutine);
        }

        foreach (var prefab in activePrefabs)
        {
            Destroy(prefab);
        }

        yield return new WaitForSeconds(1f);
        bossAnimator.SetTrigger("throw");
        GameObject land = Instantiate(coinBomb, bossTransform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(0.7f);
        bossAnimator.SetBool("back", true);
    }

    IEnumerator SpecialPattern()
    {
        // 여기에 특수한 패턴을 실행하는 코드를 추가하세요.
        // 예를 들어, 특정 공격이나 이펙트를 실행하는 등의 작업을 수행할 수 있습니다.
        // 필요한 경우 WaitForSeconds 등을 사용하여 타이밍을 조절하세요.

        yield return new WaitForSeconds(1f); // 예시: 1초 대기

        // 특수한 패턴 실행 후에는 다시 표준 패턴으로 돌아가도록 할 수 있습니다.
        currentPatternCoroutine = StartCoroutine(BombPattern());
    }

    IEnumerator TakeDamageRoutine(int damage)
    {
        count++;
        takeAttack = true;

        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position;
        hudText.GetComponent<DamageText>().damage = damage;
        HP -= damage;
        Health.value = HP;
        Debug.Log(damage);

        if (Health.value == 2) // HP가 2일 때 특수한 패턴 실행
        {
            StartCoroutine(SpecialPattern());
        }
        else if (count % 2 == 0)
        {
            if (currentPatternCoroutine != null)
            {
                StopCoroutine(currentPatternCoroutine);
            }

            foreach (var prefab in activePrefabs)
            {
                Destroy(prefab);
            }

            activePrefabs.Clear();
            currentPatternCoroutine = null;
            coll.enabled = false;
            bossAnimator.SetBool("hit", true);
            yield return new WaitForSeconds(0.5f);
            bossAnimator.SetBool("hit", false);
            coll.enabled = true;
            bossTransform.position += new Vector3(0f, 1.3f, 0f);
            SetActiveMoney(0);

            yield return new WaitForSeconds(2f);

            while (currentPatternCoroutine != null)
            {
                yield return null;
            }
            coll.enabled = true;
            yield return new WaitForSeconds(1f);

            adActiveMoney(0);
            bossTransform.position -= new Vector3(0f, 1.3f, 0f);
        }
        else
        {
            coll.enabled = false;
            yield return new WaitForSeconds(1f);
            coll.enabled = true;
        }

        if (Health.value <= 0f)
        {
            dead = true;
            boss.isDie = true;
            if (currentPatternCoroutine != null)
            {
                StopCoroutine(currentPatternCoroutine);

                foreach (var prefab in activePrefabs)
                {
                    Destroy(prefab);
                }
                activePrefabs.Clear();
                currentPatternCoroutine = null;
            }
            yield return new WaitForSeconds(2f);

            bossAnimator.SetTrigger("die");
            Invoke("SetFalse", SetTime);
        }
        takeAttack = false;
    }

    public void TakeDamage(int damage)
    {
        if (dead) return;

        if (takeAttack)
            return;

        StartCoroutine(TakeDamageRoutine(damage));
    }

    private void SetFalse()
    {
        Destroy(gameObject);
    }

    public void SetActiveMoney(int index)
    {
        money[index].SetActive(true);
    }

    public void adActiveMoney(int index)
    {
        money[index].SetActive(false);
    }
}