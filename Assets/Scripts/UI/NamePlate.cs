using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NamePlate : MonoBehaviour
{
    [SerializeField] private GameObject textObj;

    public bool isHighlight;
    public bool isMakeVisible;

    private TextMeshProUGUI txtMesh;

    public string entityName;

    private void Start()
    {
        txtMesh = textObj.GetComponent<TextMeshProUGUI>();
        SetText(entityName);
    }

    private void Update()
    {
        PlateVisible();
        Highlight();
    }


    public void SetText(string text)
    {
        txtMesh.text = text;   
    }

    public void Highlight()
    {
        if (isHighlight)
        {
            txtMesh.color = Color.yellow;
            isHighlight = false;
        }
        else
        {
            txtMesh.color = Color.white;
        }
    }

    public void PlateVisible()
    {
        if (isMakeVisible)
        {
            txtMesh.enabled = true;
            isMakeVisible = false;
        }
        else
        {
            txtMesh.enabled = false;
        }
    }

    public void OverrideVisible(bool visible)
    {
        textObj.SetActive(visible);
    }

}
