using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Player player;
    bool canDoubleJump = false;


    void Update()
    {
        Jump();
    }

    void Jump()
    {

        if (Input.GetButtonDown("Jump") && !player.ani.GetBool("isJump"))
        {
            player.ani.SetBool("isJump", true);
            player.rigid.AddForce(Vector2.up * player.jumpPower, ForceMode2D.Impulse);

            canDoubleJump = true;
            
            SoundManager.soundManager.GetPlayerAudioClip("SPlayerJump");
        }
        else if (Input.GetButtonDown("Jump") && canDoubleJump == true)
        {
            player.ani.SetTrigger("DoubleJump");
            player.rigid.AddForce(Vector2.up * player.jumpPower, ForceMode2D.Impulse);
            canDoubleJump = false;
           ;
            SoundManager.soundManager.GetPlayerAudioClip("SPlayerJump");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals("Npc"))
            player.ani.SetBool("isJump", false);
    }

   
 }
