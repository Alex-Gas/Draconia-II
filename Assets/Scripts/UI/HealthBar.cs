using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject foreground;

    private Image foregroundImg;

    public void Awake()
    {
        foregroundImg = foreground.GetComponent<Image>();
    }

    public void SetValue(float currHealth, float maxHealth)
    {
        foregroundImg.fillAmount = currHealth / maxHealth;
    }



}
