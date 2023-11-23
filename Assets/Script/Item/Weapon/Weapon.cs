using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{   
    public WeaponData weaponData;
    public bool isEquiped = false;
    protected void Start()
    {
        if(this.gameObject.name== "흔한검(Clone)") this.gameObject.name="흔한검";
        else if(this.gameObject.name== "대검(Clone)") this.gameObject.name="대검";
        else if(this.gameObject.name== "불의검(Clone)") this.gameObject.name="불의검";
        else if(this.gameObject.name== "얼음의검(Clone)") this.gameObject.name="얼음의검";
        else if(this.gameObject.name== "피의검(Clone)") this.gameObject.name="피의검";

        player = Player.instance;
    }
    protected override void Update()
    {
        if(!isEquiped)
        {
            base.Update();
        }
    }
    public void EqipWeapon(GameObject Weapon)
    {
        Weapon.transform.SetParent(player.transform);
        player.MAXHP+=weaponData.maxHp;
        player.moveSpeed+=weaponData.moveSpeed;
        player.damage+=weaponData.damage;
        player.attackSpeed+=weaponData.attackSpeed;
        isEquiped=true;
        player.weaponType = weaponData.weaponType;
        
    }
    public void RemoveWeapon(GameObject Weapon)
    {
        player.MAXHP-=weaponData.maxHp;
        player.HP=player.MAXHP;
        player.moveSpeed-=weaponData.moveSpeed;
        player.damage-=weaponData.damage;
        player.attackSpeed-=weaponData.attackSpeed;
        isEquiped=false;
    }
}
