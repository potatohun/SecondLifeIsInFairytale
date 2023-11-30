using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionWindow;
    public void NewStartBtn()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CurrentChapter", 1);
        PlayerPrefs.SetInt("RollPaper", 0);
        SceneManager.LoadScene("시작");
    }

    public void ContinueBtn()
    {
        switch (PlayerPrefs.GetInt("CurrentChapter"))
        {
            case 1:
                SceneManager.LoadScene("1장시작");
                break;
            case 2:
                SceneManager.LoadScene("2장시작");
                break;
            case 3:
                SceneManager.LoadScene("3장시작");
                break;
            default:
                SceneManager.LoadScene("1장시작");
                break;
        }
        
    }

    public void OptionBtn()
    {
        OptionWindowOpen();
    }

    public void ExitBtn()
    {
        Application.Quit();
    }

    public void OptionWindowOpen()
    {
        optionWindow.SetActive(true);
    }

    public void OptionWindowClose()
    {
        optionWindow.SetActive(false);
    }
}
