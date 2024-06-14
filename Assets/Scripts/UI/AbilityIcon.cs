using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour
{
    [SerializeField]
    private GameObject background, icon, clock, highlight, foreground;

    [HideInInspector]
    public Image backgroundImg, iconImg, clockImg, highlightImg, foregroundImg;

    public void Awake()
    {
        backgroundImg = background.GetComponent<Image>();
        iconImg = icon.GetComponent<Image>();
        clockImg = clock.GetComponent<Image>();
        highlightImg = highlight.GetComponent<Image>();
        foregroundImg = foreground.GetComponent<Image>();
    }

    public void SetValue(float currTime, float maxTime)
    {
        clockImg.fillAmount = 1 - (currTime / maxTime);
    }

    public void SetHighlight(bool isHighlight)
    {
        highlightImg.enabled = isHighlight;
    }

    public void SetAvailable(bool isAvailable)
    {
        foregroundImg.enabled = !isAvailable;     
    }
}
