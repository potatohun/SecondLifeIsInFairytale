using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    CardData[] carddata;
    public GameObject Icon;
    public GameObject title;
    public GameObject text;

    int randomIndex;
    int cost;

    public Transform cardScale;

    Vector3 defaultScale;

    public Animator animator;

    private void Start()
    {
        //랜덤 1~10 값 설정
        cost = Random.Range(1, 4);
        carddata = CardManager.instance.cardData;
        defaultScale = cardScale.localScale;
        this.SetIcon();
        Debug.Log(cost);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("OnCursor", true);
        Debug.Log("커서 가져다댐");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("OnCursor", false);
        Debug.Log("커서 뗌");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("클릭함");
        if (PlayerPrefs.GetInt("RollPaper") >= cost)
        {
            bool invenfull = true;
            for(int i = 0; i < 3; i++)
            {
                if (Player.instance.inventoryManager.inventory[i] == null)
                {
                    invenfull = false;
                }
            }
            if(invenfull) 
            {
                Debug.Log("인벤토리가 꽉찼습니다.");
            }
            else
            {
                carddata[0].UseItem(carddata[randomIndex].useType, title.GetComponent<Text>().text, cost);
                PlayerPrefs.SetInt("RollPaper", PlayerPrefs.GetInt("RollPaper") - cost);
                GameObject parentObject = this.gameObject.transform.parent.gameObject;
                parentObject.SetActive(false);
                Debug.Log(SceneManager.GetActiveScene().name);
                switch (SceneManager.GetActiveScene().name)
                {
                    case "1장":
                        SceneManager.LoadScene("1장엔딩");
                        break;
                    case "2장":
                        SceneManager.LoadScene("2장엔딩");
                        break;
                    case "3장":
                        SceneManager.LoadScene("3장엔딩");
                        break;
                }
                
            }
        }
        else
        {
            Debug.Log("두루마기가 부족합니다.");
        }
    }

    private void SetIcon()
    {
        if (carddata.Length > 0)
        {
            randomIndex = Random.Range(0, carddata.Length);
            Image imageComponent = Icon.GetComponent<Image>();
            imageComponent.sprite = carddata[randomIndex].icon;
            Text titleComponent = title.GetComponent<Text>();
            titleComponent.text = carddata[randomIndex].title;
            Text textComponent = text.GetComponent<Text>();
            textComponent.text = carddata[randomIndex].text;
            textComponent.text = textComponent.text + " " + cost;
        }
        else
        {
            Debug.Log("저장된 이미지가 없습니다.");
        }
    }
}
