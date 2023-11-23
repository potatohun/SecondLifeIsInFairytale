using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class Item : MonoBehaviour
{   
    public ItemData item;
    public Player player;
    protected GameObject inventory;
    protected InventoryManager inventoryManager;
    
    protected float activationDistance = 1.0f;

    public GameObject ItemStatus;

    protected void Awake()
    {   
        StartCoroutine(MoveUpDown());
        Vector3 ItemStatusPos = transform.position + new Vector3(0f, 3.75f, 0f);
        ItemStatus = Instantiate(item.itemStatusPrefab, ItemStatusPos, Quaternion.identity);
        ItemStatus.transform.parent = this.transform;
        ItemStatus.SetActive(false);

        SetItemStatusInfo(ItemStatus, item.itemName,item.itemIamge,item.itemEffect,item.itemTooltip);

        inventory = GameObject.Find("Inventory");
        inventoryManager = inventory.GetComponent<InventoryManager>();
        player= inventoryManager.Player;
    }

    protected virtual void Update()
    {
        float distance = Vector3.Distance(transform.position, inventory.transform.position);

        if (ItemStatus != null &&distance <= activationDistance)
        {  
            ItemStatus.SetActive(true);
        }
        else 
        {   
            if(ItemStatus != null)
                ItemStatus.SetActive(false);
        }
    }

    private void SetItemStatusInfo(GameObject itemStatus,string itemName, Sprite itemImage,string itemEffect, string itemTooltip)
    {
        TextMeshProUGUI Name = itemStatus.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        Image Icon = itemStatus.transform.Find("Icon").GetComponent<Image>();
        TextMeshProUGUI Effect = itemStatus.transform.Find("Effect").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Tootip = itemStatus.transform.Find("Tooltip").GetComponent<TextMeshProUGUI>();

        Name.text=itemName;
        Icon.sprite = itemImage;
        Effect.text=itemEffect;
        Tootip.text = itemTooltip;
    }
    IEnumerator MoveUpDown()
    {
        float moveDistance = 0.15f;
        float duration = 0.6f;

        while (true)
        {
            // 아래로 이동
            yield return MoveY(transform.localPosition.y - moveDistance, duration);

            // 잠시 대기
            yield return new WaitForSeconds(0.5f);

            // 위로 이동
            yield return MoveY(transform.localPosition.y + moveDistance, duration);

            // 잠시 대기
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator MoveY(float targetY, float duration)
    {
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = new Vector3(startPosition.x, targetY, startPosition.z);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확한 위치로 설정
        transform.localPosition = endPosition;
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
}