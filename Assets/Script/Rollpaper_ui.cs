using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rollpaper_ui : MonoBehaviour
{
    public Text rollpaper_text;
    public int rollpaper;
    // Start is called before the first frame update
    void Start()
    {
        rollpaper = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rollpaper_text.text = rollpaper.ToString();
    }
}
