using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    [Header("dim")]
    public Button dim;

    [Header("Tab")]
    public Button[] arrTabBtn;
    public GameObject[] arrTabFocus;

    [Header("Graid")]
    public UIShopGrid uiShopGride;

    [Header("Close")]
    public Button btnClose;

    [Header("ShopPopup")]
    public UIShopPopup uiShopPopup;

    [Header("ShopPopupInvenVolume")]
    public UIShopPopupInvenVolume uIShopPoupInvenVolume;
    public Button btnInventroyVolume;

    [Header("UIShopGoods")]
    public UIShopGoods uIShopGoods;

    [SerializeField] private Text txtGold;
    public System.Action onClosing;

    UIShopDirector.eShopScene type;
    public GameObject audioGo;


    public void Init(UIShopDirector.eShopScene sceneType)
    {
        var audioSource = this.audioGo.GetComponent<AudioSource>();

        this.uIShopGoods.Init();
        this.type = sceneType;  
        this.dim.onClick.AddListener(() => {
            this.onClosing();
        });
        this.btnClose.onClick.AddListener(() => {
            this.onClosing();
        });

        /// <summary>
        /// 인벤토리 용량 추가
        /// 인벤토리 용량의 최대(9)을 체크 한 후 최대 값 "이하" 라면 용량 추가         
        /// </summary>
        if (this.uIShopPoupInvenVolume != null && this.btnInventroyVolume != null){
            this.uIShopPoupInvenVolume.Init();
            this.btnInventroyVolume.onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
                this.uIShopPoupInvenVolume.gameObject.SetActive(true);
                this.uIShopPoupInvenVolume.CheckEnoughGold();
            });
        }
        this.arrTabBtn[0].onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.OnFocusNormalTabClick();
            this.uiShopPopup.txtEquipmentNameDetail.text = "일반장비";
            this.uiShopGride.equipmentContent.gameObject.SetActive(true);
            this.uiShopGride.foodContent.gameObject.SetActive(false);
        });
        this.arrTabBtn[1].onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.OnFocusFoodTabClick();
            this.uiShopPopup.txtEquipmentNameDetail.text = "소모아이템";
            this.uiShopGride.equipmentContent.gameObject.SetActive(false);
            this.uiShopGride.foodContent.gameObject.SetActive(true);
        });
        this.TabInit();
        this.uiShopGride.Init(sceneType);
    }

    private void TabInit()
    {
        this.arrTabFocus[0].gameObject.SetActive(true);
        this.arrTabFocus[1].gameObject.SetActive(false);
        this.uiShopPopup.transform.Find("frame").Find("FoodTabDetail").gameObject.SetActive(false);
        this.uiShopPopup.transform.Find("frame").Find("NormalTabDetail").gameObject.SetActive(true);
    }

    private void OnFocusNormalTabClick()
    {
        this.arrTabFocus[0].SetActive(true);
        this.arrTabFocus[1].SetActive(false);
        this.uiShopPopup.transform.Find("frame").Find("FoodTabDetail").gameObject.SetActive(false);
        this.uiShopPopup.transform.Find("frame").Find("NormalTabDetail").gameObject.SetActive(true);
    }
    private void OnFocusFoodTabClick()
    {
        this.arrTabFocus[0].SetActive(false);
        this.arrTabFocus[1].SetActive(true);
        this.uiShopPopup.transform.Find("frame").Find("FoodTabDetail").gameObject.SetActive(true);
        this.uiShopPopup.transform.Find("frame").Find("NormalTabDetail").gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (this.gameObject.activeSelf)
        {
            if (this.type == UIShopDirector.eShopScene.DUNGEON)
                this.txtGold.text = InfoManager.instance.possessionAmountInfo.dungeonGoldAmount.ToString();
            else if (this.type == UIShopDirector.eShopScene.SANCTUARY)
                this.txtGold.text = InfoManager.instance.possessionAmountInfo.goldAmount.ToString();
        }      
    }

}
