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
    public ItemManager itemManager; 
    
    public int rollPaperCount=0; 
    public TextMeshProUGUI RollPaperCountText;

    public bool canAddItem = true;
    public float AdditemCoolTime = 1.0f;
    public bool canUseItem=true;
    public float UseItemCoolTime=1.0f;
    public bool hasAmulet=false;

    public GameObject inventoryUI;
    public InventoryUI Bag ;

    void Awake()
    {   
        Player=GetComponentInParent<Player>(); 
        inventoryUI=GameObject.Find("Bag");
        Bag=inventoryUI.GetComponent<InventoryUI>();
        itemManager=GameObject.Find("ItemManager").GetComponent<ItemManager>();
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
            bool TryAddInventory = AddSobi(other.gameObject);
            if (TryAddInventory)
            {  
                other.gameObject.SetActive(false);
                canAddItem = false;   
                StartCoroutine(StartAddItemCooldown());
            }
            break;
        case "Amulet":
            if(!hasAmulet){
            bool TryAddInventory2 = AddSobi(other.gameObject);
            if (TryAddInventory2)
            {  
                other.gameObject.SetActive(false);
                canAddItem = false;   
                StartCoroutine(StartAddItemCooldown());
            }
            }
            break;
        case "RollPaper":
            bool TryAddRollPaper=AddRollPaper(other.gameObject);
            if (TryAddRollPaper)
            {
                Canvas.instance.rollpaper.rollpaper += 1;
                Destroy(other.gameObject);
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
                other.gameObject.SetActive(false);
                canAddItem = false;
                StartCoroutine(StartAddItemCooldown());
            }
            break;  
        }
    }

    public void AddReward(GameObject item)
    {
        for (int index = 0; index < 3; index++)
        {
            if (inventory[index] == null )
            {
                inventory[index] = item;
                item.transform.SetParent(this.transform);
                item.SetActive(false);
                Sobi sobiData = item.gameObject.GetComponent<Sobi>();
                Bag.AcquireItem(sobiData.sobiData, index);
                break;
            }
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
                    if(item.CompareTag("Amulet"))
                    {
                        Player.haveAmulet=index+1;
                        hasAmulet=true;
                    }
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
                Transform newAccTranform = newAcc.gameObject.transform.parent;
                GameObject OldAcc=accessorys[0];
                Accessory oldAcc = NewAccessory.GetComponent<Accessory>();
                accessorys[0]=NewAccessory;              
                newAcc.EqipAcc(NewAccessory);
                oldAcc.RemoveAcc(OldAcc);
                Bag.AcquireItem(newAcc.accessoryData,3);
               
                GameObject Accessory = itemManager.InstantiateItem("acc", OldAcc.name);
                Accessory.gameObject.transform.SetParent(newAccTranform);
                Destroy(OldAcc);
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
                Transform newAccTranform = newAcc.gameObject.transform.parent;
                GameObject OldAcc=accessorys[1];
                Accessory oldAcc = NewAccessory.GetComponent<Accessory>();
                accessorys[1]=NewAccessory;              
                newAcc.EqipAcc(NewAccessory);
                oldAcc.RemoveAcc(OldAcc);
                Bag.AcquireItem(newAcc.accessoryData,4);
               
                GameObject Accessory = itemManager.InstantiateItem("acc", OldAcc.name);
                Accessory.gameObject.transform.SetParent(newAccTranform);
                Destroy(OldAcc);
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
                Transform newWeaponTranform = NewWeapon.gameObject.transform.parent;
                GameObject oldWeapon = Weapon;
                Weapon OldWeapon=oldWeapon.GetComponent<Weapon>();
                Weapon=newWeapon;
                NewWeapon.EqipWeapon(newWeapon);
                Weapon.transform.SetParent(transform.parent.transform);
                OldWeapon.RemoveWeapon(oldWeapon);
                Bag.AcquireItem(NewWeapon.weaponData,5);

                GameObject weapon = itemManager.InstantiateItem("sword", oldWeapon.name);
                Weapon.gameObject.transform.SetParent(newWeaponTranform);
                Destroy(oldWeapon);
                return true;
            }
            else
            {
                Weapon = newWeapon;
                NewWeapon.EqipWeapon(newWeapon);
                Bag.AcquireItem(NewWeapon.weaponData,5);
                Weapon.transform.SetParent(transform.parent.transform);
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
                    if(item.gameObject.name=="돌")bojo.UseRock(Player.instance.isSeeRight);
                    else if(item.gameObject.name=="덫")bojo.UseTrap();
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