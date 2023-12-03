using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPreventer : MonoBehaviour
{
    private void OnEnable()
    {
        Player.instance.rigid.simulated = false;
    }

    private void OnDisable()
    {
        Player.instance.rigid.simulated = true;
    }
}
