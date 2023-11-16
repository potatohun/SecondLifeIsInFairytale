using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public Player player;

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
}
