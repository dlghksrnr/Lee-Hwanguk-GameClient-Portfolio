using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopDirector : MonoBehaviour
{
    public enum eShopScene
    {
        SANCTUARY,
        DUNGEON
    }

    public eShopScene shopType;
    public UIShop uiShop;
    public UIShopGrid uiShopGride;
    public UIShopPopup uiShopPopup;
    public UIShopGoods uiShopGoods;

    public GameObject audioGo;

    public void SanctuaryShopInit()
    {
        var audioSource=this.audioGo.GetComponent<AudioSource>();
        this.uiShopGoods.type=UIShopGoods.eShopSceneType.SANCTUARY;
        this.shopType = eShopScene.SANCTUARY;
        this.uiShop.arrTabBtn[0].gameObject.SetActive(true);
        this.uiShop.arrTabBtn[1].gameObject.SetActive(false);
        this.uiShop.Init(eShopScene.SANCTUARY);
        this.uiShopGride.equipmentContent.gameObject.SetActive(true);
        this.uiShopGride.foodContent.gameObject.SetActive(false);
        this.uiShopGride.SanctuaryShopCell();

        this.uiShop.onClosing = () => {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.gameObject.SetActive(false);
        };

        this.gameObject.SetActive(false);
    }
    public void DungeonShopInit()
    {
        var audioSource = this.audioGo.GetComponent<AudioSource>();
        this.uiShopGoods.type = UIShopGoods.eShopSceneType.DUNGEON;
        this.shopType = eShopScene.DUNGEON;
        this.uiShop.arrTabBtn[0].gameObject.SetActive(true);
        this.uiShop.arrTabBtn[1].gameObject.SetActive(true);
        this.uiShop.Init(eShopScene.DUNGEON);

        this.uiShopGride.equipmentContent.gameObject.SetActive(true);
        this.uiShopGride.foodContent.gameObject.SetActive(false);
        this.uiShopGride.DungeonShopCell();
        this.uiShopGride.FoodCell();

        this.uiShop.onClosing = () => {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.gameObject.SetActive(false); 
        };

        this.gameObject.SetActive(false);
    }
}
