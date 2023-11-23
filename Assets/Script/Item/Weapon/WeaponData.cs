using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="WeaponData",menuName="Scriptable Object/WeaponData",order=3)]
public class WeaponData : ItemData
{
     public enum WeaponType
    {
        Fire,
        Ice,
        Blood,
        None
    }

   public int maxHp;
   public int moveSpeed;
   public int damage;
   public int attackSpeed;
   public WeaponType weaponType;
}
