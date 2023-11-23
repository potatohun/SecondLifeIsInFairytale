using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerAttack : MonoBehaviour
{
    public Player player;
    public Vector2 boxSize;
    public Transform boxPos;

    public GameObject attackEffect;

    public int combo = 1;
    public bool canAttack = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("Idle") || player.ani.GetCurrentAnimatorStateInfo(0).IsName("Walk")) // 공격아닐때 초기화
            {
                combo = 1;
                player.ani.SetInteger("Combo", 0);
                canAttack = true;
            }
            if (canAttack)
                Attack();//StartCoroutine(ComboAttack());
        }
    }

    void Attack()
    {

        canAttack = false;
        player.ani.SetInteger("Combo", combo++);
        player.ani.SetTrigger("Attack");

    }

    /* IEnumerator ComboAttack()
     {
         yield return null;
         Attack();
     }*/


    void Hit()
    {
        Collider2D[] enemy = Physics2D.OverlapBoxAll(boxPos.position, boxSize, 0);
        foreach (Collider2D collider in enemy) //병철이랑 진규랑 통합해야 하는 파트
        {
            Debug.Log(collider.tag);
            switch (collider.tag)
            {
                case "Pozol":
                    collider.GetComponent<Pozol>().TakeDamage(20);//데미지 어케함               
                    break;
                case "Arrow_Pozol":
                    collider.GetComponent<ArrowPozol>().TakeDamage(20);//데미지 어케함             
                    break;
                case "Tiger":
                    collider.GetComponent<Tiger>().TakeDamage(20);//데미지 어케함             
                    break;
                case "Nolbu":
                    collider.GetComponent<NewNolbu>().TakeDamage(1);//데미지 어케함             
                    break;
                case "manim":
                    collider.GetComponent<Pozol>().TakeDamage(20);//데미지 어케함             
                    break;
            }
        }
    }

    void OnCanAttack()
    {
        canAttack = true;
    }

    void PlayAttackSound()
    {
        //SoundManager.soundManager.GetPlayerAudioClip("SPlayerAttack");
    }

    void OnAttackEffect()
    {
        /*GameObject effect = Instantiate(attackEffect, transform.position, transform.rotation);
        effect.transform.parent = gameObject.transform;

        if (gameObject.transform.localScale.x == -2.5f)
        {
           
            effect.transform.position = effect.transform.position + new Vector3(-1f, -0.1f, 0);

        }
        else
        {
            effect.transform.position = effect.transform.position + new Vector3(1f, 0.1f, 0);
        }*/

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxPos.position, boxSize);
    }
}
