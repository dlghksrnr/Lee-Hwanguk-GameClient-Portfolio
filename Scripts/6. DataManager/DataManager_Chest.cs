using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public partial class DataManager
{
    public Dictionary<int, ChestData> dicChestDatas;
    public void LoadChestData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Data/chest_data");
        string json = asset.text;
        ChestData[] arr = JsonConvert.DeserializeObject<ChestData[]>(json);

        this.dicChestDatas = arr.ToDictionary((x) => x.difficulty);
    }
    public int GetDifficulty(int id)
    {
        var values = this.dicChestDatas[id];
        int difficulty = values.difficulty;
        return difficulty;
    }
   
    public int GetRewardGold(int diffculty) //=dungeonInfo.currentStepInfo
    {
        var values = this.dicChestDatas[diffculty];
        int rewardGold=values.reward_gold;
        return rewardGold;
    }
    public int GetRewardEther(int difficulty) //=dungeonInfo.currentStepInfo
    {
        var values = this.dicChestDatas[difficulty];
        int rewardEther = values.reward_ether;
        return rewardEther*2; 
    }
    public ChestData GetReturnChestDicValue(int difficulty)
    {
        return dicChestDatas[difficulty];
    }
    public int GetRanFieldCoin()
    {
        var coin = default(int);
        var currentStage = InfoManager.instance.dungeonInfo.CurrentStageInfo;

        if (currentStage == 1 || currentStage == 2)
        {
            coin = 5;
        }
        else coin = 10;

        return coin; 
    }
}
