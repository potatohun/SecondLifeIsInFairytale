using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    public TMP_Text talkText;
    public GameObject textBox;
    public bool moveAble;
    public Player player;
    public Image fade;
    public Text chapterText;
    public MapController mapController;

    private void Awake()
    {
    }
    private void Start()
    {
        // 맵 진입시 뜨는 UI 관리
        chapterText.text = SceneManager.GetActiveScene().name;
        fade.gameObject.SetActive(true);
        chapterText.gameObject.SetActive(true);

        StartCoroutine(FadeIn());                     //코루틴    //판넬 투명도 조절
        talkText = textBox.GetComponentInChildren<TMP_Text>();
        textBox.gameObject.SetActive(false);
        moveAble = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        mapController = GameObject.FindGameObjectWithTag("MapController").GetComponent<MapController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) /*&& moveAble == true*/)
        {
            if (player.ScanObj().gameObject.CompareTag("Npc"))
            {
                Debug.Log("NPC와 대화");
                TalkWithNPC();
            }
            else if (player.ScanObj().gameObject.CompareTag("1Page"))
            {
                //1포탈일때
                Move1Page();
            }
            else if (player.ScanObj().gameObject.CompareTag("2Page"))
            {
                //2포탈일때
                Move2Page();
            }
            else if (player.ScanObj().gameObject.CompareTag("Portal"))
            {
                //그냥 포탈일때
                mapController.MapChange();
                StartCoroutine(FadeIn());
                //MovePortal();
            }
        }
    }

    public void Move1Page()
    {
        moveAble = false;
        SceneManager.LoadScene("1Page");
    }
    public void Move2Page()
    {
        moveAble = false;
        SceneManager.LoadScene("2Page");
    }
    public void MovePortal()
    {
        int num = GameManager.gameManager.GetVerse();

        if (num == GameManager.maxVerse) //보스
        {
            moveAble = false;
            string scenes = "2page-boss";
            Debug.Log(scenes);
            SceneManager.LoadScene(scenes);
        }
        else // 일반
        {
            moveAble = false;
            int randomNumber = Random.Range(1, 6);
            string scenes = "2page-" + randomNumber;
            Debug.Log(scenes);
            SceneManager.LoadScene(scenes);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("새로운 씬이 로드되었습니다: " + scene.name);
    }
    IEnumerator FadeIn()
    {
        float fadeCount = 1;
        while (fadeCount > 0.0f)
        {
            fadeCount -= 0.01f;
            fade.color = new Color(0, 0, 0, fadeCount);
            chapterText.color = new Color(1, 1, 1, fadeCount);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void TalkWithNPC()
    {
        Npc npc = player.ScanObj().gameObject.GetComponent<Npc>();

        if (npc.talk_count == npc.words.Count)
        {
            textBox.SetActive(false);
            npc.talk_count = 0;
        }
        else
        {
            textBox.SetActive(true);
            talkText.text = npc.words[npc.talk_count++];
        }
    }
}