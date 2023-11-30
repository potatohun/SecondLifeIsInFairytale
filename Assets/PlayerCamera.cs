using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    CinemachineVirtualCamera camera;
    public CinemachineImpulseSource impulse;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindWithTag("Player");
        camera.Follow = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManager.player.ani.GetBool("Hit"))
        {
            impulse.GenerateImpulse();
        }
    }
}
