using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XpPopup : MonoBehaviour
{
    public GameObject textObj;
    private TextMeshProUGUI txtMesh;

    public float riseSpeed;
    public float selfDestructTime;

    private float time = 0f;

    [HideInInspector] public string text;

    private void Start()
    {
        txtMesh = textObj.GetComponent<TextMeshProUGUI>();
        txtMesh.text = text + " XP";
    }

    public void SetText(string text)
    {
        this.text = text;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= selfDestructTime)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
    }
}
