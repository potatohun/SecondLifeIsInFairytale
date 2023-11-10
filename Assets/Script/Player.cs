using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

using static UnityEngine.ParticleSystem;

public class Player : MonoBehaviour
{
    private static Player instance;
    Vector2 input;
    public int HP = 100;
    public int MAXHP = 100;

    public int moveSpeed = 5;
    public float jumpPower = 5;
    public float rollSpeed = 1.5f;

    bool canDoubleJump = false;

    // GameObject hand;

    public int combo = 1;
    public bool isAttack = false;
    float comboTimer;
    SpriteRenderer sr;

    AudioSource attackAudio;
    public Animator ani;

    Rigidbody2D rigid;
    CapsuleCollider2D playerCollider;


    public Vector2 boxSize;
    public Transform pos;

    //public Particle Particle;

    public float potionCoolTime = 2f;
    public bool canHpPotionDrink = true;

    public GameObject weapon;

    public Vector3 dirvec;
    public GameObject scanObj;

    public bool isSeegRight = true;

    public float x;

    public InventroyManager Inventory;


    public GameObject ApplePrefab;
    public GameObject RiceCakePrefab;
    public GameObject YakgwaPrefab;
    public GameObject TrapPrefab;
    public GameObject RockPrefab;
    public GameObject RollPaperPrefab;
    public GameObject WeponPrefab1;
    public GameObject WeponPrefab2;
    public Player Player1
    {
        get => default;
        set
        {
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // sceneLoaded 이벤트에 OnSceneLoaded 메소드를 연결
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.transform.position = Vector3.zero; // 씬이 로드될 때마다 Player 위치 (0,0,0)으로 초기화.
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        attackAudio = GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 오브젝트를 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복 생성된 것이므로 이 오브젝트를 파괴
            Destroy(gameObject);
        }

        Inventory = GetComponentInChildren<InventroyManager>();

    }
    void Update()
    {
        //Debug.Log(PlayerPrefs.GetInt("currentChapter"));
        RaycastHit2D rayhit;

        if (transform.localScale.x > 0)
        {
            Debug.DrawRay(transform.position + new Vector3(1, 0, 0), Vector3.forward * 20f, Color.green);
            rayhit = Physics2D.Raycast(transform.position + new Vector3(1, 0, 0), Vector3.forward * 20f, LayerMask.GetMask("Object"));
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(-1, 0, 0), Vector3.back * 20f, Color.green);
            rayhit = Physics2D.Raycast(transform.position + new Vector3(-1, 0, 0), Vector3.back * 20f, LayerMask.GetMask("Object"));
        }

        if (rayhit.collider != null)
        {
            scanObj = rayhit.collider.gameObject;
            if (scanObj.layer == LayerMask.NameToLayer("Item"))
            {
                Item item = scanObj.GetComponent<Item>();
                //item.isWatched = true; //아이템 쳐다보는중
            }
        }
        else
            scanObj = null;

        Jump();
        ComboAttack();
        Roll();
        Dead();
    }
    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        ani.SetFloat("speed", input.magnitude);
    }

    //오브젝트 스캔
    public GameObject ScanObj()
    {
        return scanObj;
    }

    void Move()
    {


        if (!ani.GetBool("Hit"))
        {
            float x = Input.GetAxis("Horizontal");
            input = new Vector2(x, 0) * moveSpeed * Time.deltaTime;
            //   rigid.MovePosition(rigid.position + input); // move Vs moveposition 
            //transform.position = transform.position + new Vector3(x,0) * speed * Time.deltaTime;  딱히 조작감차이는없는듯 rigid>>>>>transform

            rigid.position += input;

            //if(!isAttack)
            if (x < 0)
                transform.localScale = new Vector3(-2.5f, 2.5f, 0);
            else if (x > 0)
                transform.localScale = new Vector3(2.5f, 2.5f, 0);

        }
    }
    void Jump()
    {

        if (Input.GetButtonDown("Jump") && !ani.GetBool("isJump"))
        {
            ani.SetBool("isJump", true);
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            canDoubleJump = true;

        }
        else if (Input.GetButtonDown("Jump") && canDoubleJump == true)
        {
            ani.SetTrigger("DoubleJump");
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            canDoubleJump = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("ground"))
            ani.SetBool("isJump", false);

        //이게 foot의 콜라이더떄문에 플레이어콜라이더가 꺼져도 피가 닳음
        if (collision.gameObject.tag.Equals("Enemy"))
        {

            HP -= collision.gameObject.GetComponent<Enemy>().Attack;

            Debug.Log(" Player HP :" + HP);
            ani.SetBool("Hit", true);
            StartCoroutine(KnockBack(collision.gameObject));
        }


    }

    void Attack() // 공격관련 메소드만 지금 4개임 개선할필요가 있을거같군요...
    {
        attackAudio.Play();
        Collider2D[] enemy = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in enemy) //병철이랑 진규랑 통합해야 하는 파트
        {
            Debug.Log(collider.tag);
            switch (collider.tag)
            {
                case "Pozol":
                    collider.GetComponent<Pozol>().TakeDamage(10);//데미지 어케함               
                    break;
                case "Arrow_Pozol":
                    collider.GetComponent<ArrowPozol>().TakeDamage(10);//데미지 어케함             
                    break;
                case "Tiger":
                    collider.GetComponent<Tiger>().TakeDamage(10);//데미지 어케함             
                    break;
                case "Nolbu":
                    collider.GetComponent<Nolbu>().TakeDamage(2);//데미지 어케함             
                    break;
            }
        }
    }

    void ComboAttack()
    {
        IsAttack();

        comboTimer += Time.deltaTime;
        if (comboTimer > 1f)
        {
            combo = 1;
            comboTimer = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {


            if (combo == 1)
            {
                ani.SetTrigger("Attack1"); Attack();
                combo++;
            }
            else if (combo == 2 || (ani.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f))
            {

                ani.SetTrigger("Attack2"); Attack();
                combo = 1;

                comboTimer = 0;
                StartCoroutine("AttackDelay");
            }
        }
    }

    void IsAttack()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || ani.GetNextAnimatorStateInfo(0).IsName("Attack1") ||
            ani.GetCurrentAnimatorStateInfo(0).IsName("Attack2") || ani.GetNextAnimatorStateInfo(0).IsName("Attack2") ||
            ani.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || ani.GetNextAnimatorStateInfo(0).IsName("Attack3"))
            isAttack = true;
        else
            isAttack = false;
    }
    IEnumerator AttackDelay() // Attack을 진작에 코루틴으로 만들었다면...
    {
        yield return new WaitForSeconds(0.5f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
    void Roll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && (!isAttack && !ani.GetBool("Hit") && !ani.GetBool("isJump")))
            StartCoroutine(roll());

    }


    IEnumerator roll() // 코루틴으로 빼고 , 애니메이션이벤트있는거 지웠음 , 코루틴vs애니메이션이벤트?
    {

        float lookingDir = transform.localScale.x;

        UnableCollider();

        ani.SetTrigger("Roll");
        rigid.velocity = new Vector2(lookingDir, 0) * rollSpeed;

        yield return new WaitForSeconds(0.5f);

        EnableCollider();

        rigid.velocity = Vector2.zero;
        rigid.gravityScale = 1.25f;


    }
    IEnumerator KnockBack(GameObject enemy)
    {
        yield return null;
        sr.material.color = new Color(230 / 255f, 110 / 255f, 110 / 255f, 150 / 255f);

        //Vector3 enemyPos = GameManager.gameManager.enemy.transform.position; 이렇게하니까 enemy하나만 넉백돼서 안됨 그냥 CollsiionEnter에서 position가져오면 되는거였는데..
        Vector3 enemyPos = enemy.transform.position;
        Vector3 Vec = transform.position - enemyPos;

        rigid.AddForce(Vec.normalized * 10, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f); // 피격 0.5초

        sr.material.color = Color.white;
        ani.SetBool("Hit", false);

    }

    void Dead()
    {
        if (HP <= 0)
        {
            ani.SetTrigger("Dead");
        }
    }

    void EnableCollider()
    {
        playerCollider.enabled = true;
    }

    void UnableCollider()
    {
        playerCollider.enabled = false;
    }


    void SetFalseisAttack()
    {
        isAttack = false;
    }

    void ParticlePlay()
    {
        //Particle.ParticlePlay();
    }

    void ParticleStop()
    {
        //Particle.ParticleStop();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("충돌");
        switch (other.tag)
        { 
            case "Potion":
            case "Bojo":
                Debug.Log("보조");
                bool TryAddInventory = Inventory.AddSobi(other.gameObject);
                
                if (TryAddInventory)
                {
                    Debug.Log("DZ");
                    other.gameObject.transform.position = new Vector3(999f, 999f, 0);
                    Inventory.canAddItem = false;
                    StartCoroutine(Inventory.StartAddItemCooldown());
                }
                break;
            case "RollPaper":
                bool TryAddRollPaper = Inventory.AddRollPaper(other.gameObject);
                if (TryAddRollPaper)
                {
                    other.gameObject.SetActive(false);
                    Inventory.canAddItem = false;
                    StartCoroutine(Inventory.StartAddItemCooldown());
                }
                break;
            case "Accessory":
                bool TryAddAccessory = Inventory.AddAccessory(other.gameObject);
                
                if (TryAddAccessory)
                {
                    other.gameObject.SetActive(false);
                    Inventory.canAddItem = false;
                    StartCoroutine(Inventory.StartAddItemCooldown());
                }
                break;
            case "Weapon":
                bool TryChnageWeapon = Inventory.ChangeWeapon(other.gameObject);
                if (TryChnageWeapon)
                {
                    Inventory.canAddItem = false;
                    StartCoroutine(Inventory.StartAddItemCooldown());
                }
                break;
        }
    }

    private void TryUseSobiItem()
    {
        if (!Input.GetKey(KeyCode.F) && Inventory.canUseItem && Input.GetKey(KeyCode.Alpha1))
        {
            Inventory.UseSobiItem(0);
        }
        if (!Input.GetKey(KeyCode.F) && Inventory.canUseItem && Input.GetKey(KeyCode.Alpha2))
        {
            Inventory.UseSobiItem(1);
        }
        if (!Input.GetKey(KeyCode.F) && Inventory.canUseItem && Input.GetKey(KeyCode.Alpha3))
        {
            Inventory.UseSobiItem(2);
        }
    }

    private void TestItem()
    {
        if (Input.GetKey(KeyCode.Alpha5) && Inventory.canUseItem)
        {
            GameObject Apple = Instantiate(ApplePrefab, transform.position, Quaternion.identity);
            Inventory.canUseItem = false;
            StartCoroutine(Inventory.StartUseItemCooldown());
        }
        if (Input.GetKey(KeyCode.Alpha6) && Inventory.canUseItem)
        {
            GameObject RiceCake = Instantiate(RiceCakePrefab, transform.position, Quaternion.identity);
            Inventory.canUseItem = false;
            StartCoroutine(Inventory.StartUseItemCooldown());
        }
        if (Input.GetKey(KeyCode.Alpha7) && Inventory.canUseItem)
        {
            GameObject Yakgwa = Instantiate(YakgwaPrefab, transform.position, Quaternion.identity);
            Inventory.canUseItem = false;
            StartCoroutine(Inventory.StartUseItemCooldown());
        }
        if (Input.GetKey(KeyCode.Alpha8) && Inventory.canUseItem)
        {
            GameObject Trap = Instantiate(TrapPrefab, transform.position, Quaternion.identity);
            Inventory.canUseItem = false;
            StartCoroutine(Inventory.StartUseItemCooldown());
        }
        if (Input.GetKey(KeyCode.Alpha9) && Inventory.canUseItem)
        {
            GameObject Rock = Instantiate(RockPrefab, transform.position, Quaternion.identity);
            Inventory.canUseItem = false;
            StartCoroutine(Inventory.StartUseItemCooldown());
        }
        if (Input.GetKey(KeyCode.Alpha0) && Inventory.canUseItem)
        {
            GameObject RollPaper = Instantiate(RollPaperPrefab, transform.position, Quaternion.identity);
            Inventory.canUseItem = false;
            StartCoroutine(Inventory.StartUseItemCooldown());
        }
        if (Input.GetKey(KeyCode.U) && Inventory.canUseItem)
        {
            GameObject TestWeapon1 = Instantiate(WeponPrefab1, transform.position, Quaternion.identity);
            Inventory.canUseItem = false;
            StartCoroutine(Inventory.StartUseItemCooldown());
        }
        if (Input.GetKey(KeyCode.I) && Inventory.canUseItem)
        {
            GameObject TestWeapon2 = Instantiate(WeponPrefab2, transform.position, Quaternion.identity);
            Inventory.canUseItem = false;
            StartCoroutine(Inventory.StartUseItemCooldown());
        }

    }
}



