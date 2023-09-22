using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{
    public Transform playerTransform; // 카메라가 따라다닐 플레이어의 Transform 컴포넌트
    Vector3 initialPos;
    public float shakeTime, shakeAmount;
    private void Start()
    {
        initialPos = transform.position;
        SceneManager.sceneLoaded += OnSceneLoaded; // sceneLoaded 이벤트에 OnSceneLoaded 메소드를 연결
        FindPlayerObject(); // 씬이 로드될 때마다 Player 오브젝트를 찾아서 설정
    }

    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 playerVector = playerTransform.position;
            playerVector.z = transform.position.z; // 카메라의 Z축 위치는 유지하여 2D 화면을 유지

            transform.position = playerVector;
        }

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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerObject(); // 씬이 로드될 때마다 Player 오브젝트를 찾아서 설정
    }

    private void FindPlayerObject()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // 태그를 이용해 Player 오브젝트를 찾음

        if (player != null)
        {
            playerTransform = player.transform; // Player 오브젝트의 Transform을 변수에 저장
        }
        else
        {
            Debug.LogWarning("Player 오브젝트를 찾을 수 없습니다. Player 오브젝트에 'Player' 태그를 추가하세요.");
        }
    }
}
