using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phase1_run : StateMachineBehaviour
{
    Bossmouse mouse;
    public float time = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse = GameObject.FindGameObjectWithTag("Bossrat").GetComponent<Bossmouse>();
        time = 0f;
        mouse.nextMove = 4f;
        mouse.Check_distance();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse.rb.velocity = new Vector2(mouse.nextMove, mouse.rb.velocity.y);

        time += Time.deltaTime;
        if (time >= 1f)
        {
            mouse.rb.velocity = Vector2.zero;
            mouse.move_attack = false;
            mouse.bossAni.SetBool("run", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse.rb.velocity = Vector2.zero;
        mouse.move_attack = false;
    }

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
