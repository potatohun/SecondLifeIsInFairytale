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
        //마우스 커서 가져다 대면 1.1배 커짐
        cardScale.localScale = defaultScale * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //마우스 커서 떼면 다시 기본 사이즈로
        cardScale.localScale = defaultScale * 1.0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(PlayerPrefs.GetInt("RollPaper") >= cost)
        {
            carddata[0].UseItem(carddata[randomIndex].useType, cost);
            PlayerPrefs.SetInt("RollPaper", PlayerPrefs.GetInt("RollPaper") - cost);
            GameObject parentObject = this.gameObject.transform.parent.gameObject;
            parentObject.SetActive(false);
            SceneManager.LoadScene("마을");
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
