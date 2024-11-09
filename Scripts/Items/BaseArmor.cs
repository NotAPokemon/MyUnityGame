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
    public float manaRegenBonus;
    public float manaBonus;
    public float speedBonus;
    public float durability;
    public bool equiped;
    float cd;

    protected override void rightClick()
    {
        if (cd >= 1)
        {
            player.swarpArmor((int)type, player.Num);
            equiped = true;
            cd = 0;
        } else
        {
            cd += Time.deltaTime;
        }
        
    }
}

public enum Armor {Helmet, Chestplate, Leggings, Boots }
