using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public partial class DataManager
{
    public Dictionary<string, EquipmentData> dicEquipmentDatas;
    public Dictionary<string, EtcItemData> dicEtcItemDatas;     
    public Dictionary<int, GunData> dicWeaponDatas;     

    public void LoadEquipmentData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Data/equipment_data");
        string json = asset.text;
        EquipmentData[] arr = JsonConvert.DeserializeObject<EquipmentData[]>(json);

        this.dicEquipmentDatas = arr.ToDictionary((x) => x.spriteName);
    }

    public void LoadEtcItemData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Data/etc_Item_data");
        string json = asset.text;
        EtcItemData[] arr = JsonConvert.DeserializeObject<EtcItemData[]>(json);

        this.dicEtcItemDatas = arr.ToDictionary((x) => x.id);
    }

    public void LoadWeaponData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Data/gun_data");
        string json = asset.text;
        GunData[] arr = JsonConvert.DeserializeObject<GunData[]>(json);

        this.dicWeaponDatas = arr.ToDictionary((x) => x.id);
    }

    public void EquipmentDetailByID(string name , out string equipmentName , out string statdisc)
    {
        var data = this.dicEquipmentDatas[name];
        var type = data.type;   

        switch(type) 
        {
            case "Sword":
                statdisc = string.Format("공격력 : + {0}",data.powerStat);
                break;
            case "Axe":
                statdisc = string.Format("치명타 피해 : + {0}", data.criticalHitAmount);
                break;
            case "Wand":
                statdisc = string.Format("치명타 확률 : + {0}", data.criticalHitChance);
                break;
            case "Arrow":
                statdisc = string.Format("공격속도 : + {0}", data.fireRateStat);
                break;
            default :
                statdisc = "데이터 매칭 오류";
                break;
        }
        equipmentName = data.name;
    }

    public int GetStatByTypeAndGrade(string type, string grade)
    {
        List<EquipmentData> keysWithTargetValue = this.dicEquipmentDatas
            .Where(pair => pair.Value.type == type)
            .Select(pair => pair.Value)
            .ToList();
        var data = keysWithTargetValue.Find(x => x.grade == grade);

        var stat = 0;
        switch (type)
        {
            case "Sword":
                stat =  data.powerStat;
                break;
            case "Axe":
                stat = data.criticalHitAmount;
                break;
            case "Wand":
                stat = data.criticalHitChance;
                break;
            case "Arrow":
                stat = data.fireRateStat;
                break;
            default:
                stat = 0;
                break;
        }
        return stat;
    }
    public EquipmentData GetNameByDiceResultIcon(string resultSpriteName)
    {
        var value=this.dicEquipmentDatas[resultSpriteName];
        return value;
    }

    public void GetEtcNameAndDescbyID(string name, out string nameKor, out string desc )
    {
        var data = default(EtcItemData);
        this.dicEtcItemDatas.TryGetValue(name, out data);   
        nameKor = data.name;
        desc = data.desc;      
    }

    public void GetWeaponNameAndDescbyID(string name, out string nameKor, out string desc)
    {
        nameKor = null;
        desc = null;

        if(name == "AssultRifle")
        {
            var data = this.dicWeaponDatas[9000];  
            nameKor = data.name;
            desc = data.desc;   
        }
        else if (name == "ShotGun")
        {
            var data = this.dicWeaponDatas[9002];
            nameKor = data.name;
            desc = data.desc;

        }
        else if (name == "SniperRifle")
        {
            var data = this.dicWeaponDatas[9001];
            nameKor = data.name;
            desc = data.desc;

        }
        else if(name == "SubmachineGun")
        {
            var data = this.dicWeaponDatas[9003];
            nameKor = data.name;
            desc = data.desc;
        }
    }
}
