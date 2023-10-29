using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject[] map;
    int currentmap;
    // Start is called before the first frame update
    void Start()
    {
        currentmap = Random.Range(0, map.Length);
        map[currentmap].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MapChange()
    {
        map[currentmap].SetActive(false);
        currentmap = Random.Range(0, map.Length);
        map[currentmap].SetActive(true);
    }
}
