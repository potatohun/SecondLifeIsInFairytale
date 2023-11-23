using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossCamera : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.Follow = player.GetComponent<Transform>();
    }
}
