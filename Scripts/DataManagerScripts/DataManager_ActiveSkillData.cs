using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager
{
    private Dictionary<int, ActiveSkillData> dicActiveSkillDatas;

    public void LoadActiveSkillData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Data/active_skill_data");
        string json = asset.text;
        ActiveSkillData[] arr = JsonConvert.DeserializeObject<ActiveSkillData[]>(json);
        this.dicActiveSkillDatas = arr.ToDictionary((x) => x.id);
    }

    public void GetActiveSkillNameAndDetails(string name, out string skillName,out string skillDetail)
    {
        foreach (var item in this.dicActiveSkillDatas)
        {
            Debug.Log(item.Value.prefabsName);    
        }
        Debug.LogFormat("딕셔너리 카운트 : {0}", this.dicActiveSkillDatas.Values.Count);
        var activeSkillData = this.dicActiveSkillDatas.Values.FirstOrDefault(x => x.prefabsName == name);
        Debug.LogFormat("링큐로 찾은 데이터 : {0}", activeSkillData);
        if (activeSkillData != null)
        {
            skillName = activeSkillData.name;
            skillDetail = activeSkillData.disc;
        }
        else
        {
            skillName =  null;
            skillDetail = null; 
        }
    }

}
