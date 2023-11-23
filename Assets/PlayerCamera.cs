using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    CinemachineVirtualCamera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        camera.Follow = player.GetComponent<Transform>();
    }
}
