using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardManager : MonoBehaviour
{
    public GameObject prefab;
    // Update is called once per frame
    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.transform.SetParent(this.gameObject.transform);
            newObject.gameObject.name = "Card " + i;
        }
    }
    void RewardUI()
    {
        this.gameObject.SetActive(true);
    }

    public void SkipBtnClick()
    {
        this.gameObject.SetActive(false);
        switch (SceneManager.GetActiveScene().name)
        {
            case "1장":
                SceneManager.LoadScene("1장엔딩");
                break;
            case "2장":
                SceneManager.LoadScene("2장엔딩");
                break;
            case "3장":
                SceneManager.LoadScene("3장엔딩");
                break;
        }
    }
}
