using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;
    public AudioSource playerAudioSource = null;
    public AudioClip[] playerAudioClip = null; 
   
    private void Start()
    {
        soundManager = this;
        playerAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
    }

    public void GetPlayerAudioClip(string behaviour)
    {
        for (int i = 0; i < playerAudioClip.Length; i++)
            if (behaviour.Equals(playerAudioClip[i].name))
            {
                playerAudioSource.clip = playerAudioClip[i];
                playerAudioSource.Play();
            }  
      
    }



}
