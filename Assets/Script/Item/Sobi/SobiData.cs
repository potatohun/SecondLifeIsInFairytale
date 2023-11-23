using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName="SobiData",menuName="Scriptable Object/SobiData",order=1)]
public class SobiData : ItemData
{   /*public enum SobiType
    {
        Potion,
        Bojo,
        None
    }*/
    public int value;
    public int maxHp;
    public int damage;
    //public SobiType sobitpye;

}
