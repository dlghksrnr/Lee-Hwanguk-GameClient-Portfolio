using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager
{

    public Dictionary<int, Shop_Data> dicShopDatas;
    //ShopData
    public void LoadShopData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Data/shop_data");
        string json = asset.text;
        Shop_Data[] arr = JsonConvert.DeserializeObject<Shop_Data[]>(json);

        this.dicShopDatas = arr.ToDictionary((x) => x.id);
    }
    //ShopData
    public Shop_Data GetDicShopData(int id)
    {
        return this.dicShopDatas[id];
    }
    public List<Shop_Data> GetShopDataList()
    {
        return this.dicShopDatas.Values.ToList();
    }
}
