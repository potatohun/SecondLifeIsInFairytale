using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player instance;

    public int HP = 100;
    public int MAXHP = 100;
    public int moveSpeed = 5;
    public float jumpPower = 5;
    public float attackSpeed = 10;
    public int damage = 40;
    public Rigidbody2D rigid;
    public CapsuleCollider2D playerCollider;
    public SpriteRenderer sr;
    public Animator ani;
    public AudioSource audio;


    public int weapondamage = 10;
    public WeaponData.WeaponType weaponType;
    public bool canTakeDamage = true;


    //����
    public bool isSeeRight = true;

    public bool createItem = true;

    public GameObject AmuletPrefab;
    public GameObject ApplePrefab;
    public GameObject RiceCakePrefab;
    public GameObject YakgwaPrefab;
    public GameObject TrapPrefab;
    public GameObject RockPrefab;

    public GameObject StrawShouesPrefab;
    public GameObject MillBootsPrefab;
    public GameObject PaePrefab;
    public GameObject NovelHatPrefab;

    public GameObject NomalPrefab;
    public GameObject BigPrefab;
    public GameObject FirePrefab;
    public GameObject IcePrefab;
    public GameObject BloodPrefab;

    public float maxVelocityX;



    public bool canDead = true;
    public int haveAmulet = 0;
    //
    public GameObject inventory;
    public InventoryManager inventoryManager;

    private void TestItem()
    {
        if (createItem && Input.GetKey(KeyCode.Alpha4))
        {
            GameObject Amulet = Instantiate(NomalPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha5))
        {
            GameObject Apple = Instantiate(BigPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }

        if (createItem && Input.GetKey(KeyCode.Alpha6))
        {
            GameObject Rock = Instantiate(FirePrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha7))
        {
            GameObject StrawShoues = Instantiate(IcePrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha8))
        {
            GameObject Hat = Instantiate(BloodPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha9))
        {
            GameObject Weapon = Instantiate(RockPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
    }
 
    public IEnumerator test()
    {
        yield return new WaitForSeconds(1.0f);
        createItem = true;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // sceneLoaded 이벤트에 OnSceneLoaded 메소드를 연결
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.gameObject.transform.position = new Vector3(0, 1, 0); // 씬이 로드될 때마다 Player 위치 (0,0,0)으로 초기화.
    }
    //
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // 오브젝트를 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복 생성된 것이므로 이 오브젝트를 파괴
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        TestItem();
        if(this.transform.localScale.x>0) isSeeRight = true;
        else isSeeRight = false;
    }

    public float CalcDamage()
    {
        Debug.Log((damage + weapondamage) + (damage + weapondamage) * UnityEngine.Random.Range(0f, 0.1f));
        return (damage + weapondamage) + (damage + weapondamage) * UnityEngine.Random.Range(0f, 0.11f);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
            limitMoveSpeed();
    }
    void limitMoveSpeed()
    {
        if (rigid.velocity.x > maxVelocityX)
        {
            rigid.velocity = new Vector2(maxVelocityX, rigid.velocity.y);
        }
        if (rigid.velocity.x < (maxVelocityX * -1))
        {
            rigid.velocity = new Vector2((maxVelocityX * -1), rigid.velocity.y);
        }

    }

}
