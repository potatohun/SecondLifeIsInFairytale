using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    private Animator ani;
    public Boss boss;


    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        boss.IsDie();
        ani.SetBool("end", true);
    }
}
