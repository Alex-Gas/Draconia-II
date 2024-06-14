using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatioStatBar : MonoBehaviour
{
    public GameObject humanXPbar, dragonXPbar;

    private Image humanXPbarImg;
    private Image dragonXPbarImg;

    public void Awake()
    {
        humanXPbarImg = humanXPbar.GetComponent<Image>();
        dragonXPbarImg = dragonXPbar.GetComponent<Image>();
    }



    public void SetValue(float humanXP, float dragonXP)
    {
        float totalXP = humanXP + dragonXP;

        humanXPbarImg.fillAmount = humanXP / totalXP;
        dragonXPbarImg.fillAmount = dragonXP / totalXP;
    }

}
