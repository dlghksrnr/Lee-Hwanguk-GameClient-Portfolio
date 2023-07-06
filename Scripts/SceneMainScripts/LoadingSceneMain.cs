using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class LoadingSceneMain : MonoBehaviour
{
    [SerializeField] private UILoadingDirector loadingDirector;
    public System.Action onComplete;

    public void Init(SceneArgs args)
    {
        AssetManager.Instance.onProgress = (per) =>
        {
            if (per >= 1)
            {
                this.onComplete();

            }
            this.loadingDirector.UpdateUI(per);
        };
        AssetManager.Instance.LoadAllAssets(args);

        if (InfoManager.instance.gameInfo.isDungeonEntered)
        {
            var adMob = GameObject.FindObjectOfType<GoogleAdMobController>();
            if (adMob != null)
            {
                adMob.RequestAndLoadRewardedAd();
            }
        }
    }
}
