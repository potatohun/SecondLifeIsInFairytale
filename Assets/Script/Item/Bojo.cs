using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bojo : Sobi
{
    public GameObject TrapPrefab;
    public GameObject RockPrefab;
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
      
        //sobiData.sobitpye=SobiData.SobiType.Bojo;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(this.gameObject.name=="UsedTrap"&&other.CompareTag("Enemy"))
        {
            GameObject Enemy = other.gameObject;
            Enemy EnemyScript = Enemy.GetComponent<Enemy>();
            EnemyScript.TakeDamage(sobiData.damage);
            Destroy(this.gameObject);
        }
        if(this.gameObject.name=="UsedRock"&&other.CompareTag("Enemy"))
        {
            GameObject Enemy = other.gameObject;
            Enemy EnemyScript = Enemy.GetComponent<Enemy>();
            EnemyScript.TakeDamage(sobiData.damage);
            Destroy(this.gameObject);
        }
    }

    public void UseTrap()
    {
        this.transform.position=inventory.transform.position;
        this.gameObject.name="UsedTrap";
        this.gameObject.tag="Used";
        this.transform.SetParent(null);
        this.gameObject.SetActive(true);
        Destroy(ItemStatus);
    }
    public void UseRock(bool SeeRight)
    {   
       
        this.transform.position=inventory.transform.position;
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
