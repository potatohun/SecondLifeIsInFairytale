using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    public static TalkManager instance;
    public TMP_Text talkText;
    public GameObject textBox;
    public bool moveAble;
    public Player player;
    public Image fade;

    //private bool checkbool = false;

    void Awake()
    { instance = this; 
      fade.gameObject.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(FadeIn());                     //코루틴    //판넬 투명도 조절
        talkText = textBox.GetComponentInChildren<TMP_Text>();
        textBox.gameObject.SetActive(false);
        moveAble = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F) && moveAble == true)
        {
            if (player.ScanObj().gameObject.CompareTag("1Page"))
            {
                Move1Page();
            }
            else if (player.ScanObj().gameObject.CompareTag("2Page"))
            {
                Move2Page();
            }
            else if (player.ScanObj().gameObject.CompareTag("Portal"))
            {
                MovePortal();
            }
        }

        if (player.ScanObj() != null)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                if (player.ScanObj().gameObject.CompareTag("1Page")){
                    Debug.Log("1Page 감지");
                    Page1Action();
                }
                else if (player.ScanObj().gameObject.CompareTag("2Page"))
                {
                    Debug.Log("2Page 감지");
                    Page2Action();
                }
                else if (player.ScanObj().gameObject.CompareTag("Portal"))
                {
                    Debug.Log("Portal 감지");
                    PortalAction();
                }
            }
        }
    }
    public void Page1Action()
    {
        textBox.gameObject.SetActive(true);
        talkText.text = "1 Page portal.. " +
            "press F";
        moveAble = true;
    }
    public void Page2Action()
    {
        textBox.gameObject.SetActive(true);
        talkText.text = "2 Page portal.. " +
            "press F";
        moveAble = true;
    }
    public void PortalAction()
    {
        textBox.gameObject.SetActive(true);
        talkText.text = "This is Portal.. " +
            "press F";
        moveAble = true;
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
        int num = GameManager.gameManager.GetChapter();
        if(num > GameManager.gameManager.maxChapter)
        {
            moveAble = false;
            string chapter = "2page-boss";
            Debug.Log(chapter);
            SceneManager.LoadScene(chapter);
            GameManager.gameManager.currentChapter = 1;
        }
        else
        {
            moveAble = false;
            int randomNumber = Random.Range(1, 1);
            GameManager.gameManager.ChapterPlus();
            string chapter = "2page-" + randomNumber;
            Debug.Log(chapter);
            SceneManager.LoadScene(chapter);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {    
        // 씬 로드 후에 호출될 함수를 여기에 작성
        Debug.Log("새로운 씬이 로드되었습니다: " + scene.name);
        //GameManager.gamemanager.Pozol_Spawn();
        //GameManager.gamemanager.ArrowPozol_Spawn();
    }
    IEnumerator FadeIn()
    {
        float fadeCount = 1;
        while (fadeCount > 0.0f)
        {
            fadeCount -= 0.01f;
            fade.color = new Color(0, 0, 0, fadeCount);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator FadeOut()
    {
        float fadeCount = 0;
        while(fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fade.color = new Color(0, 0, 0, fadeCount);
        }
    }
    
}