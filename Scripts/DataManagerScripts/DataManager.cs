using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager
{
    public static readonly DataManager Instance = new DataManager();

    private DataManager() { }

    public Dictionary<int, TestData> dicTestDatas;

    public void LoadAllDatas()
    {
        this.LoadMonsterData();
        this.LoadMonsterGroupData();
        this.LoadDifficultyData();
        this.LoadDialogData();
        this.LoadActiveSkillData();
        this.LoadRoomData();
        this.LoadEquipmentData();
        this.LoadRelicData();
        this.LoadChestData();
        this.LoadGambleData();
        this.LoadEtcItemData();
        this.LoadWeaponData();
    }
    public void LoadTestData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Data/Test_data");
        string json = asset.text;
        TestData[] arr = JsonConvert.DeserializeObject<TestData[]>(json);

        this.dicTestDatas = arr.ToDictionary((x) => x.id);
    }


    public TestData GetDicTestData(int id)
    {
        return this.dicTestDatas[id];
    }

    public List<TestData> GetTestDataList()
    {
        return this.dicTestDatas.Values.ToList();
    }

}
