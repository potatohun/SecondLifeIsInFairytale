using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updown_rollback : StateMachineBehaviour
{
    Bossmouse mouse;
    float step; // 프레임당 이동 거리
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Bossmouse>();
        mouse.monsterSpeed = 8f;
        step = mouse.monsterSpeed * Time.deltaTime; // 프레임당 이동 거리
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse.transform.position = Vector3.MoveTowards(mouse.transform.position, mouse.initialPosition, step);
        // 초기 위치에 도달하면 원하는 작업 실행
        if (mouse.transform.position == mouse.initialPosition)
        {
            animator.SetTrigger("bottom_all");
        }
    }
}
