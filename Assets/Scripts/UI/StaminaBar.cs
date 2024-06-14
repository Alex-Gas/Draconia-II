using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public GameObject foreground;

    private Image foregroundImg;

    public void Awake()
    {
        foregroundImg = foreground.GetComponent<Image>();
    }

    public void SetValue(float currStamina, float maxStamina)
    {
        foregroundImg.fillAmount = currStamina / maxStamina;
    }
}
