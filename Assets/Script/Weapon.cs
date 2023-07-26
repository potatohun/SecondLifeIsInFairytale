using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator ani;
    //플레이어상속하면 플레이어에있는거 다써야됨..?
    private void Awake()
    {
        ani = GetComponent<Animator>(); 
    }
    void SetAttackF()
    {
      ani.SetBool("Attack", false);
    }

 /*   공격은 아이들 점프 런떄만,
       하면안될때 setbool로 attack못하게하면 되지않을까? -> 애니메이션 재생됨안됨
        canAttack을 넣어?
    아니그냥 구르기할때 setActive만하면 될거같은데 맞을때 물약먹을때 공격해도 되지않을까?*/
}
