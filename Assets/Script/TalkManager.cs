using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    public Text talkText;
    public GameObject textBox;
    public bool moveAble;
    public PlayerScanner playerscanner;
    public Image fade;
    public Text chapterText;
    public MapController mapController;

    private void Awake()
    {
    }
    private void Start()
    {
        StartCoroutine(FadeIn());                     //코루틴    //판넬 투명도 조절
        talkText = textBox.GetComponentInChildren<Text>();
        textBox.gameObject.SetActive(false);
        moveAble = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        playerscanner = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScanner>();
        mapController = GameObject.FindGameObjectWithTag("MapController").GetComponent<MapController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerscanner.ScanObj() != null)
        {
            if (playerscanner.ScanObj().gameObject.CompareTag("Npc"))
            {
                Debug.Log("NPC와 대화");
                TalkWithNPC();
            }
            else if (playerscanner.ScanObj().gameObject.CompareTag("1PageIntro"))
            {
                //1포탈일때
                Move1PageIntro();
                StartCoroutine(FadeIn());
            }
            else if (playerscanner.ScanObj().gameObject.CompareTag("2PageIntro"))
            {
                //2포탈일때
                Move2PageIntro();
                StartCoroutine(FadeIn());
            }
            else if (playerscanner.ScanObj().gameObject.CompareTag("3PageIntro"))
            {
                //3포탈일때
                Move3PageIntro();
                StartCoroutine(FadeIn());
            }
            else if (playerscanner.ScanObj().gameObject.CompareTag("1Page"))
            {
                //1포탈일때
                Move1Page();
                StartCoroutine(FadeIn());
            }
            else if (playerscanner.ScanObj().gameObject.CompareTag("2Page"))
            {
                //2포탈일때
                Move2Page();
                StartCoroutine(FadeIn());
            }
            else if (playerscanner.ScanObj().gameObject.CompareTag("3Page"))
            {
                //3포탈일때
                Move3Page();
                StartCoroutine(FadeIn());
            }
            else if (playerscanner.ScanObj().gameObject.CompareTag("Ending"))
            {
                //엔딩포탈일때
                MoveEnding();
                StartCoroutine(FadeIn());
            }
            else if (playerscanner.ScanObj().gameObject.CompareTag("Portal"))
            {
                //그냥 포탈일때
                mapController.MapChange();
                PlayerPrefs.SetInt("CurrentVerse", PlayerPrefs.GetInt("CurrentVerse") + 1);
                //chapterText.text = PlayerPrefs.GetInt("CurrentChapter").ToString() + "장" + PlayerPrefs.GetInt("CurrentVerse").ToString() + "절";
                StartCoroutine(FadeIn());
                //MovePortal();
            }
        }
    }

    public void Move1PageIntro()
    {
        PlayerPrefs.SetInt("CurrentChapter", 1);
        PlayerPrefs.SetInt("CurrentVerse", 1);
        moveAble = false;
        SceneManager.LoadScene("1장시작");
    }
    public void Move2PageIntro()
    {
        PlayerPrefs.SetInt("CurrentChapter", 2);
        PlayerPrefs.SetInt("CurrentVerse", 1);
        moveAble = false;
        SceneManager.LoadScene("2장시작");
    }
    public void Move3PageIntro()
    {
        PlayerPrefs.SetInt("CurrentChapter", 3);
        PlayerPrefs.SetInt("CurrentVerse", 1);
        moveAble = false;
        SceneManager.LoadScene("3장시작");
    }
    public void Move1Page()
    {
        PlayerPrefs.SetInt("CurrentChapter", 1);
        PlayerPrefs.SetInt("CurrentVerse", 1);
        moveAble = false;
        SceneManager.LoadScene("1장");
    }
    public void Move2Page()
    {
        PlayerPrefs.SetInt("CurrentChapter", 2);
        PlayerPrefs.SetInt("CurrentVerse", 1);
        moveAble = false;
        SceneManager.LoadScene("2장");
    }
    public void Move3Page()
    {
        PlayerPrefs.SetInt("CurrentChapter", 3);
        PlayerPrefs.SetInt("CurrentVerse", 1);
        moveAble = false;
        SceneManager.LoadScene("3장");
    }
    public void MoveEnding()
    {
        SceneManager.LoadScene("마지막");
        Player.instance.gameObject.SetActive(false);
        Canvas.instance.gameObject.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
        Debug.Log("새로운 씬이 로드되었습니다: " + scene.name);
        mapController = GameObject.FindGameObjectWithTag("MapController").GetComponent<MapController>();
    }
    IEnumerator FadeIn()
    {
        // 맵 진입시 뜨는 UI 관리
        if (SceneManager.GetActiveScene().name == "마을" || SceneManager.GetActiveScene().name == "시작")
        {
            //chapterText.text = SceneManager.GetActiveScene().name;
        }
        else
        {
            //chapterText.text = SceneManager.GetActiveScene().name + PlayerPrefs.GetInt("CurrentVerse").ToString() + "절";
        }

        fade.gameObject.SetActive(true);
        //chapterText.gameObject.SetActive(true);
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
        Npc npc = playerscanner.ScanObj().gameObject.GetComponent<Npc>();

        /*if (npc.talk_count == npc.words.Count)
        {
            textBox.SetActive(false);
            npc.talk_count = 0;
        }
        else
        {
            textBox.SetActive(true);
            talkText.text = npc.words[npc.talk_count++];
        }*/

        npc.PlayCineMachine();
    }
}