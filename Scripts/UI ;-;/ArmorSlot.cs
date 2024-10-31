using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmorSlot : ItemSlot
{
    protected override void Start()
    {
        inventory = FindObjectOfType<Armory>();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Armory temp = inventory as Armory;
    }
}
