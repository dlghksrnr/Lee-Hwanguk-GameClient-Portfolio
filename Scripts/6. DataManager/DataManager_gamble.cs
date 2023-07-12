using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager
{
    public Dictionary<int, GambleData> dicGambleDatas;

    public void LoadGambleData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Data/gamble_data");
        string json = asset.text;
        GambleData[] arr = JsonConvert.DeserializeObject<GambleData[]>(json);

        this.dicGambleDatas = arr.ToDictionary((x) => x.stage);
    }
    public int GetGamblePrice(int stage)
    {
        var values = this.dicGambleDatas[stage];
        int dicePrice = values.price;
        return dicePrice;
    }
}
