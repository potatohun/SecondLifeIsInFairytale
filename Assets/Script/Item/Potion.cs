using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Sobi
{  

    void Start()
    {   
        switch(this.gameObject.name)
        {
            case "Apple(Clone)":
                this.gameObject.name="Apple";

                break;
            case "RiceCake(Clone)":
                this.gameObject.name="RiceCake";
                break;
            case "Yakgwa(Clone)":
                this.gameObject.name="Yakgwa";
                break;   
        }

        //sobiData.sobitpye=SobiData.SobiType.Potion;
    }
    public void UsePotion()
    {   
        if(sobiData.maxHp!=null)player.MAXHP+=sobiData.maxHp;
        player.HP+=sobiData.value;
        if(player.HP>player.MAXHP)player.HP=player.MAXHP;
        Destroy(this.gameObject);
        
    }
    
}
