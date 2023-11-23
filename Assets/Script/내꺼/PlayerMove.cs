using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Vector2 input;
    public PlayerRoll playerRoll;
    public Player player;

    private void FixedUpdate()
    {
        if(!playerRoll.isRoll)
        Move();
    }
    void LateUpdate()
    {
        player.ani.SetFloat("speed", input.magnitude);
    }
    void Move()
    {

        float x = Input.GetAxis("Horizontal");
        input = new Vector2(x, 0) * player.moveSpeed * Time.deltaTime;

        player.rigid.position += input;  
        
        if (x < 0)
            transform.localScale = new Vector3(-2.5f, 2.5f, 0);
        else if (x > 0)
            transform.localScale = new Vector3(2.5f, 2.5f, 0);

    }
    void PlayMoveSound()
    {
        SoundManager.soundManager.GetPlayerAudioClip("SPlayerMove");
    }
}
