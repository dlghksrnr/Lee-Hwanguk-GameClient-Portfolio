using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public partial class DataManager
{
    public Dictionary<string, RelicData> dicRelicData;

    public void LoadRelicData()
    {
        var asset = Resources.Load<TextAsset>("Data/relic_data");
        if (asset != null)
        {
            var json = asset.text;
            RelicData[] arr = JsonConvert.DeserializeObject<RelicData[]>(json);
            this.dicRelicData = arr.ToDictionary(x => x.prefabName);
            Debug.Log("RelicDataLoadComplete");
        }
        else Debug.LogError("Data/relic_data not found.");
    }

    public RelicData GetRelicDataFromPrefabName(string name)
    {
        return this.dicRelicData[name];
    }

}
