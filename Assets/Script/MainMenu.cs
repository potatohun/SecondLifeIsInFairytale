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
        PlayerPrefs.SetInt("RollPaper", 1);
        SceneManager.LoadScene("1장시작");
    }

    public void ContinueBtn()
    {
        SceneManager.LoadScene("마을");
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
