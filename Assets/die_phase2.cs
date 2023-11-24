using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class die_phase2 : StateMachineBehaviour
{
    Bossmouse mouse;
    float speed = 5.0f; // 이동 속도
    float step; // 프레임당 이동 거리
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse = GameObject.FindGameObjectWithTag("Bossrat").GetComponent<Bossmouse>();
        mouse.render.color = new Color(1f, 1f, 1f);
        mouse.pcoll.enabled = false;
        mouse.ccoll.enabled = false;
        mouse.transform.position += new Vector3(0f, -0.8f, 0f);
        mouse.render.flipX = true;
        step = speed * Time.deltaTime; // 프레임당 이동 거리
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse.transform.position = Vector3.MoveTowards(mouse.transform.position, new Vector3(-7.15f, mouse.transform.position.y, 0f), step);

        // 초기 위치에 도달하면 원하는 작업 실행
        if (mouse.transform.position.x == new Vector3(-7.15f, -3.09f, 0f).x)
        { 
            mouse.bossDelete();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
