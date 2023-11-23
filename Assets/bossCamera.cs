using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossCamera : MonoBehaviour
{
    public CinemachineVirtualCamera BossCamera;
    public GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        //boss = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        BossCamera.Follow = boss.GetComponent<Transform>();
    }
}
