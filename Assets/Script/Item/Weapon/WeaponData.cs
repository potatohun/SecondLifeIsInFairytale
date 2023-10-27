using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="WeaponData",menuName="Scriptable Object/WeaponData",order=1)]
public class WeaponData : ScriptableObject
{
   [SerializeField]
   private string weaponName;
   public string WeaponName{get {return weaponName;}}
   [SerializeField]
   private float strength;
   public float Strength{get{return strength;}}
   [SerializeField]  
   private float attackSpeed;
   public float AttackSpeed{get{return attackSpeed;}}
   
}
