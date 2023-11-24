using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phase2_updown : StateMachineBehaviour
{
    Bossmouse mouse;
    float currentTime = 0f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse = GameObject.FindGameObjectWithTag("Bossrat").GetComponent<Bossmouse>();
        currentTime = 0f;
        mouse.pcoll.enabled = false;
        mouse.ccoll.enabled = true;
        mouse.ChangeMaterial(mouse.newPhysicsMaterial);
        mouse.currentPatternCoroutine = mouse.StartCoroutine(mouse.pattern_updown());
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime += Time.deltaTime;
        if (currentTime >= 5f)
        {
            currentTime = 0f;
            mouse.bossAni.SetTrigger("bottom_all");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mouse.ChangeMaterial(null);
        mouse.pcoll.enabled = true;
        mouse.ccoll.enabled = false;
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
