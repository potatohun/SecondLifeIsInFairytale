using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject[] weaponEffect;
    public Player player;
    public Vector2 boxSize;
    public Transform boxPos;

    public GameObject[] attackEffect = null;


    public int combo = 1;
    public bool canAttack = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //tlqkf

            if (player.ani.GetCurrentAnimatorStateInfo(0).IsName("Idle") || player.ani.GetCurrentAnimatorStateInfo(0).IsName("Walk")) // ���ݾƴҶ� �ʱ�ȭ
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

        if (player.ani.GetInteger("Combo") == 3) // ���ݹ�������
        {
            boxSize = new Vector2(8f, 2.5f);
            boxPos.transform.localPosition = new Vector3(1.5f, 0f, 0f);
        }
        else
        {
            boxSize = new Vector2(2.5f, 2.5f);
            boxPos.transform.localPosition = new Vector3(0.6f, 0f, 0f);
        }

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
        SoundManager.soundManager.GetPlayerAudioClip("SPlayerAttack");
    }

    void OnAttackEffect()
    {
        GameObject aeffect = Instantiate(attackEffect[player.ani.GetInteger("Combo") - 1], transform.position, transform.rotation);
        aeffect.transform.parent = gameObject.transform;

        if (transform.localScale.x == -2.5f)
        {
            Vector3 a = aeffect.transform.localScale;
            a.x = -a.x;
            aeffect.transform.localScale = a;

            aeffect.transform.position = aeffect.transform.position + new Vector3(-1f, -0.1f, 0);
        }
        else
            aeffect.transform.position = aeffect.transform.position + new Vector3(1f, 0.1f, 0);


    }

    void onWeaponEffect()
    {
        GameObject weffect = null;

        switch (player.weaponType)
        {
            case WeaponData.WeaponType.Fire:
                weffect = Instantiate(weaponEffect[0], transform.position, transform.rotation);
                break;
            case WeaponData.WeaponType.Ice:
                weffect = Instantiate(weaponEffect[1], transform.position, transform.rotation);
                break;
            case WeaponData.WeaponType.Blood:
                weffect = Instantiate(weaponEffect[2], transform.position, transform.rotation);
                break;
            case WeaponData.WeaponType.None:
                return;
                
        }

        weffect.transform.parent = gameObject.transform;

        if (transform.localScale.x == -2.5f)
        {
            Vector3 a = weffect.transform.localScale;
            a.x = -a.x;
            weffect.transform.localScale = a;

            weffect.transform.position = weffect.transform.position + new Vector3(-1.5f, -0.1f, 0);
        }
        else
            weffect.transform.position = weffect.transform.position + new Vector3(1.5f, 0.1f, 0);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxPos.position, boxSize);
    }
}
