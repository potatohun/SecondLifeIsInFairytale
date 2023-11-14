using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewStartBtn()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CurrentChapter", 1);
        SceneManager.LoadScene("Intro");
    }

    public void ContinueBtn()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void OptionBtn()
    {

    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}
