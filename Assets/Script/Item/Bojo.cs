using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bojo : Sobi
{
    public GameObject TrapPrefab;
    public GameObject RockPrefab;
    public float Damage=15;
    Rigidbody2D rigid;

    protected override void Update()
    {
        if(this.gameObject.tag!="Used")
            base.Update();
        
    }
    void Start()
    {
        if(this.gameObject.name=="Trap(Clone)")
            this.gameObject.name="Trap";
        if(this.gameObject.name=="Rock(Clone)")
            this.gameObject.name="Rock";

        rigid = GetComponent<Rigidbody2D>();
        GameObject Player = GameObject.Find("Player");
        Player playerScript = Player.GetComponent<Player>();  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(this.gameObject.name=="UsedTrap"&&other.CompareTag("Enemy"))
        {
            GameObject Enemy = other.gameObject;
            Enemy EnemyScript = Enemy.GetComponent<Enemy>();
            EnemyScript.TakeDamage(20);
            Destroy(this.gameObject);
        }
        if(this.gameObject.name=="UsedRock"&&other.CompareTag("Enemy"))
        {
            GameObject Enemy = other.gameObject;
            Enemy EnemyScript = Enemy.GetComponent<Enemy>();
            EnemyScript.TakeDamage(10);
            Destroy(this.gameObject);
        }
    }

    public void UseTrap()
    {
        useItem();
        this.transform.position=playerTransform.position;
        this.gameObject.name="UsedTrap";
        this.gameObject.tag="Used";
        this.transform.SetParent(null);
        this.gameObject.SetActive(true);
        Destroy(ItemStatus);
    }
    public void UseRock(bool SeeRight)
    {   
        useItem();
        this.transform.position=playerTransform.position;
        this.gameObject.name="UsedRock";
        this.gameObject.tag="Used";
         this.transform.SetParent(null);
        this.gameObject.SetActive(true);
        Destroy(ItemStatus);
        if(SeeRight)
            rigid.AddForce(Vector3.right*17.0f,ForceMode2D.Impulse);       
        else
            rigid.AddForce(Vector3.left*17.0f,ForceMode2D.Impulse);      
        StartCoroutine(DestroyRockTime());
        
    }

    IEnumerator DestroyRockTime()
    {
    yield return new WaitForSeconds(3.0f);
    Destroy(this.gameObject);
    }

}
