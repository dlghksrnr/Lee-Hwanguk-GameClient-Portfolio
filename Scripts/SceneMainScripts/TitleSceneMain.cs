using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneMain : MonoBehaviour
{
    public UITitleDirector uiTitleDirector;

    public void Init()
    {
        this.uiTitleDirector.Init();
        this.uiTitleDirector.FadeIn();
        AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.TITLEBG);
        InfoManager.instance.gameInfo.isDungeonEntered = false;
    }
}
