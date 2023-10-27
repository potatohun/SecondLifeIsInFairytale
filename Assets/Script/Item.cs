using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Item: MonoBehaviour
{
    public GameObject ItemStatusPrefab;
    protected GameObject ItemStatus;
    protected Vector3 ItemStatusPos;

    protected Transform playerTransform;

    protected float activationDistance = 1.0f;

    //
    public Image uiPrefab; // Image 컴포넌트를 가진 UI 이미지 프리팹
    protected Image uiInstance; // 생성된 UI 이미지를 저장할 변수

    protected Canvas targetCanvas;
    protected RectTransform SlotPostion;


    protected GameObject Player;
    protected Player playerScript;
    protected InventroyManager Inventory;

    protected void Awake()
    {
        ItemStatusPos = transform.position + new Vector3(0f, 3.0f, 0f);
        ItemStatus = Instantiate(ItemStatusPrefab, ItemStatusPos, Quaternion.identity);
        ItemStatus.transform.parent = this.transform;
        ItemStatus.SetActive(false);

        Player = GameObject.Find("Player");
        playerScript = Player.GetComponent<Player>();
        playerTransform = Player.transform;
        Inventory = Player.GetComponentInChildren<InventroyManager>();
    }

    protected virtual void Update()
    {
        //아이템 스테이터스 띄우는 조건
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (ItemStatus != null && distance <= activationDistance)
        {
            ItemStatus.SetActive(true);
        }
        else
        {
            if (ItemStatus != null)
                ItemStatus.SetActive(false);
        }

    }
    public virtual void AddUI(float index)
    {
        //캔버스 찾아서
        targetCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        SlotPostion = GameObject.Find("Slot" + index).GetComponent<RectTransform>();
        //해당 위치에 ui이미지 생성
        uiInstance = Instantiate(uiPrefab);
        uiInstance.rectTransform.position = SlotPostion.position;
        uiInstance.rectTransform.localScale = new Vector3(0.345f, 0.345f, 0f);
        uiInstance.transform.SetParent(targetCanvas.transform);

    }
    protected void OnDisable()
    {
        if (ItemStatus != null)
        {
            ItemStatus.SetActive(false);
        }

    }

    protected void OnDestroy()
    {
        if (ItemStatus != null)
        {
            Destroy(ItemStatus);
        }
    }
    /*
    protected void letsCooldown()
    {
        coolTimeInstance.fillAmount=1.0f;
        currentCooldown = 0.0f;
        isCoolingDown=true;
    }
    protected void UpdateCooldown()
    {
        
        currentCooldown += Time.deltaTime;
        coolTimeInstance.fillAmount = 1-currentCooldown;
        if (currentCooldown >= cooldownTime)
        {
            isCoolingDown = false;
            Destroy(uiInstance);
            Destroy(coolTimeInstance);
            if(this.gameObject.tag=="Potion")Destroy(this.gameObject);
    
        }
    }
    */
}