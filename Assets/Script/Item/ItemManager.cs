using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public GameObject[] SobiList= new GameObject[6];
    public GameObject[] AccList= new GameObject[4];
    public GameObject[] SwordList = new GameObject[5];

    public GameObject InstantiateItem(string type,string name)
    {
        GameObject tmp;

        switch (type)
        {
            case "sobi":
                for(int i=0;i<6;i++)
                {
                    if (name == SobiList[i].name) 
                    {
                        tmp = Instantiate(SobiList[i]);
                        tmp.name = SobiList[i].name;
                        tmp.SetActive(false);
                        return tmp;
                    }
                    
                }
                break;
            case "acc":
                for(int i=0;i<4;i++)
                {
                    if (name == AccList[i].name) 
                    {
                        tmp = Instantiate(AccList[i]);
                        tmp.name = AccList[i].name;
                        tmp.SetActive(false);
                        return tmp;
                    }
                }
                break;
            case "sword":
                for(int i=0;i<4;i++)
                {
                    if (name == SwordList[i].name) 
                    {
                        tmp = Instantiate(SwordList[i]);
                        tmp.name = SwordList[i].name;
                        tmp.SetActive(false);
                        return tmp;
                    }
                }
                break;
        }
        return null;
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 오브젝트를 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복 생성된 것이므로 이 오브젝트를 파괴
            Destroy(gameObject);
        }
    }
}
