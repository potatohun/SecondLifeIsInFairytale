using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlotUI : MonoBehaviour
{
    public ItemData _item;
    public Image _base;
    public Image _image;
    public Image _coolImage;
    protected float currentCooldown = 0.0f;
    public void SetColor(Image _image,float value)
    {
        Color color=_image.color;
        color.a=value;
        _image.color=color;
    }
    public void AddItem(ItemData item)
    {
        _item=item;
        _image.sprite=_item.itemIamge;
        SetColor(_image,255);
        if(item.coolTimeIamge!=null)
        {
        _coolImage.sprite=item.coolTimeIamge;
        _coolImage.color=new Color(100f / 255f, 100f / 255f, 100f / 255f, 0f);
        }
        else _coolImage=null;
    }
    public void RemoveItem()
    {
        if(_coolImage!=null)
        {  
            _coolImage.sprite=null;
            SetColor(_coolImage,0);
        }
        _image .sprite=null;
        _item= null;
        SetColor(_image,0);
    }
    
    public void CoolItem()
    {
        if (_coolImage != null)
        {
            SetColor(_coolImage,255);
            StartCoroutine(DecreaseCoolImageOverTime(1f));
        }
    }

    
    IEnumerator DecreaseCoolImageOverTime(float duration)
    {
        float currentTime = 0f;
        float startFillAmount = 1f;

        while (currentTime <= duration)
        {
            float fillAmount = Mathf.Lerp(startFillAmount, 0f, currentTime / duration);
            _coolImage.fillAmount = fillAmount;

            currentTime += Time.deltaTime;
            yield return null;
        }
        _coolImage.fillAmount = 0f;
    }
}
