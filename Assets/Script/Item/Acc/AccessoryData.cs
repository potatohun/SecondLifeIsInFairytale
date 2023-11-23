using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName="AccessoryData",menuName="Scriptable Object/AccessoryData",order=2)]
public class AccessoryData : ItemData
{  
    public int maxHp;
    public int moveSpeed;
    public int damage;
    public int attackSpeed;
}
