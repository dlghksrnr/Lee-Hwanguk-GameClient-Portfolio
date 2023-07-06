using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public partial class DataManager
{
    public Dictionary<int, MonsterData> dicMonsterData;
    public Dictionary<int, MonsterGroupData> dicMonsterGroupData;
    public Dictionary<int, DifficultyData> dicDifficultyData;
    public Dictionary<string, RoomData> dicRoomData;    

    public void LoadRoomData()
    {
        var asset = Resources.Load<TextAsset>("Data/room_data");
        if (asset != null)
        {
            var json = asset.text;
            RoomData[] arr = JsonConvert.DeserializeObject<RoomData[]>(json);
            this.dicRoomData = arr.ToDictionary(x => x.name);
            Debug.Log("RoomDataLoadComplete");
        }
        else Debug.LogError("Data/room_data not found.");
    }

    public RoomData GetRoomDataFromName(string name)
    {
        return this.dicRoomData[name];
    }

    public void LoadMonsterData()
    {
        var asset = Resources.Load<TextAsset>("Data/monster_data");
        if (asset != null)
        {
            var json = asset.text;
            MonsterData[] arr = JsonConvert.DeserializeObject<MonsterData[]>(json);
            this.dicMonsterData = arr.ToDictionary(x => x.id);
            Debug.Log("MonsterDataLoadComplete");
        }
        else Debug.LogError("Data/monster_data not found.");
    }

    public List<MonsterData> GetMonsterDatas()
    {
        return this.dicMonsterData.Values.ToList();
    }

    public MonsterData GetMonsterData(int id)
    {
        return this.dicMonsterData[id];
    }

    public void LoadMonsterGroupData()
    {
        var asset = Resources.Load<TextAsset>("Data/monster_group_data");
        if (asset != null)
        {
            var json = asset.text;
            MonsterGroupData[] arr = JsonConvert.DeserializeObject<MonsterGroupData[]>(json);
            this.dicMonsterGroupData = arr.ToDictionary(x => x.id);
            Debug.Log("MonsterGroupDataLoadComplete");
        }
        else Debug.LogError("Data/monster_group_data not found.");
    }

    public List<int> GetMonsetIDList(int id)
    {
        List<int> listMonsterID = new List<int>();
        var data = dicMonsterGroupData[id];

        if (data.monsterId1 >= 15000) listMonsterID.Add(data.monsterId1);
        if (data.monsterId2 >= 15000) listMonsterID.Add(data.monsterId2);
        if (data.monsterId3 >= 15000) listMonsterID.Add(data.monsterId3);
        if (data.monsterId4 >= 15000) listMonsterID.Add(data.monsterId4);
        if (data.monsterId5 >= 15000) listMonsterID.Add(data.monsterId5);
        if (data.monsterId6 >= 15000) listMonsterID.Add(data.monsterId6);
        if (data.monsterId7 >= 15000) listMonsterID.Add(data.monsterId7);

        return listMonsterID;
    }

    public void LoadDifficultyData()
    {
        var asset = Resources.Load<TextAsset>("Data/difficulty_data");
        if (asset != null)
        {
            var json = asset.text;
            DifficultyData[] arr = JsonConvert.DeserializeObject<DifficultyData[]>(json);
            this.dicDifficultyData = arr.ToDictionary(x => x.id);
            Debug.Log("DifficultyDataLoadComplete");
        }
        else Debug.LogError("Data/difficulty_data not found.");
    }

    public DifficultyData GetDifficultyData(int id)
    {
        return this.dicDifficultyData[id];
    }
}
