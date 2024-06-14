using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButton : MonoBehaviour, IPointerClickHandler
{
    public delegate void ButtonAction();
    public ButtonAction onLeftClick;

    public GameObject textObj;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && onLeftClick != null) onLeftClick.Invoke();
    }

    public void SetText(string text)
    {
        textObj.GetComponent<TextMeshProUGUI>().text = text;
    }
}
