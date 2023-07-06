using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager
{
    public Dictionary<int, DialogData> dicDialogDatas;

    public void LoadDialogData()
    {
        TextAsset asset = Resources.Load<TextAsset>("Data/dialog_data");
        string json = asset.text;
        DialogData[] arr = JsonConvert.DeserializeObject<DialogData[]>(json);

        this.dicDialogDatas = arr.ToDictionary((x) => x.id);
    }

    public DialogData GetDicDialogData(int id)
    {
        return this.dicDialogDatas[id];
    }

    public List<DialogData> GetDialog(UIDialogPanel.eDialogType dialogType)
    {
        return this.dicDialogDatas.Values
       .Where(data => data.dialogType == dialogType.ToString())
       .ToList();
    }
}
