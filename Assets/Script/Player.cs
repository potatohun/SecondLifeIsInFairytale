using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private static Player instance;
    Vector2 input;
    int HP = 100;
    public int speed=5;
    public float jumpPower=5;
    public float rollSpeed=1.5f;

    bool canDoubleJump = false;
    
    float curTIme;
    float coolTime = 0.5f;
    GameObject weapon;
   

    AudioSource weaponAudio;
    Animator ani,weaponAni;

    Rigidbody2D rigid;
    CapsuleCollider2D playerCollider;
  

    public Vector2 boxSize;
    public Transform pos;

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
        playerCollider=GetComponent<CapsuleCollider2D>();  
       
        weapon = GameObject.Find("Hand(Weapon)");
        weaponAni = weapon.GetComponent<Animator>();
        weaponAudio = weapon.GetComponent<AudioSource>();

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
        Jump();
        Attack();
        Roll();
        Dead();
    }
    void FixedUpdate()
    {      
       Move();

       Debug.DrawRay(rigid.position, dirvec * 2f, new Color(0, 1, 0));
       RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, dirvec, 2f, LayerMask.GetMask("Object"));
       if (rayhit.collider != null)
       {
           scanObj = rayhit.collider.gameObject;
       }
       else
           scanObj = null;
    }

    //오브젝트 스캔
    public GameObject ScanObj()
    {
        return scanObj;
    }

    void LateUpdate()
    {
        ani.SetFloat("speed", input.magnitude);
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        input = new Vector2(x, 0) * speed * Time.deltaTime;
        //   rigid.MovePosition(rigid.position + input); // move Vs moveposition 
        //transform.position = transform.position + new Vector3(x,0) * speed * Time.deltaTime;  딱히 조작감차이는없는듯 rigid>>>>>transform


        rigid.position = rigid.position + input;

       if (!weaponAni.GetBool("Attack"))
        {
            if (x < 0)
                transform.localScale = new Vector3(-3.5f, 3.5f, 0);
            else if (x > 0)
                transform.localScale = new Vector3(3.5f, 3.5f, 0);
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
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            canDoubleJump = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "ground")
            ani.SetBool("isJump", false);

        //이게 foot의 콜라이더떄문에 플레이어콜라이더가 꺼져도 피가 닳음
        if (collision.gameObject.tag == "Enemy")
        {

            HP -= collision.gameObject.GetComponent<Enemy>().Attack;

            Debug.Log(" Player HP :" + HP);
            ani.SetTrigger("Hit");
            StartCoroutine(KnockBack());
        }
    }

    void Attack()
    {
        if (curTIme <= 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && weapon.activeSelf)
            {
               
                weaponAni.SetBool("Attack",true);
                weaponAudio.Play();
                curTIme = coolTime;

                Collider2D[] enemy = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

                foreach (Collider2D collider in enemy)
                {
                    if (collider.tag == "Monster")
                    {
                        //몬스터 데미지 주는 코드 필요
                        Destroy(collider.gameObject); //임시로 삭제
                    }
                    else if (collider.tag == "Boss") //보스 때릴때
                    {
                        Boss boss = collider.GetComponent<Boss>();
                        boss.isDie = true;
                        //Destroy(collider.gameObject); //임시로 삭제
                    }
                }
              
            }
        }
        else 
            curTIme -= Time.deltaTime;

       // 데미지주는 함수르 따로만들고 애니메이션 끝족에 이벤트로 넣으라는거지
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }


    void Roll()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !weaponAni.GetBool("Attack")) 
        {
            float lookingDir = transform.localScale.x;
            ani.SetTrigger("Roll");

            rigid.velocity = Vector2.zero;
            rigid.velocity = new Vector2(lookingDir, 0) * rollSpeed;
            
           
             
        }
    }




   IEnumerator KnockBack()
    {
        yield return null;
        Vector3 enemyPos = GameManager.gameManager.enemy.transform.position;
        Vector3 Vec =transform.position - enemyPos;
        rigid.AddForce(Vec.normalized * 6, ForceMode2D.Impulse);

    }


    void Dead()
    {
        if(HP<=0)
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

    void SetActiveF()
    {
        gameObject.SetActive(false);
    }

    void SetWActiveF()
    {
        weapon.SetActive(false);
    }

    void SetWActiveT()
    {
        weapon.SetActive(true);
    }

}



