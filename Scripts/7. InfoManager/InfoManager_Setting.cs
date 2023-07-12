using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class InfoManager
{
    private PlayerPrefsEncryption encryption = PlayerPrefsEncryption.Instance;
    public SettingInfo settingInfo = new SettingInfo();

    public void LoadSettingInfo()
    {
        try
        {
            string path = string.Format("{0}/setting_info.json", Application.persistentDataPath);
            string encryptedJson = File.ReadAllText(path);
            string decryptedJson = encryption.GetGeneric<string>(path, encryptedJson);
            this.settingInfo = JsonConvert.DeserializeObject<SettingInfo>(decryptedJson);
            Debug.Log("<color=red>SettingInfo loaded successfully.</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save SettingInfo: " + e.Message);
        }

    }

    public void SaveSettingInfo()
    {
        try
        {
            string path = string.Format("{0}/setting_info.json", Application.persistentDataPath);
            string json = JsonConvert.SerializeObject(this.settingInfo);
            
            encryption.SetGeneric(path, json);
            File.WriteAllText(path, json);
            Debug.Log("<color=red>SettingInfo saved successfully.</color>");

        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save SettingInfo: " + e.Message);
        }

    }
}
