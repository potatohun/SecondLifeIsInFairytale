using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Text text;
    public GameObject btn1;
    public GameObject btn2;
    public string str = "";

    public void Start()
    {
        StartCoroutine(TextPrint());
    }
    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator TextPrint()
    {
        int count = 0;
        while (count != str.Length)
        {
            if (count < str.Length)
            {
                text.text += str[count].ToString();
                count++;
            }
            yield return new WaitForSeconds(0.05f);
        }
        btn1.SetActive(true);
        btn2.SetActive(true);
    }
    public void MainBtn()
    {
        SceneManager.LoadScene("메뉴");
    }

    public void StartBtn()
    {
        SceneManager.LoadScene("1장시작");
    }
}
