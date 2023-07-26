using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner instance;

    public GameObject pozol;
    public GameObject arrow_pozol;
    public GameObject spawnPoint;
    void Awake()
    { instance = this; }


    public void PozolSpawn()
    {
        GameObject monster = Instantiate(pozol, spawnPoint.transform.position , Quaternion.identity, GameObject.Find("Canvas").transform);
        //monster.transform.position = new Vector3(0, 0, 0);
        monster.SetActive(true);
    }
    public void Arrow_PozolSpawn()
    {
        GameObject monster = Instantiate(arrow_pozol, spawnPoint.transform.position + new Vector3(2,0,0), Quaternion.identity, GameObject.Find("Canvas").transform);
        //monster.transform.position = new Vector3(3, 0, 0);
        monster.SetActive(true);
    }
}
