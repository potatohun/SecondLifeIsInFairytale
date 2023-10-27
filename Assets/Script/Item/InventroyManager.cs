using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
/*
해야할일 
- 소비아이템 획득 F키로 변경, 인벤토리 앞에서부터 빈 곳부터 채우기, 만약 꽉찻다면 못 먹음 -- 완료
-악세,무기 어떻게 종류 저장해놓을지 -- 아마도 scriptable object로 만들것같음
-두루마리 만들어야함 ㅡㅡ 완료
=보스잡으면 드랍되는아이템 + 메스테이지 넘어갈 때 선택지 ㅡㅡ 이건 아마도 합치고 코드를 받아봐야 할 수 있을듯
템은 무조건 보스 잡았을때만.

*/
public class InventroyManager : MonoBehaviour
{
    Player Player;
    public GameObject hand;
    public GameObject ground;
    public GameObject[] inventory = new GameObject[3];
    public GameObject[] accessorys = new GameObject[2];
    public GameObject weapon;
    public int rollPaperCount=0;

    public bool canAddItem = true;
    public float AdditemCoolTime = 1.0f;

    public bool canUseItem=true;
    public float UseItemCoolTime=1.0f;

    public Image[] Items;

    public TextMeshProUGUI RollPaperCountText;
    
    void Awake()
    {   
        Player=GetComponentInParent<Player>(); 
        //RollPaperCountText.text = rollPaperCount.ToString();
    }
    public bool AddSobi(GameObject item)
    {
        Debug.Log("소비");
        if (canAddItem&&Input.GetKey(KeyCode.F))
        {
            for(int index=0;index<3;index++)
            {
                if (inventory[index]==null)
                {
                    inventory[index]=item;
                    item.transform.SetParent(this.transform);
                    Sobi itemScript = item.GetComponent<Sobi>();
                    itemScript.AddUI(index+1);
                    Items = GameObject.FindGameObjectsWithTag("CoolUI").Select(item => item.GetComponent<Image>()).ToArray();
                    return true;
                }
            }            
        }
        return false;
        /*
        if (canAddItem && Input.GetKey(KeyCode.F)&&Input.GetKey(KeyCode.Alpha1))
        {
            inventory[0]=item;
            item.transform.SetParent(this.transform);
            //이 밑에 문제인데
            Item itemScript = item.GetComponent<Item>();
            itemScript.AddUI(1);
            Items = GameObject.FindGameObjectsWithTag("CoolUI").Select(item => item.GetComponent<Image>()).ToArray();
            return true;
        }
        else if (canAddItem && Input.GetKey(KeyCode.F)&&Input.GetKey(KeyCode.Alpha2))
        {
            inventory[1]=item;
            item.transform.SetParent(this.transform);
            Item itemScript = item.GetComponent<Item>();
            itemScript.AddUI(2);
            Items = GameObject.FindGameObjectsWithTag("CoolUI").Select(item => item.GetComponent<Image>()).ToArray();
            return true;
            }

        else if (canAddItem && Input.GetKey(KeyCode.F)&&Input.GetKey(KeyCode.Alpha3))
        {
            inventory[2]=item;
            item.transform.SetParent(this.transform);
            Item itemScript = item.GetComponent<Item>();
            itemScript.AddUI(3);
            Items = GameObject.FindGameObjectsWithTag("CoolUI").Select(item => item.GetComponent<Image>()).ToArray();
            return true;
            }
            */
    }
    public bool AddRollPaper(GameObject RollPaper)
    {
        if(canAddItem && Input.GetKey(KeyCode.F))
        {
            rollPaperCount++;
            RollPaper.transform.SetParent(this.transform);
            UpdateRollPaperCount(rollPaperCount);
            return true;
        }
        return false;
    }
    public void UpdateRollPaperCount(int count)
    {
        RollPaperCountText.text=count.ToString();
    }
    public bool AddAccessory(GameObject NewAccessory)
    {   //획득시 ui이미지생성 해야함 ㅡ 소비템부터 완성시키
        if (canAddItem && Input.GetKey(KeyCode.F)&&Input.GetKey(KeyCode.Alpha1))
        {   
            if(accessorys[0]!=null)
            {
                Vector3 NewAccPos=NewAccessory.transform.position;
                GameObject OldAcc=accessorys[0];
                accessorys[0]=NewAccessory;
                NewAccessory.transform.SetParent(this.transform);
                GameObject DropAcc=Instantiate(OldAcc, NewAccPos, Quaternion.identity);
                DropAcc.SetActive(true);
            }
            else accessorys[0]=NewAccessory;
            return true;  
        }
        else if (canAddItem && Input.GetKey(KeyCode.F)&&Input.GetKey(KeyCode.Alpha2))
        {
            if(accessorys[1]!=null)
            {
                Vector3 NewAccPos=NewAccessory.transform.position;
                GameObject OldAcc=accessorys[1];
                accessorys[1]=NewAccessory;
                NewAccessory.transform.SetParent(this.transform);
                GameObject DropAcc=Instantiate(OldAcc, NewAccPos, Quaternion.identity);
                DropAcc.SetActive(true);
            }
            else accessorys[1]=NewAccessory;
            return true;
        }
        return false;
    }
    public bool ChangeWeapon(GameObject newWeapon)
    {   //획독시 ui이미지 생성하는거 만들어아햐나? 무기 슬롯은없긴함 프리팹은 만들어놔야 플레이어손에있는무기 바꿀거같은데
        if(canAddItem && Input.GetKey(KeyCode.F))
        {   
            /*if (weapon != null)
            {
                GameObject oldWeapon = weapon;
                Weapon OldWeapon=oldWeapon.GetComponent<Weapon>();
                OldWeapon.isEquipment=false;
                oldWeapon.gameObject.transform.position= newWeapon.transform.position;
                oldWeapon.transform.SetParent(ground.transform);
            }
            Weapon NewWeapon=newWeapon.GetComponent<Weapon>();
            weapon=newWeapon;
            weapon.transform.position=hand.transform.position;
            NewWeapon.isEquipment=true;
            newWeapon.transform.SetParent(hand.transform);
            return true;*/
        }
        return false;
    }
    
    public void UseSobiItem(int Index)
    {
        GameObject item= inventory[Index];
        switch(item.gameObject.tag)
        {
            case "Potion":
                
                Potion Potion = item.GetComponent<Potion>();
                    Potion.UsePotion();
                    inventory[Index]=null;
                    canUseItem=false;
                    StartCoroutine(StartUseItemCooldown());
                    break;
            case "Bojo":
                    Bojo bojo=item.GetComponent<Bojo>();
                    if(item.gameObject.name=="Rock")bojo.UseRock(Player.isSeegRight);
                    else if(item.gameObject.name=="Trap")bojo.UseTrap();
                    else break;
                    inventory[Index]=null;
                    canUseItem=false;
                    StartCoroutine(StartUseItemCooldown());
                break;
                
        }       
    }
    public IEnumerator StartAddItemCooldown()
    {
    yield return new WaitForSeconds(AdditemCoolTime);
    canAddItem = true;
    }
    public IEnumerator StartUseItemCooldown()
    {
    yield return new WaitForSeconds(UseItemCoolTime);
    canUseItem=true;
    }

}
