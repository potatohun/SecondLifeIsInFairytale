using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //충돌레이어로 플레이어말고는 안부딪히게해야댐
    //플레이어 대쉬할때 못먹음?
    // scriptable object로 아이템 데이터 저장?
    Rigidbody2D rigid;
    public int emptySlot;
    public GameObject ItemStatus;
    public bool isWatched;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        isWatched = false;
    }

    private void Update()
    {
        if (isWatched) //플레이어가 쳐다보면 status 창 on
        {
            ShowStatus();
            isWatched = false;
        }
        else
            CloseStatus();
    }

    public void ShowStatus()
    {
        Debug.Log(this.gameObject.name + "스탯창 ");
        ItemStatus.SetActive(true);
    }
    public void CloseStatus()
    {
        ItemStatus.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            for (emptySlot = 0; emptySlot < GameManager.gameManager.player.inventory.Length; emptySlot++)
            {

                if (GameManager.gameManager.player.inventory[emptySlot] == null)  
                break;
            
            }

            switch (this.gameObject.name)
            {

                case "Apple":
                    GameManager.gameManager.player.inventory[emptySlot] = this.gameObject;
                    gameObject.SetActive(false);
                    break;            
                case "RiceCake":
                    GameManager.gameManager.player.inventory[emptySlot] = this.gameObject;
                    gameObject.SetActive(false);
                    break;
                case "Yakgwa":
                    GameManager.gameManager.player.inventory[emptySlot] = this.gameObject;
                    gameObject.SetActive(false);
                    break;
                case "Weapon":
                    GameManager.gameManager.player.inventory[emptySlot] = this.gameObject;
                    gameObject.SetActive(false);
                    break;
                case "StrawShoes":
                    GameManager.gameManager.player.inventory[emptySlot] = this.gameObject;
                    gameObject.SetActive(false);
                    break;


            }
        }

    }
}


