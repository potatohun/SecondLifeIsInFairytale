using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nolbu_arrow : StateMachineBehaviour
{
    NewNolbu nolbu;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nolbu = GameObject.FindGameObjectWithTag("Nolbu").GetComponent<NewNolbu>();
        nolbu.arrowCount++;
        nolbu.currentPatternCoroutine = nolbu.StartCoroutine(nolbu.RangeAll());
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (nolbu.arrowCount == 3)
        {
            nolbu.bossAni.SetBool("arrowRe", false);
            nolbu.currentPatternCoroutine = null;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (nolbu.arrowCount == 3)
        {
            nolbu.arrowCount = 0;
        }
    }
}
