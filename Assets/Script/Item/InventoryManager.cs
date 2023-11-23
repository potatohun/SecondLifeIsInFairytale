using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class InventoryManager : MonoBehaviour
{
    public Player Player;

    public GameObject Weapon;
    public GameObject[] inventory = new GameObject[3];
    public GameObject[] accessorys = new GameObject[2];

    public bool canAddItem = true;
    public float AdditemCoolTime = 1.0f;
    public bool canUseItem=true;
    public float UseItemCoolTime=1.0f;

    public GameObject inventoryUI;
    public InventoryUI Bag ;

    void Awake()
    {   
        Player=GetComponentInParent<Player>(); 
        Bag=inventoryUI.GetComponent<InventoryUI>();
    }
    void Update()
    {
        TryUseSobiItem();
    }    
    private void OnTriggerStay2D(Collider2D other)
    {
        switch (other.tag)
        {
        case "Potion":
        case "Bojo":
        case "Amulet":
            bool TryAddInventory = AddSobi(other.gameObject);
            if (TryAddInventory)
            {  
                //other.gameObject.transform.position=new Vector3(999f,999f, 999f);
                other.gameObject.SetActive(false);
                canAddItem = false;   
                StartCoroutine(StartAddItemCooldown());
            }
            break;
        
        case "RollPaper":
            bool TryAddRollPaper=AddRollPaper(other.gameObject);
            if (TryAddRollPaper)
            {
                other.gameObject.SetActive(false);
                canAddItem = false;
                StartCoroutine(StartAddItemCooldown());
            }
            break;
        case "Accessory" :
            bool TryAddAccessory = EqipAccessory(other.gameObject);
            if (TryAddAccessory)
            {
                other.gameObject.SetActive(false);
                canAddItem = false;
                StartCoroutine(StartAddItemCooldown());
            }
            break;
            
        case "Weapon":
            bool TryChnageWeapon = EqipWeapon(other.gameObject);
            if (TryChnageWeapon)
            {
                other.gameObject.transform.position=new Vector3(999f,999f, 0);
                canAddItem = false;
                StartCoroutine(StartAddItemCooldown());
            }
            break;  
        }
    }

    public bool AddSobi(GameObject item)
    {         
        if(canAddItem&&Input.GetKey(KeyCode.F))
        {
            for(int index=0;index<3;index++)
            {
                if (inventory[index]==null)
                {
                    inventory[index]=item;
                    item.transform.SetParent(this.transform);
                    Sobi itemData=item.gameObject.GetComponent<Sobi>();
                    Bag.AcquireItem(itemData.sobiData,index);
                    if(item.CompareTag("Amulet"))Player.haveAmulet=index+1;                    
                    return true;
                }
            }            
        }
        return false;   
    }
    public bool AddRollPaper(GameObject RollPaper)
    {
        if(canAddItem && Input.GetKey(KeyCode.F))
        {
            PlayerPrefs.SetInt("RollPaper", PlayerPrefs.GetInt("RollPaper"));
            return true;
        }
        return false;
    }
    public bool EqipAccessory(GameObject NewAccessory)
    {  
        if (canAddItem && Input.GetKey(KeyCode.F)&&Input.GetKey(KeyCode.Alpha1))
        {   
            Accessory newAcc = NewAccessory.GetComponent<Accessory>();
            if(accessorys[0]!=null)
            {
                Vector3 NewAccPos=NewAccessory.transform.position;
                GameObject OldAcc=accessorys[0];
                accessorys[0]=NewAccessory;              
                Accessory oldAcc = NewAccessory.GetComponent<Accessory>();
                newAcc.EqipAcc(NewAccessory);
                oldAcc.RemoveAcc(OldAcc);
                Bag.AcquireItem(newAcc.accessoryData,3);
                OldAcc.transform.position=NewAccPos;
                OldAcc.SetActive(true);
            }
            else 
            {
                accessorys[0]=NewAccessory;
                newAcc.EqipAcc(NewAccessory);
                Bag.AcquireItem(newAcc.accessoryData,3);
            }
            return true;  
        }
        else  if (canAddItem && Input.GetKey(KeyCode.F)&&Input.GetKey(KeyCode.Alpha2))
        {   
            Accessory newAcc = NewAccessory.GetComponent<Accessory>();
            if(accessorys[1]!=null)
            {
                Vector3 NewAccPos=NewAccessory.transform.position;
                GameObject OldAcc=accessorys[1];
                accessorys[1]=NewAccessory;              
                Accessory oldAcc = NewAccessory.GetComponent<Accessory>();
                newAcc.EqipAcc(NewAccessory);
                oldAcc.RemoveAcc(OldAcc);
                Bag.AcquireItem(newAcc.accessoryData,4);
                OldAcc.transform.position=NewAccPos;
                OldAcc.SetActive(true);
            }
            else 
            {
                accessorys[1]=NewAccessory;
                newAcc.EqipAcc(NewAccessory);
                Bag.AcquireItem(newAcc.accessoryData,4);
            }
            return true;  
        }
        return false;
    }
    
    public bool EqipWeapon(GameObject newWeapon)
    {  
        if(canAddItem && Input.GetKey(KeyCode.F))
        {   

            Weapon NewWeapon = newWeapon.GetComponent<Weapon>();
            if (Weapon != null)
            {
                Vector3 NewWeaponPos=newWeapon.transform.position;
                GameObject oldWeapon = Weapon;
                Weapon OldWeapon=oldWeapon.GetComponent<Weapon>();
                NewWeapon.EqipWeapon(newWeapon);
                Weapon.transform.SetParent(transform.parent.transform);
                OldWeapon.RemoveWeapon(oldWeapon);
                Bag.AcquireItem(NewWeapon.weaponData,5);
                oldWeapon.transform.SetParent(null);
                oldWeapon.transform.position=NewWeaponPos;
                return true;
            }
            else
            {
                Weapon = newWeapon;
                NewWeapon.EqipWeapon(newWeapon);
                Bag.AcquireItem(NewWeapon.weaponData,5);
                Weapon.transform.SetParent(this.transform);
                return true;
            }
            
        }
        return false;
    }

    private void TryUseSobiItem()
    {
        if(!Input.GetKey(KeyCode.F)&&canUseItem&&Input.GetKey(KeyCode.Alpha1))
        {       
            UseSobiItem(0);
        }
        if(!Input.GetKey(KeyCode.F)&&canUseItem&&Input.GetKey(KeyCode.Alpha2))
        {       
           UseSobiItem(1);
        }
        if(!Input.GetKey(KeyCode.F)&&canUseItem&&Input.GetKey(KeyCode.Alpha3))
        {       
            UseSobiItem(2);
        }
    }
    private void UseSobiItem(int index)
    {
        GameObject item= inventory[index];
        switch(item.gameObject.tag)
        {
            case "Potion":
                    Potion potion = item.GetComponent<Potion>();
                    Bag.UseItem(index);
                    potion.UsePotion();
                    inventory[index]=null;
                    canUseItem=false;
                    StartCoroutine(StartUseItemCooldown());
                    break;
            case "Bojo":
                    Bojo bojo=item.GetComponent<Bojo>();
                    Bag.UseItem(index);
                    if(item.gameObject.name=="Rock")bojo.UseRock(Player.isSeegRight);
                    else if(item.gameObject.name=="Trap")bojo.UseTrap();
                    inventory[index]=null;
                    canUseItem=false;
                    StartCoroutine(StartUseItemCooldown());
                break;
                
        }       
    }
    public void UseAmulet(int index)
    {
        Amulet amulet=inventory[index].gameObject.GetComponent<Amulet>();
        Bag.UseItem(index);
        amulet.UseAmulet(index);
    }
    public IEnumerator StartAddItemCooldown()
    {
    yield return new WaitForSeconds(AdditemCoolTime);
    canAddItem = true;
    }
    public IEnumerator StartUseItemCooldown ()
    {
    yield return new WaitForSeconds(UseItemCoolTime);
    canUseItem=true;
    }

}