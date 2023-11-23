using System.Collections;
using UnityEngine;

public class Camera : MonoBehaviour
{
    Vector3 initialPos;
    public float shakeAmount;

    Transform player;
    void Start()
    {
        initialPos = transform.position;
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (GameManager.gameManager.player.ani.GetBool("Hit"))
            StartCoroutine("ShakeCamera");
    }

    IEnumerator ShakeCamera()
    {

        transform.position = initialPos + Random.insideUnitSphere * shakeAmount;
        yield return null;

        if (!GameManager.gameManager.player.ani.GetBool("Hit"))
            transform.position = initialPos;
    }

    
    private void LateUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
        transform.position = targetPos;
    }
}
