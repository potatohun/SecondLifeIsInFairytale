using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class RollPaper : MonoBehaviour
{
    public Text rollpaper_text;

    private void Start()
    {
    }

    private void Update()
    {
        rollpaper_text.text = PlayerPrefs.GetInt("RollPaper").ToString();
    }
}
