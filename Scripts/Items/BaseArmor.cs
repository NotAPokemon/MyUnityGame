using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseArmor : BaseItem
{
    public Armor type;
    public float healthBonus;
    public float armorBonus;
    public float damageBonus;
    public float regenBonus;
    public float durability;
    public bool equiped;

    protected override void rightClick()
    {
        player.swarpArmor( (int) type , player.Num);
        equiped = true;
    }
}

public enum Armor {Helmet, Chestplate, Leggings, Boots }
