using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScanner : MonoBehaviour
{
    public Vector3 dirvec;
    public GameObject scanObj;
    public GameObject ScanObj()
    {
        return scanObj;
    }

    void Update()
    {
        //Debug.Log(PlayerPrefs.GetInt("currentChapter"));
        RaycastHit2D rayhit;

        if (transform.localScale.x > 0)
        {
            Debug.DrawRay(transform.position + new Vector3(1, 0, 0), Vector3.forward * 20f, Color.green);
            rayhit = Physics2D.Raycast(transform.position + new Vector3(1, 0, 0), Vector3.forward * 20f, LayerMask.GetMask("Object"));
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(-1, 0, 0), Vector3.back * 20f, Color.green);
            rayhit = Physics2D.Raycast(transform.position + new Vector3(-1, 0, 0), Vector3.back * 20f, LayerMask.GetMask("Object"));
        }

        if (rayhit.collider != null)
        {
            scanObj = rayhit.collider.gameObject;
            if (scanObj.layer == LayerMask.NameToLayer("Item"))
            {
                Item item = scanObj.GetComponent<Item>();
                //item.isWatched = true; //아이템 쳐다보는중
            }
        }
        else
            scanObj = null;
    }
}
