using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Sobi
{  

    void Start()
    {   
        switch(this.gameObject.name)
        {
            case "»ç°ú(Clone)":
                this.gameObject.name="»ç°ú";

                break;
            case "¶±(Clone)":
                this.gameObject.name="¶±";
                break;
            case "¾à°ú(Clone)":
                this.gameObject.name="¾à°ú";
                break;   
        }

        //sobiData.sobitpye=SobiData.SobiType.Potion;
    }
    public void UsePotion()
    {
        Debug.Log(player);
        if(sobiData.maxHp!=null)player.MAXHP+=sobiData.maxHp;
        player.HP+=sobiData.value;
        if(player.HP>player.MAXHP)player.HP=player.MAXHP;
        Destroy(this.gameObject);
        
    }
    
}
