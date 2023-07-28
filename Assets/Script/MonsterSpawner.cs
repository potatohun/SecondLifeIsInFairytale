using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterSpawner : MonoBehaviour
{
    //public static MonsterSpawner instance;

    public GameObject pozol;
    public GameObject arrow_pozol;
    public GameObject spawnPoint;
    public int howMany;

    public void Awake()
    {
        howMany = Random.Range(2, 10);
    }
    public void Start()
    {
        Debug.Log("몬스터스폰!");
        for(int i = 0; i < howMany; i++)
        {
            int n = Random.Range(1, 3);

            if (n == 1)
            {
                PozolSpawn(i);
            }
            else if (n == 2)
            {
                Arrow_PozolSpawn(i);
            }
        }
        Debug.Log(howMany);
    }

    public void PozolSpawn(int i)
    {
        GameObject monster = Instantiate(pozol, spawnPoint.transform.position + new Vector3(i, 0, 0), Quaternion.identity);
        //monster.transform.position = new Vector3(0, 0, 0);
        monster.SetActive(true);
    }
    public void Arrow_PozolSpawn(int i)
    {
        GameObject monster = Instantiate(arrow_pozol, spawnPoint.transform.position + new Vector3(i,0,0), Quaternion.identity);
        //monster.transform.position = new Vector3(3, 0, 0);
        monster.SetActive(true);
    }
}
