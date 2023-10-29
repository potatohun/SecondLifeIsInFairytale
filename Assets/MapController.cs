using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    //public GameObject[] map;
    public List<GameObject> map;
    public GameObject boss_map;
    int currentmap;
    // Start is called before the first frame update
    void Start()
    {
        currentmap = Random.Range(0, map.Count);
        if(map.Count == 0)
        {
            return;
        }
        else
        {
            map[currentmap].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MapChange()
    {
        map[currentmap].SetActive(false);
        map.RemoveAt(currentmap);
        if(map.Count == 0 )
        {
            boss_map.SetActive(true);
        }
        else
        {
            currentmap = Random.Range(0, map.Count);
            map[currentmap].SetActive(true);
        }
    }
}
