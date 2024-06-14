using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormCooldownBar : MonoBehaviour
{
    public GameObject cooldownBar;

    private Image cooldownBarImg;

    public void Awake()
    {
        cooldownBarImg = cooldownBar.GetComponent<Image>();
    }

    public void SetValue(float cooldownCurrTime, float cooldownTime)
    {
        cooldownBarImg.fillAmount = cooldownCurrTime / cooldownTime;
    }
}
