using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Npc : MonoBehaviour
{

    public PlayableDirector cinemachine;

    private void Start()
    {
        cinemachine = GameObject.FindWithTag("CineMachine").GetComponent<PlayableDirector>();
    }

    public void PlayCineMachine()
    {
        cinemachine.Play();
    }
}
