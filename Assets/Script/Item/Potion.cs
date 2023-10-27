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
    }

    public void UsePotion()
    {   
        switch(this.gameObject.name)
        {
            case "Apple":
                useItem();
                playerScript.HP += 5;
                if(playerScript.HP>playerScript.MAXHP)playerScript.HP=playerScript.MAXHP;
                //Destroy(this.gameObject);
                break;
            case "RiceCake":
                useItem();
                playerScript.HP += 15;
                if(playerScript.HP>playerScript.MAXHP)playerScript.HP=playerScript.MAXHP;
                //Destroy(this.gameObject);
                break;
            case "Yakgwa":
                useItem();
                playerScript.MAXHP+=10;
                playerScript.HP+=10;
                if(playerScript.HP>playerScript.MAXHP)playerScript.HP=playerScript.MAXHP;
                //Destroy(this.gameObject);
                break;
        }
    }
    
}
