using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sobi : Item
{
    public Image coolTimePrefab; // Image 컴포넌트를 가진 UI 이미지 프리팹
    protected Image coolTimeInstance; // 생성된 UI 이미지를 저장할 변수

    protected float cooldownTime = 1.0f;
    protected float currentCooldown = 0.0f;
    public bool isCoolingDown = false;

    protected override void Update()
    {   
        base.Update();
        //아이템 사용시 돌아가는 메소드
        if (isCoolingDown)
        {   
            ResetOtherItemsCooldown();
        }
    }
    protected void useItem()
    {  
        Destroy(uiInstance.gameObject);
        Destroy(coolTimeInstance.gameObject);
        currentCooldown = 0.0f;
        foreach (Image itemUI in Inventory.Items)
        {   
            if(itemUI!=null)itemUI.fillAmount=1.0f;
           
        }
        isCoolingDown=true;
    }
    protected void ResetOtherItemsCooldown()
    {   
        currentCooldown += Time.deltaTime;
        
        foreach (Image itemUI in Inventory. Items)
        {   
            
            if(itemUI!=null)itemUI.fillAmount = 1-currentCooldown;
        }
         if (currentCooldown >= cooldownTime)
        {
            isCoolingDown = false;
            if(this.gameObject.tag=="Potion")Destroy(this.gameObject);
    
        }
    }
    public override void AddUI(float index)
    {   
        base.AddUI(index);
        coolTimeInstance = Instantiate(coolTimePrefab);
        coolTimeInstance.rectTransform.position = SlotPostion.position;
        coolTimeInstance.rectTransform.localScale = new Vector3(0.345f, 0.345f,0f);

        //쿨탐용 이미지는 안보이게 꺼놓기
        coolTimeInstance.transform.SetParent(targetCanvas.transform);
        coolTimeInstance.fillAmount=0f;

    }   
}
