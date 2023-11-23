using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : Item
{
   public AccessoryData accessoryData;
   
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
