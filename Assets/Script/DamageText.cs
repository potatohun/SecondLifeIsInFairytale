using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float alphaSpeed = 1f;
    TextMeshPro text;
    Color alpha;
    public int damage;

    public AudioSource hitsound;
    // Start is called before the first frame update
    void Start()
    {
        hitsound.Play();
        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();
        alpha = text.color;
        Invoke("DestroyObject", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0f, moveSpeed*Time.deltaTime, 0f));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime*alphaSpeed);
        text.color = alpha;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
