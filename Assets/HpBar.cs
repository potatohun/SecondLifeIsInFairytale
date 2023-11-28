using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Slider healthSlider; // Unity Inspector에서 할당할 슬라이더

    public float maxHealth;
    private float currentHealth;

    void Start()
    {
        maxHealth = Player.instance.MAXHP;
        currentHealth = Player.instance.HP;
    }

    private void Update()
    {
        maxHealth = Player.instance.MAXHP;
        currentHealth = Player.instance.HP;

        UpdateHealthBar(); // 체력 바 업데이트
    }
    void UpdateHealthBar()
    {
        float healthPercentage = currentHealth / maxHealth; // 체력의 백분율 계산
        healthSlider.value = healthPercentage; // 슬라이더 값 설정
    }
}
