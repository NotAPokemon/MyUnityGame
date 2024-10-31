using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public int index;
    protected BaseUI inventory;

    protected virtual void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Inventory temp = inventory as Inventory;
        temp.changed = true;
        temp.LastClicked = temp.Selected;
        temp.Selected = index;
    }
}
