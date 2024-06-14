using System;
using UnityEngine;


public interface IInteractable
{

    public void OnInteract(PlayerBehaviour entity);

    public void ToggleNamePlateVisible();
    public void HighlightNamePlate();
}
