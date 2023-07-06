using Firebase.Auth;
using GooglePlayGames;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class SceneArgs {
    public string nextSceneType;
    public bool isFromTitle;
}

public class App : MonoBehaviour
{
    public enum eSceneType
    {
        AppScene, TitleScene, LoadingScene, SanctuaryScene, DungeonScene
    }

    private TitleSceneMain titleSceneMain;

    private void Awake()
    {     
        //SRDebug.Init();  
        if (!this.NewbieCheck())
        {
            InfoManager.instance.LoadGameInfo();
            InfoManager.instance.LoadStatInfo();
            InfoManager.instance.LoadInventoryInfo();
            InfoManager.instance.LoadpossessionGoodsInfo();
            InfoManager.instance.LoadSettingInfo();

            //Info Init 
            //InfoManager.instance.SaveGameInfo();
            //InfoManager.instance.SaveStatInfo();
            //InfoManager.instance.SaveInventoryInfo();
            //InfoManager.instance.SavepossessionGoodsInfo();
            //InfoManager.instance.SaveSettingInfo();
            Debug.Log("Existing User");
        }
        else
        {
            InfoManager.instance.SaveGameInfo();
            InfoManager.instance.SaveStatInfo();
            InfoManager.instance.SaveInventoryInfo();
            InfoManager.instance.SavepossessionGoodsInfo();
            InfoManager.instance.SaveSettingInfo();
            Debug.Log("New User");
        }
        DataManager.Instance.LoadAllDatas();

        DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        Debug.Log("¾Û ±ú¾î³²");
        Application.targetFrameRate = 60;
        this.ChangeScene(eSceneType.TitleScene);
    }

    public void ChangeScene(eSceneType sceneType, SceneArgs args = null)
    {

        Debug.LogFormat("ChangeScene: {0}", sceneType);

        var oper = SceneManager.LoadSceneAsync(sceneType.ToString());
        AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.NONE, true);
        AudioManager.instance.SceneBGMusicSetting(sceneType);
        switch (sceneType)
        {
            case eSceneType.TitleScene:
                oper.completed += (obj) =>
                {                  
                    var arg = new SceneArgs() { nextSceneType = "SanctuaryScene", isFromTitle = true };
                    this.titleSceneMain = GameObject.FindObjectOfType<TitleSceneMain>();
                    this.titleSceneMain.uiTitleDirector.onClick = () =>
                    {
                        this.ChangeScene(eSceneType.LoadingScene, arg);
                    };
                    this.titleSceneMain.Init();
                };
                break;

            case eSceneType.LoadingScene:
                oper.completed += (obj) =>
                {
                    var loadingMain = GameObject.FindObjectOfType<LoadingSceneMain>();
                    loadingMain.onComplete = () =>
                    {
                        if (args.nextSceneType == "SanctuaryScene") this.ChangeScene(eSceneType.SanctuaryScene);
                        else if (args.nextSceneType == "DungeonScene") this.ChangeScene(eSceneType.DungeonScene);
                    };
                    loadingMain.Init(args);
                };
                break;

            case eSceneType.SanctuaryScene:
                oper.completed += (obj) =>
                {
                    var sanctuaryMain = GameObject.FindObjectOfType<SanctuarySceneMain>();
                    var arg = new SceneArgs() { nextSceneType = "DungeonScene" };
                    sanctuaryMain.onintotheDungeon = () =>
                    {
                        this.ChangeScene(eSceneType.LoadingScene, arg);
                    };
                    sanctuaryMain.Init();
                };
                break;

            case eSceneType.DungeonScene:
                oper.completed += (obj) =>
                {
                    var dungeonMain = GameObject.FindObjectOfType<DungeonSceneMain>();
                    var arg = new SceneArgs() { nextSceneType = "SanctuaryScene" };
                    dungeonMain.goBackToTheSanctuary = () =>
                    {
                        this.ChangeScene(eSceneType.LoadingScene, arg);
                    };
                    dungeonMain.Init();
                };
                break;
        }
    }

    private bool NewbieCheck()
    {
        bool FNG = false;
        string filePath = Path.Combine(Application.persistentDataPath, "game_info.json");
        if (!File.Exists(filePath))
        {
            FNG = true;
        }
        return FNG;
    }

    private UIPauseDirector uiPauseDirector;

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            this.uiPauseDirector = GameObject.FindObjectOfType<UIPauseDirector>();

            if (this.uiPauseDirector != null && !this.uiPauseDirector.uiPauseMenu.gameObject.activeSelf)
            {
                this.uiPauseDirector.ActivePauseUI();
                this.uiPauseDirector.onPushPause(this.uiPauseDirector.uiPauseMenu.gameObject);
            }
          
        }
    }
}