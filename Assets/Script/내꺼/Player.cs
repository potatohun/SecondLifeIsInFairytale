using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private static Player instance;

    public int HP = 100;
    public int MAXHP = 100;
    public int moveSpeed = 5;
    public float jumpPower = 5;
    public float rollSpeed = 3;
    public float attackSpeed = 10;
    public int damage = 10;
    public Rigidbody2D rigid;
    public CapsuleCollider2D playerCollider;
    public SpriteRenderer sr;
    public Animator ani;
    public AudioSource audio;
   

    public bool canTakeDamage = true;


    //동진
    public bool isSeegRight = true;

    public bool createItem = true;
    public GameObject ApplePrefab;
    public GameObject StrawShouesPrefab;
    public GameObject YakgwaPrefab;
    public GameObject TrapPrefab;
    public GameObject RockPrefab;
    public GameObject RollPaperPrefab;
    public GameObject WeaponPrefab;
    public GameObject HatPrefab;
    public GameObject AmuletPrefab;

    public float x;
    public bool canDead = true;
    public int haveAmulet = 0;
    //
    public GameObject inventory;
    public InventoryManager inventoryManager;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // sceneLoaded 이벤트에 OnSceneLoaded 메소드를 연결
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.transform.position = Vector3.zero + new Vector3(0,1,0); // 씬이 로드될 때마다 Player 위치 (0,0,0)으로 초기화.
    }


    private void TestItem()
    {
        if (createItem && Input.GetKey(KeyCode.Alpha4))
        {
            GameObject Amulet = Instantiate(AmuletPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha5))
        {
            GameObject Apple = Instantiate(ApplePrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }

        if (createItem && Input.GetKey(KeyCode.Alpha6))
        {
            GameObject Rock = Instantiate(RockPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha7))
        {
            GameObject StrawShoues = Instantiate(StrawShouesPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha8))
        {
            GameObject Hat = Instantiate(HatPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
        if (createItem && Input.GetKey(KeyCode.Alpha9))
        {
            GameObject Weapon = Instantiate(WeaponPrefab, transform.position, Quaternion.identity);
            createItem = false;
            StartCoroutine(test());
        }
    }
 
    public IEnumerator test()
    {
        yield return new WaitForSeconds(1.0f);
        createItem = true;
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
            DontDestroyOnLoad(gameObject); // 오브젝트를 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복 생성된 것이므로 이 오브젝트를 파괴
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        TestItem();
    }
}
