using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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
}
