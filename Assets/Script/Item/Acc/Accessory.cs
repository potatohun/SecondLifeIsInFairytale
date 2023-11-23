using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : Item
{
   public AccessoryData accessoryData;
   
   public void Start()
   {
        if(this.gameObject.name== "Â¤½Å(Clone)") this.gameObject.name="Â¤½Å";
        else if(this.gameObject.name== "ÆÐ·©ÀÌ(Clone)") this.gameObject.name="ÆÐ·©ÀÌ";
        else if(this.gameObject.name== "±ºÈ­(Clone)") this.gameObject.name="±ºÈ­";
        else if(this.gameObject.name== "¾ç¹Ý°«(Clone)") this.gameObject.name="¾ç¹Ý°«";
   }
    public void EqipAcc(GameObject Accessory)
    {
        Accessory.transform.SetParent(inventoryManager.transform);
        player.MAXHP+=accessoryData.maxHp;
        player.moveSpeed+=accessoryData.moveSpeed;
        player.damage+=accessoryData.damage;
        player.attackSpeed+=accessoryData.attackSpeed;
        

    }
    public void RemoveAcc(GameObject Accessory)
    {
        player.MAXHP-=accessoryData.maxHp;
        player.HP=player.MAXHP;
        player.moveSpeed-=accessoryData.moveSpeed;
        player.damage-=accessoryData.damage;
        player.attackSpeed-=accessoryData.attackSpeed;
    }
}
