using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public bool isDie;
    GameObject rewardManager;
    public GameObject cinemachine;

    private void Start()
    {
        isDie = false;

        GameObject canvas = GameObject.Find("Canvas");
        rewardManager = canvas.transform.Find("RewardManager").gameObject;


        if (rewardManager != null)
        {
            // 활성 상태를 설정합니다.
            rewardManager.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Reward Manager를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (isDie)
        {
            StartCoroutine(this.Die());
        }
    }
    public void IsDie()
    {
        isDie = true;
    }

    IEnumerator Die()
    {
        cinemachine.SetActive(true);
        yield return new WaitForSeconds(3f);
        //죽는 애니메이션 실행

        rewardManager.gameObject.SetActive(true);
        //this.gameObject.SetActive(false);
    }

}
