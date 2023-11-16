using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName="ItemData",menuName="Scriptable Object/ItemData",order=0)]
public class ItemData : ScriptableObject
{   
    public string name;
    public Sprite itemIamge;
    public Sprite coolTimeIamge;
    public GameObject itemPrefab;
    public GameObject itemStatusPrefab;
    public string itemTooltip;
    
}
