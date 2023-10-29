using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public Player player;
    public Enemy enemy;

    public static int level;
    public static int maxhp;
    public static int hp;
    public static int attack;

    public static int maxVerse;
    private static int chapter;
    private static int verse;


    private void Awake()
    {
        gameManager = this;
    }

    public static GameManager gamemanager
    {
        get
        {
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();

                // 씬 전환 시에도 유지되도록 하기 위해 인스턴스가 존재하지 않으면 생성
                if (gameManager == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    gameManager = obj.AddComponent<GameManager>();
                }

                DontDestroyOnLoad(gameManager.gameObject);
            }

            return gameManager;
        }
    }

    public void Chapter2Setting()
    {
        chapter = 2;
        verse = 1;
        maxVerse = 6;
    }

    public void Chapter1Setting()
    {
        chapter = 1;
        verse = 1;
        maxVerse = 1;
    }

    public void VersePlus()
    {
        verse++;
    }
    public int GetChapter()
    {
        return chapter;
    }
    public int GetVerse()
    {
        return verse;
    }
    public void SetVerse(int i)
    {
        verse = i;
    }
    public void SetChapter(int i)
    {
        chapter = i;
    }
}
