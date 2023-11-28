using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rollpaper_ui : MonoBehaviour
{
    public Text rollpaper_text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rollpaper_text.text = PlayerPrefs.GetInt("RollPaper").ToString();
    }
}
