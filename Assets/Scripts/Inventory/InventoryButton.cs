using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void ButtonAction();
    public ButtonAction onLeftClick;
    public ButtonAction onRightClick;
    public ButtonAction onHoverEnter;
    public ButtonAction onHoverExit;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && onLeftClick != null) onLeftClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right && onRightClick != null) onRightClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onHoverEnter != null) onHoverEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onHoverExit != null) onHoverExit.Invoke();
    }
}
