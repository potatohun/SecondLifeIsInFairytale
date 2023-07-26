using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP = 100;
    public int Attack = 10;
    Rigidbody2D rigid;
    AudioSource hitAudio;
    Animator ani;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();    
        hitAudio = GetComponent<AudioSource>();
        ani = GetComponent<Animator>(); 
    }

    private void Update()
    {
        if(HP <= 0)
        {
            Dead();
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log(" ENEMY HP :" + HP);
        hitAudio.Play();
        ani.SetTrigger("Hit");
        StartCoroutine(KnockBack());
    }

    void Dead()
    {
        ani.SetTrigger("Dead");
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return null;  
        Vector3 playerPos = GameManager.gameManager.player.transform.position;
        Vector3 Vec = transform.position - playerPos;
        rigid.AddForce(Vec.normalized * 2, ForceMode2D.Impulse);
        
    }

   
}
