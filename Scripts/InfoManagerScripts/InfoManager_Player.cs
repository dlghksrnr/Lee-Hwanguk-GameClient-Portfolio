using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public partial class InfoManager
{

    public StatInfo statInfo = new StatInfo();

    public StatInfo GetStatInfo()
    {
        return this.statInfo;
    }

    /// <summary>
    /// 능력치 초기화 메서드 + 자동저장
    /// </summary>
    /// <param name="powerStat">최종 수정 공격력</param>
    /// <param name="fireRateStat">최종 수정 공격속도</param>
    /// <param name="criticalHitAmount">최종 수정 치명타 피해</param>
    /// <param name="criticalHitChance">최종 수정 치명타 확률</param>
    public void InitStats(int powerStat, int fireRateStat,
        int criticalHitChance, int criticalHitAmount)
    {
        this.statInfo.powerStat = powerStat;
        this.statInfo.fireRateStat = fireRateStat;
        this.statInfo.criticalHitAmount = criticalHitAmount;
        this.statInfo.criticalHitChance = criticalHitChance;
        GameObject.FindObjectOfType<GunShell>()?.GetComponent<GunShell>().SetGun();
        this.SaveStatInfo();
    }

    public void UpgradeStats(int powerStat, int fireRateStat,
        int criticalHitChance, int criticalHitAmount)
    {
        this.statInfo.powerStatOrigin += powerStat;
        this.statInfo.fireRateStatOrigin += fireRateStat;

        this.statInfo.criticalHitAmountOrigin += criticalHitAmount;
        this.statInfo.criticalHitChanceOrigin += criticalHitChance;

        this.statInfo.powerStat += powerStat;
        this.statInfo.fireRateStat += fireRateStat;
        this.statInfo.criticalHitAmount += criticalHitAmount;
        this.statInfo.criticalHitChance += criticalHitChance;

        GameObject.FindObjectOfType<GunShell>()?.GetComponent<GunShell>().SetGun();
        this.SaveStatInfo();
    }

    public void ResetStats()
    {
        int powerStatChange = this.statInfo.powerStatOrigin - 10;
        int fireRateStatChange = this.statInfo.fireRateStatOrigin - 10;
        int criticalHitAmountChange = this.statInfo.criticalHitAmountOrigin - 10;
        int criticalHitChanceChange = this.statInfo.criticalHitChanceOrigin - 10;

        this.statInfo.powerStat -= powerStatChange;
        this.statInfo.fireRateStat -= fireRateStatChange;
        this.statInfo.criticalHitAmount -= criticalHitAmountChange;
        this.statInfo.criticalHitChance -= criticalHitChanceChange;

        this.statInfo.powerStatOrigin = 10;
        this.statInfo.fireRateStatOrigin = 10;
        this.statInfo.criticalHitAmountOrigin = 10;
        this.statInfo.criticalHitChanceOrigin = 10;

        GameObject.FindObjectOfType<GunShell>()?.GetComponent<GunShell>().SetGun();
        this.SaveStatInfo();
    }

    /// <summary>
    /// 캐릭터의 공격력을 증가시킵니다. **영구 증가인 경우 Enum 타입 인자 필요**
    /// <para>첫 번째 정수: <paramref name="amount"/> 증가시키고 싶은 정도.</para>
    /// <para>두 번째 정수: <paramref name="type"/> 영구 증가 인지 여부 (기본값 No).</para>
    /// </summary>
    /// <param name="amount">공격력 증가 정도</param>
    /// <param name="type">영구적 증가 여부</param>
    public void IncreasePowerStat(int amount, ePermanent type = ePermanent.NO)
    {
        if (type == ePermanent.YES)
        {
            this.statInfo.powerStat += amount;
            this.SaveStatInfo();
        }
        else
        {
            this.statInfo.powerStat += amount;
        }

        GameObject.FindObjectOfType<GunShell>()?.GetComponent<GunShell>().SetGun();
    }

    /// <summary>
    /// 캐릭터의 공격속도를 증가시킵니다. **영구 증가인 경우 Enum 타입 인자 필요**
    /// <para>첫 번째 정수: <paramref name="amount"/> 증가시키고 싶은 정도.</para>
    /// <para>두 번째 정수: <paramref name="type"/> 영구 증가 인지 여부 (기본값 No).</para>
    /// </summary>
    /// <param name="amount">공격속도 증가 정도</param>
    /// <param name="type">영구적 증가 여부</param>
    public void IncreaseFireRateStat(int amount = 1, ePermanent type = ePermanent.NO)
    {
        if (type == ePermanent.YES)
        {
            this.statInfo.fireRateStat += amount;
            this.SaveStatInfo();
        }
        else
        {
            this.statInfo.fireRateStat += amount;
        }

        GameObject.FindObjectOfType<GunShell>()?.GetComponent<GunShell>().SetGun();
    }

    /// <summary>
    /// 캐릭터의 치명타 피해량을 증가시킵니다. **영구 증가인 경우 Enum 타입 인자 필요**
    /// <para>첫 번째 정수: <paramref name="amount"/> 증가시키고 싶은 정도.</para>
    /// <para>두 번째 정수: <paramref name="type"/> 영구 증가 인지 여부 (기본값 No).</para>
    /// </summary>
    /// <param name="amount">치명타 피해량 증가 정도</param>
    /// <param name="type">영구적 증가 여부</param>
    public void IncreaseCriticalHitAmountStat(int amount, ePermanent type = ePermanent.NO)
    {
        if (type == ePermanent.YES)
        {
            this.statInfo.criticalHitAmount += amount;
            this.SaveStatInfo();
        }
        else
        {
            this.statInfo.criticalHitAmount += amount;
        }

        GameObject.FindObjectOfType<GunShell>()?.GetComponent<GunShell>().SetGun();
    }

    /// <summary>
    /// 캐릭터의 치명타 확률을 증가시킵니다. **영구 증가인 경우 Enum 타입 인자 필요**
    /// <para>첫 번째 정수: <paramref name="amount"/> 증가시키고 싶은 정도.</para>
    /// <para>두 번째 정수: <paramref name="type"/> 영구 증가 인지 여부 (기본값 No).</para>
    /// </summary>
    /// <param name="amount">치명타 확률 증가 정도</param>
    /// <param name="type">영구적 증가 여부</param>
    public void IncreaseCriticalHitChanceStat(int amount = 1, ePermanent type = ePermanent.NO)
    {
        if (type == ePermanent.YES)
        {
            this.statInfo.criticalHitChance += amount;
            this.SaveStatInfo();
        }
        else
        {
            this.statInfo.criticalHitChance += amount;
        }

        GameObject.FindObjectOfType<GunShell>()?.GetComponent<GunShell>().SetGun();
    }

    /// <summary>
    /// 플레이어 StatInfo 불러오기
    /// </summary>
    public void LoadStatInfo()
    {
        try
        {
            string path = string.Format("{0}/stat_info.json", Application.persistentDataPath);
            string encryptedJson = File.ReadAllText(path);
            string decryptedJson = encryption.GetGeneric<string>(path, encryptedJson);
            this.statInfo = JsonConvert.DeserializeObject<StatInfo>(decryptedJson);
            Debug.Log("<color=red>statInfo loaded successfully.</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save statInfo: " + e.Message);
        }
    }

    /// <summary>
    /// 플레이어 StatInfo 저장
    /// </summary>
    public void SaveStatInfo()
    {
        try
        {
            string path = string.Format("{0}/stat_info.json", Application.persistentDataPath);
            string json = JsonConvert.SerializeObject(this.statInfo);
            encryption.SetGeneric(path, json);
            File.WriteAllText(path, json);
            Debug.Log("<color=red>statInfo saved successfully.</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save statInfo: " + e.Message);

        }
    }
}
