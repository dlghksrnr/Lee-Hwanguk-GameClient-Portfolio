using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSwordFactory : AbsShopFactory
{
    public override Color MakeFrameColor(string grade)
    {
        Color gradeColor = new Color();
        switch (grade)
        {
            case "Wood":
                gradeColor = new Color32(176, 82, 0, 255);
                break;
            case "Iron":
                gradeColor = new Color32(192, 178, 166, 255);
                break;
            case "Gold":
                gradeColor = new Color32(255, 255, 0, 255);
                break;
            case "Diamond":
                gradeColor = new Color32(0, 255, 188, 255);
                break;
        }
        return gradeColor;
    }

    public override Sprite MakeShopIconImage(string grade)
    {
        var atlas = AtlasManager.instance.GetAtlasByName("UIEquipmentIcon");
        Sprite SwordImage = null;
        switch (grade)
        {
            case "Wood":
                SwordImage = atlas.GetSprite("Wood_Sword");
                break;
            case "Iron":
                SwordImage = atlas.GetSprite("Iron_Sword");
                break;
            case "Gold":
                SwordImage = atlas.GetSprite("Gold_Sword");
                break;
            case "Diamond":
                SwordImage = atlas.GetSprite("Diamond_Sword");
                break;
        }
        return SwordImage;
    }
    public override string MakeShopName(string grade)
    {
        string name = null;
        switch (grade)
        {
            case "Wood":
                name = "나무검";
                break;
            case "Iron":
                name = "강철검";
                break;
            case "Gold":
                name = "황금검";
                break;
            case "Diamond":
                name = "다이아몬드검";
                break;
        }
        return name;
    }
    public override int MakeShopPowerStat(string grade)
    {
        int powerStat = 0;
        switch (grade)
        {
            case "Wood":
                powerStat = 10;
                break;
            case "Iron":
                powerStat = 20;
                break;
            case "Gold":
                powerStat = 30;
                break;
            case "Diamond":
                powerStat = 40;
                break;
        }
        return powerStat;
    }
    public override int MakeShopCriticalHitAmount(string grade)
    {
        int criticalHitAmount = 0;
        switch (grade)
        {
            case "Wood":
                criticalHitAmount = 0;
                break;
            case "Iron":
                criticalHitAmount = 0;
                break;
            case "Gold":
                criticalHitAmount = 0;
                break;
            case "Diamond":
                criticalHitAmount = 0;
                break;
        }
        return criticalHitAmount;
    }
    public override int MakeShopCriticalHitChance(string grade)
    {
        int criticalHitChance = 0;
        switch (grade)
        {
            case "Wood":
                criticalHitChance = 0;
                break;
            case "Iron":
                criticalHitChance = 0;
                break;
            case "Gold":
                criticalHitChance = 0;
                break;
            case "Diamond":
                criticalHitChance = 0;
                break;
        }
        return criticalHitChance;
    }
    public override int MakeShopFireRateStat(string grade)
    {
        int fireRateStat = 0;
        switch (grade)
        {
            case "Wood":
                fireRateStat = 0;
                break;
            case "Iron":
                fireRateStat = 0;
                break;
            case "Gold":
                fireRateStat = 0;
                break;
            case "Diamond":
                fireRateStat = 0;
                break;
        }
        return fireRateStat;
    }
    public override int MakeShopRecoveryAmount(string grade)
    {
        int recoveryAmount = 0;
        switch (grade)
        {
            case "Wood":
                recoveryAmount = 0;
                break;
            case "Iron":
                recoveryAmount = 0;
                break;
            case "Gold":
                recoveryAmount = 0;
                break;
            case "Diamond":
                recoveryAmount = 0;
                break;
        }
        return recoveryAmount;
    }

    public override int MakeShopItemPrice(string grade)
    {
        int itemPrice = 0;
        switch (grade)
        {
            case "Wood":
                itemPrice = 3000;
                break;
            case "Iron":
                itemPrice = 5000;
                break;
            case "Gold":
                itemPrice = 15000;
                break;
            case "Diamond":
                itemPrice = 30000;
                break;
        }
        return itemPrice;
    }
}
