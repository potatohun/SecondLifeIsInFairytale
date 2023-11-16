using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject go_SlotsParent; 
    public SlotUI[] slots;
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<SlotUI>();
    }
    public void AcquireItem(ItemData item,int number)
    {   
        Debug.Log(item);
        Debug.Log(number);
        if(number<3)
        {
        if (slots[number]._item == null)
            {
                slots[number].AddItem(item);
                return;
            }
        }
        else
        {
            slots[number].AddItem(item);
                return;
        }
    }
    public void UseItem(int number)
    {
        for(int index=0;index<3;index++)
        {
            if(number!=index&&slots[index]._item!=null)slots[index].CoolItem();
            else slots[index].RemoveItem();
        }
    
    }
}
