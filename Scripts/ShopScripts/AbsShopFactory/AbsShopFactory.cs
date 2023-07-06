using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsShopFactory 
{
    public abstract Color MakeFrameColor(string grade);
    public abstract Sprite MakeShopIconImage(string grade);
    public abstract string MakeShopName(string grade);
    public abstract int MakeShopPowerStat(string grade);
    public abstract int MakeShopCriticalHitAmount(string grade);
    public abstract int MakeShopCriticalHitChance(string grade);
    public abstract int MakeShopFireRateStat(string grade);
    public abstract int MakeShopRecoveryAmount(string grade);
    public abstract int MakeShopItemPrice(string grade);
}
