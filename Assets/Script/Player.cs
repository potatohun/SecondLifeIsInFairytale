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
    public GameObject[] inventory = new GameObject[3]; // 이새기 왜 맨밑으로 내리면 안됨????????????????????이해가안되네진짜.
    public GameObject[] accessory = new GameObject[2];

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

    public Particle Particle;

    public float potionCoolTime = 2f;
    public bool canHpPotionDrink = true;

    public GameObject weapon;

    public Vector3 dirvec;
    public GameObject scanObj;
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

    }
    void Update()
    {
        RaycastHit2D rayhit;

        if (transform.localScale.x > 0)
        {
            Debug.DrawRay(transform.position + new Vector3(1, 0, 0), Vector3.forward*20f, Color.green);
            rayhit = Physics2D.Raycast(transform.position + new Vector3(1, 0, 0), Vector3.forward*20f, LayerMask.GetMask("Object"));
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(-1, 0, 0), Vector3.back*20f, Color.green);
            rayhit = Physics2D.Raycast(transform.position + new Vector3(-1, 0, 0), Vector3.back*20f, LayerMask.GetMask("Object"));
        }
      
        if (rayhit.collider != null)
        {
            scanObj = rayhit.collider.gameObject;
            if (scanObj.layer == LayerMask.NameToLayer("Item")){
                Item item = scanObj.GetComponent<Item>();
                item.isWatched = true; //아이템 쳐다보는중
            }
        }
        else
            scanObj = null;

        Jump();
        ComboAttack();
        Roll();
        Dead();
        UseItem();
    }
    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        ani.SetFloat("speed", input.magnitude);
    }

    void UseItem()
    {
        UsePotion(KeyCode.Alpha1, 0);
        UsePotion(KeyCode.Alpha2, 1);
        UsePotion(KeyCode.Alpha3, 2);
    }

    void UsePotion(KeyCode key, int slotNumber) // 원래 ref썼었음 callbyreference?
    {
        if (!Input.GetKey(KeyCode.F) && Input.GetKeyDown(key))
        {
            if (canHpPotionDrink)
            {

                if (inventory[slotNumber] == null)
                    return;


                GameObject what = inventory[slotNumber].gameObject;

                if (HP == MAXHP && !what.name.Equals("Yakgwa"))
                {
                    Debug.Log("풀피에용");
                    return;
                }

                switch (what.name)
                {

                    case "Apple":
                        HP += 5;
                        break;
                    case "RiceCake":
                        HP += 15;
                        break;
                    case "Yakgwa":
                        {
                            MAXHP += 20;
                            HP += 10;
                            moveSpeed += 10;
                        }
                        break;
                }


                if (HP > MAXHP)
                    HP = MAXHP;

                Debug.Log("Player HP : " + HP);
                canHpPotionDrink = false;

                inventory[slotNumber] = null;
                Destroy(what);
                StartCoroutine(PotionDelay(value => canHpPotionDrink = value));

            }
            else
                Debug.Log("쿨타임이에용ㅋ");
        }
    }

    IEnumerator PotionDelay(Action<bool> setBool) // 매개변수를 이렇게 한 이유는 , 원래 포션 딜레이 종류가 두개였는데 사용 포션종류를 매개변수로 받아서 구분지으려했음, 근데 스피드포션없애서 있으나마나됨 아무튼
    {
        yield return new WaitForSeconds(potionCoolTime);
        setBool(true);
    }

    void changeWeapon(int slotNumber)
    {

        GameObject what = inventory[slotNumber].gameObject;
        inventory[slotNumber] = null;


        what.transform.position = weapon.transform.position;
        what.transform.rotation = weapon.transform.rotation;
        what.transform.localScale = weapon.transform.localScale;

        weapon.transform.SetParent(null);
        //  what.transform.SetParent(hand.transform);

        what.gameObject.SetActive(true);
        Destroy(weapon);
    }


    void EquipAccessory(int slotNumber)
    {

        GameObject acc = inventory[slotNumber].gameObject;
        inventory[slotNumber] = null;

        int accSlotNum = 0;

        for (int i = 0; i < accessory.Length; i++)
        {
            if (accessory[i] == null) // 빈 슬롯 찾기
            {
                accSlotNum = i;

                if (acc.name.Equals("StrawShoes"))
                {
                    moveSpeed += 10;
                }
                else if (acc.name.Equals("Yeomju"))
                {
                    MAXHP += 10;
                    //스트렝스,공속
                }
                break;
            }
            else // 빈곳없으면 첫번째칸 고정
            {
                accSlotNum = 0;

                if (accessory[accSlotNum].name.Equals("StrawShoes")) // 첫 칸 악세 능력치빼고, 착용할거 더해주기
                {
                    moveSpeed -= 10;
                }
                else if (accessory[accSlotNum].name.Equals("Yeomju"))
                {
                    MAXHP -= 10;
                }


                if (acc.name.Equals("StrawShoes"))
                {
                    moveSpeed += 10;
                }
                else if (acc.name.Equals("Yeomju"))
                {
                    MAXHP += 10;
                    //스트렝스,공속
                }


            }
        }

        accessory[accSlotNum] = acc; // 착용은 했는데 . .. 능력치는 어떻게 입히지 // 프레임단위로 accessory배열 확인하는건 어때

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
        foreach (Collider2D collider in enemy)
        {
            if (collider.tag == "Enemy")
                collider.GetComponent<Enemy>().TakeDamage(10);//데미지 어케함                     
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
        Particle.ParticlePlay();
    }

    void ParticleStop()
    {
        Particle.ParticleStop();

    }
}



