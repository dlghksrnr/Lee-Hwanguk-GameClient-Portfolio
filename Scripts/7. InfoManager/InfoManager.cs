using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[Serializable]
public partial class InfoManager
{
    public enum ePermanent
    {
        NO, YES
    }

    public enum eNextStageType
    {
        NONE = 1,
        STAGE2,
        STAGE3,
        STAGE4,
    }

    public enum eTutorialType
    {
        NONE = -1,
        SHOP,
        STAT,
        RESULT,
        KNIGHTDIPOSIT,
        ROGUEDIPOSIT,
        DICE,
    }

    public static readonly InfoManager instance = new InfoManager();
    private InfoManager() { }

    public DungeonInfo dungeonInfo = new DungeonInfo();
    public InventoryInfo inventoryInfo = new InventoryInfo();
    public GameInfo gameInfo = new GameInfo();

    /// <summary>
    /// 인벤토리 정보 초기화 + 자동 저장( 던전씬 엔드시 반드시 호출 )
    /// </summary>
    public void InitInventoryInfo()
    {
        Debug.Log("<color=red>인벤토리 인포 이니셜라이징</color>");
        this.inventoryInfo.isEquipment = false;
        this.inventoryInfo.currentEquipmentsArr = null;
        this.SaveInventoryInfo();
    }

    /// <summary>
    /// 현재 유저 난이도 및 스테이지 정보 초기화( 던전씬 엔드시 반드시 호출 )
    /// </summary>
    public void InitDungeonInfo()
    {
        this.dungeonInfo.currentStepInfo = 1;
    }

    /// <summary>
    /// 인벤토리 갯수 영구증가(1개) +  자동 저장
    /// </summary>
    public int IncreaseInventoryCount()
    {
        this.inventoryInfo.InventoryCount += 1;
        this.SaveInventoryInfo();
        return this.inventoryInfo.InventoryCount;
    }

    /// <summary>
    /// 아이템 구매시 Inventory Info 에 구매아이템을 포함한 리스트 저장 + 자동 저장
    /// </summary>
    public void UpdateEquipmentInfo(List<string> EquipmentList)
    {
        Debug.Log("<color=red>인벤토리 인포 업데이트</color>");
        if(EquipmentList.Count != 0)
            this.inventoryInfo.isEquipment = true;
        else 
            this.inventoryInfo.isEquipment = false;
        this.inventoryInfo.currentEquipmentsArr = EquipmentList.ToArray();
        this.SaveInventoryInfo();
    }

    /// <summary>
    /// 플레이어 InventoryInfo 불러오기
    /// </summary>
    public void LoadInventoryInfo()
    {
        try
        {
            string path = string.Format("{0}/Inventory_Info.json", Application.persistentDataPath);
            string encryptedJson = File.ReadAllText(path);
            string decryptedJson = encryption.GetGeneric<string>(path, encryptedJson);
            this.inventoryInfo = JsonConvert.DeserializeObject<InventoryInfo>(decryptedJson);
            Debug.Log("<color=red>inventoryInfo loaded successfully.</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load inventoryInfo: " + e.Message);
        }
    }

    /// <summary>
    /// 플레이어 Inventory 저장
    /// </summary>
    public void SaveInventoryInfo()
    {
        try
        {
            string path = string.Format("{0}/Inventory_Info.json", Application.persistentDataPath);
            string json = JsonConvert.SerializeObject(this.inventoryInfo);
            encryption.SetGeneric(path, json);
            File.WriteAllText(path, json);
            Debug.Log("<color=red>inventoryInfo saved successfully.</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save inventoryInfo: " + e.Message);
        }
    }

    /// <summary>
    /// 유저 난이도 변경 (스테이지 정보 입력시 해당 스테이지 시작 난이도로 자동 설정), 매개변수 없을시 = 난이도 + 1
    /// <para>스테이지2 : 시작 난이도 = 3</para>
    /// <para>스테이지3 : 시작 난이도 = 6</para>
    /// <para>스테이지4 : 시작 난이도 = 10</para>
    /// </summary>
    /// <param name="stage">유저 난이도 점프시 스테이지 enum 입력</param>
    public void ChangeDungeonStepInfo(eNextStageType stage = default(eNextStageType))
    {
        if (stage == default(eNextStageType))
        {
            if (this.dungeonInfo.currentStepInfo == 2)
                this.dungeonInfo.currentStepInfo = 2;
            else if (this.dungeonInfo.currentStepInfo == 5)
                this.dungeonInfo.currentStepInfo = 5;
            else if (this.dungeonInfo.currentStepInfo == 9)
                this.dungeonInfo.currentStepInfo = 9;
            else
                this.dungeonInfo.currentStepInfo += 1;
        }
        else if (stage == eNextStageType.STAGE2)
        {
            this.dungeonInfo.currentStepInfo = 3;
        }
        else if (stage == eNextStageType.STAGE3)
        {
            this.dungeonInfo.currentStepInfo = 6;
        }
        else if (stage == eNextStageType.STAGE4)
        {
            this.dungeonInfo.currentStepInfo = 10;
        }
    }

    public void LoadGameInfo()
    {
        try
        {
            string path = string.Format("{0}/game_info.json", Application.persistentDataPath);
            string encryptedJson = File.ReadAllText(path);
            string decryptedJson = encryption.GetGeneric<string>(path, encryptedJson);
            this.gameInfo = JsonConvert.DeserializeObject<GameInfo>(decryptedJson);
            Debug.Log("<color=red>gameInfo loaded successfully.</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save inventoryInfo: " + e.Message);

        }
    }

    /// <summary>
    /// 플레이어 Inventory 저장
    /// </summary>
    public void SaveGameInfo()
    {
        try
        {
            string path = string.Format("{0}/game_info.json", Application.persistentDataPath);
            string json = JsonConvert.SerializeObject(this.gameInfo);
            encryption.SetGeneric(path, json);
            File.WriteAllText(path, json);
            Debug.Log("<color=red>gameInfo saved successfully.</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save gameInfo: " + e.Message);

        }
    }

    /// <summary>
    /// 튜토리얼 완료여부 저장 + 자동저장
    /// </summary>
    /// <param name="type"></param>
    public void TutorialDone(eTutorialType type)
    {
        switch (type)
        {
            case eTutorialType.SHOP:
                this.gameInfo.isShopTuto = true;
                break;
            case eTutorialType.STAT:
                this.gameInfo.isStatTuto = true;
                break;
            case eTutorialType.RESULT:
                this.gameInfo.isResultTuto = true;
                break;
            case eTutorialType.KNIGHTDIPOSIT:
                this.gameInfo.isKnightDipositTuto = true;
                break;
            case eTutorialType.ROGUEDIPOSIT:
                this.gameInfo.isRogueDipositTuto = true;
                break;
            case eTutorialType.DICE:
                this.gameInfo.isDiceTuto = true;
                break;
        }
        this.SaveGameInfo();
    }
}
