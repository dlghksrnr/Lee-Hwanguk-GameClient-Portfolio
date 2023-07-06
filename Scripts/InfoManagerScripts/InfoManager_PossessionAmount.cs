using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class InfoManager
{
    public PossessionAmountInfo possessionAmountInfo = new PossessionAmountInfo(); //보유 재화info
    /// <summary>
    /// 던전골드 초기화 ( 값 : 0 )
    /// </summary>
    public void InitDungeonGoldAmount()
    {
        this.possessionAmountInfo.dungeonGoldAmount = 0;
        this.possessionAmountInfo.LastDipositList = new List<int[]>() {
            new int[2], new int[2], new int[2], new int[2] };
    }

    /// <summary>
    /// 던전 골드 증가 
    /// </summary>
    public void IncreaseDungeonGold(int coin, bool isFieldCoin = false)
    {
        Debug.LogFormat("coin 증가 :{0}", coin);
        this.possessionAmountInfo.dungeonGoldAmount += coin;
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UICurrencyDirectorUpdateGoldUI);
        if(!isFieldCoin)
        this.SavepossessionDungeonGoldInfo();
    }

    /// <summary>
    /// 에테르 증가
    /// </summary>
    public void IncreaseEther(int ether)
    {
        Debug.LogFormat("ether 증가 :{0}", ether);
        this.possessionAmountInfo.etherAmount += ether;
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UICurrencyDirectorUpdateEtherUI);
        InfoManager.instance.possessionAmountInfo.totalDungeonEther += ether;
        this.SavepossessionGoodsInfo();
    }

    /// <summary>
    /// 골드,에테르 감소(아이템 구매) + 자동 저장
    /// </summary>
    public int DecreasePossessionGoods(int price)
    {
        this.possessionAmountInfo.totalConsumptionGold+=price;
        Debug.LogFormat("<color=red>item price:{0},totalConsumptionGold:{1}</color>", price, this.possessionAmountInfo.totalConsumptionGold);

        this.possessionAmountInfo.goldAmount -= price;
        this.SavepossessionGoodsInfo();
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UICurrencyDirectorUpdateGoldUI);
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UICurrencyDirectorUpdateEtherUI);
        return this.possessionAmountInfo.goldAmount;
    }

    /// <summary>
    /// 던전내 골드 감소
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool DecreaseDungeonGold(int price)
    {
        if (this.possessionAmountInfo.dungeonGoldAmount >= price)
        {
            this.possessionAmountInfo.totalConsumptionGold += price;
            Debug.LogFormat("<color=red>item price:{0},totalConsumptionGold:{1}</color>", price, this.possessionAmountInfo.totalConsumptionGold);

            this.possessionAmountInfo.dungeonGoldAmount -= price;
            EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UICurrencyDirectorUpdateGoldUI);
            return true;
        }
        else
            return false;
    }
    

    /// <summary>
    /// 에테르 소모
    /// </summary>
    public bool DecreaseEther(int price)
    {
        if (this.possessionAmountInfo.etherAmount >= price)
        {
            this.possessionAmountInfo.etherAmount -= price;
            EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UICurrencyDirectorUpdateEtherUI);
            var usedEther = InfoManager.instance.gameInfo.allUsedEther += price;

            this.SavepossessionGoodsInfo();
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 디파짓 메서드 디파짓 성공 금액 성소 지갑에 더함 + 자동 저장 (도적단원 일경우 인자값 true)
    /// </summary>
    /// <param name="isRouge">도적단원일경우 true</param>
    public bool Deposit(bool isRouge = false)
    {
        var price = this.possessionAmountInfo.dungeonGoldAmount;
        if (price <= 50) { Debug.Log("돈부족"); return false; }

        if (this.possessionAmountInfo.LastDipositList == null)
            this.possessionAmountInfo.LastDipositList = new List<int[]>() { new int[2], new int[2], new int[2], new int[2] }; ;

        var tempArr = this.possessionAmountInfo.LastDipositList[this.dungeonInfo.CurrentStageInfo - 1];

        if (isRouge)
        {
            var ran = UnityEngine.Random.Range(0, 2);
            var finalGold = price * ran;
            this.possessionAmountInfo.LastDipositList[this.dungeonInfo.CurrentStageInfo - 1]
                = new int[] { tempArr[0] + finalGold, tempArr[1] + price };
            this.possessionAmountInfo.goldAmount += finalGold;
        }
        else
        {
            var ran = UnityEngine.Random.Range(1, 11) * 0.1f;
            var depositGold = price * ran;
            var finalGold = (int)Math.Round(depositGold / 10.0f) * 10;
            this.possessionAmountInfo.LastDipositList[this.dungeonInfo.CurrentStageInfo - 1]
                = new int[] { tempArr[0] + finalGold, tempArr[1] + price };
            this.possessionAmountInfo.goldAmount += finalGold;
            Debug.Log(finalGold);
        }
        this.possessionAmountInfo.dungeonGoldAmount -= price;
        Debug.LogFormat("디파짓 골드 : {0} , 최종 골드 {1}",
            this.possessionAmountInfo.LastDipositList[this.dungeonInfo.CurrentStageInfo - 1][0],
            this.possessionAmountInfo.LastDipositList[this.dungeonInfo.CurrentStageInfo - 1][1]);
        this.SavepossessionGoodsInfo();
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UICurrencyDirectorUpdateGoldUI);

        return true;
    }

    /// <summary>
    /// 디파짓 결과를 튜플 리스트로 반환합니다 itme1 : 스테이지별 디파짓 성공한 금액 itme2 : 스테이지별 디파짓한 금액
    /// </summary>
    /// <returns></returns>
    public List<Tuple<int, int>> MakeDipositTupleList()
    {
        var tupleList = new List<Tuple<int, int>>();
        var templist = this.possessionAmountInfo.LastDipositList;

        if (templist != null)
        {
            for (int i = 0; i < templist.Count; i++)
            {
                tupleList.Add(new Tuple<int, int>(templist[i][0], templist[i][1]));
            }
        }

        return tupleList;
    }


    /// <summary>
    /// 플레이어 소유재화 불러오기
    /// </summary>
    public void LoadpossessionGoodsInfo()
    {

        try
        {
            string path = string.Format("{0}/possessionAmount_info.json", Application.persistentDataPath);
            string encryptedJson = File.ReadAllText(path);
            string decryptedJson = encryption.GetGeneric<string>(path, encryptedJson);
            this.possessionAmountInfo = JsonConvert.DeserializeObject<PossessionAmountInfo>(decryptedJson);
            Debug.Log("<color=red>possessionGoodsInfo loaded successfully.</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save inventoryInfo: " + e.Message);
        }
    }

    /// <summary>
    /// 플레이어 소유재화 저장
    /// </summary>
    public void SavepossessionGoodsInfo()
    {

        try
        {
            string path = string.Format("{0}/possessionAmount_info.json", Application.persistentDataPath);
            string json = JsonConvert.SerializeObject(this.possessionAmountInfo);
            encryption.SetGeneric(path, json);
            File.WriteAllText(path, json);
            Debug.Log("<color=red>possessionGoodsInfo saved successfully.</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save inventoryInfo: " + e.Message);
        }
    }

    /// <summary>
    /// 던전 골드 저장
    /// </summary>
    public void SavepossessionDungeonGoldInfo()
    {
        string path = string.Format("{0}/possessionAmount_info.json", Application.persistentDataPath);
        string json = JsonConvert.SerializeObject(this.possessionAmountInfo);
        encryption.SetGeneric(path, json);
        File.WriteAllText(path, json);
        Debug.Log("<color=red>SavepossessionDungeonGoldInfo saved successfully.</color>");
    }


}
