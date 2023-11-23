using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet : Sobi
{
    public void UseAmulet(int index)
    {
        player.HP=player.MAXHP/4;
        player.haveAmulet=0;
        inventoryManager.inventory[index]=null;
    }
}
