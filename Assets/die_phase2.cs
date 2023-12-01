using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class die_phase2 : StateMachineBehaviour
{
    Bossmouse mouse;
    float step; // 프레임당 이동 거리
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Bossmouse>();
        mouse.render.color = Color.white;
        mouse.pcoll.enabled = false;
        mouse.ccoll.enabled = false;
        mouse.bColl.enabled = false;
        mouse.transform.position += new Vector3(0f, -1.53f, 0f);
        mouse.rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        mouse.render.flipX = false;
        mouse.monsterSpeed = 1f;
        step = mouse.monsterSpeed * Time.deltaTime; // 프레임당 이동 거리
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse.transform.position = Vector3.MoveTowards(mouse.transform.position, new Vector3(-12.15f, mouse.transform.position.y, 0f), step);

        if (mouse.transform.position.x == new Vector3(-12.15f, mouse.transform.position.y, 0f).x)
        { 
            mouse.monsterDestroy();
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
