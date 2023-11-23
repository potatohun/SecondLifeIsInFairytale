using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void OffEffect()
    {
        Destroy(gameObject);
    }
}
