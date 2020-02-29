using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatController : MonoBehaviour
{
    //health and dmg stats
    public int baseDamage = 0;
    public int health = 0;
    public int maxHealth = 10;
    public int currentHealth;
    public int currentDmg;

    public int coinCount = 0;
    public string currentWeapon;

    public int potionHeal = 1;
    public int potionCount = 0;
    public int maxPotionCount = 5;

    //weapon modifiers for melee
    public int slashDmg;
    public int stabDmg;
    public int smashDmg;
    //weapon modifiers for range (not used since only one range weapon)
    public int fastShot;
    public int longShot;
    public int normalShot;

    private UIManager UI;

    void Awake()
    {
        currentHealth = health;
        currentDmg = baseDamage;
    }
    public void UpdateHP(int newHP)
    {
        currentHealth = newHP;
    }
    public void UpdateWeaponStats(StatController weaponSC)
    {
        if (weaponSC.gameObject.tag == "MeleeWeapon")
        {
            if (slashDmg != 0 | stabDmg != 0 | smashDmg != 0)
            {
                slashDmg = 0;
                stabDmg = 0;
                smashDmg = 0;
            }
            slashDmg = weaponSC.slashDmg;
            stabDmg = weaponSC.stabDmg;
            smashDmg = weaponSC.smashDmg;
        }
        /*
        if (weaponSC.gameObject.tag == "RangeWeapon")
        {
            if (fastShot != 0 | longShot != 0 | normalShot != 0)
            {
                normalShot = 0;
                fastShot = 0;
                longShot = 0;
            }
            normalShot = weaponSC.normalShot;
            fastShot = weaponSC.fastShot;
            longShot = weaponSC.longShot;
        }
        */
    }
    public void UpdateItemAndUI(bool hpChange, int howMuchHPChanged, bool updatePotion, int howManyItems, bool changeWeapon, string Name, bool updateCoin)
    {
        UI = GameObject.FindGameObjectWithTag("GM").GetComponent<UIManager>();
        if (hpChange == true)
        {
            if (howMuchHPChanged > 0)
            {
                UI.PlayerUI(howMuchHPChanged, true, false, false);
                UI.UpdateHp(currentHealth, howMuchHPChanged, true);
            }
            else if(howMuchHPChanged < 0)
            {
                UI.UpdateHp(currentHealth, howMuchHPChanged, false);
            }
        }
        if (updateCoin == true)
        {
            UI.PlayerUI(howManyItems, false, false, true);
            UI.UpdateCoin(howManyItems);
        }
        if (updatePotion == true)
        {
            if (howManyItems > 0)
            {
                UI.PlayerUI(howManyItems, false, true, false);
                UI.UpdateItem(potionCount, howManyItems, true);
            }
            else if(howManyItems < 0)
            {
                UI.UpdateItem(potionCount, howManyItems, false);
            }
        }
        if (changeWeapon == true)
        {
            UI.UpdateWeapon(Name);
        }
    }
}
