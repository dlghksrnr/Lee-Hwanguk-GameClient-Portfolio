using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField]
    private GameObject axePrefab;
    [SerializeField]
    private GameObject wandPrefab;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private GameObject foodPrefab;

    public System.Action onUIPopupActive;

    public GameObject audioGo;

    //Sword
    public EquipmentCell CreatShopSword(string grade,Transform content)
    {
        AbsShopFactory swordFactory = new ShopSwordFactory();
        var swordGo=Instantiate(this.swordPrefab).GetComponent<EquipmentCell>();

        var btnSword=swordGo.GetComponent<Button>();
        var audioSource = this.audioGo.GetComponent<AudioSource>();
        btnSword.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.onUIPopupActive();
        });
        swordGo.equipmentCellname= swordFactory.MakeShopName(grade);
        swordGo.powerStat = swordFactory.MakeShopPowerStat(grade);
        swordGo.criticalHitAmount = swordFactory.MakeShopCriticalHitAmount(grade);
        swordGo.criticalHitChance = swordFactory.MakeShopCriticalHitChance(grade);
        swordGo.fireRateStat = swordFactory.MakeShopFireRateStat(grade);
        swordGo.recoveryAmount = swordFactory.MakeShopRecoveryAmount(grade);
        swordGo.itemPrice=swordFactory.MakeShopItemPrice(grade);

        swordGo.transform.GetChild(1).GetComponent<Image>().sprite= swordFactory.MakeShopIconImage(grade);
        swordGo.transform.GetChild(0).GetComponent<Image>().color= swordFactory.MakeFrameColor(grade);
        return swordGo;
    }

    //Axe
    public EquipmentCell CreatShopAxe(string grade, Transform content)
    {
        AbsShopFactory axeFactory = new ShopAxeFactory();
        var axeGo = Instantiate(this.axePrefab).GetComponent<EquipmentCell>();
        var audioSource = this.audioGo.GetComponent<AudioSource>();
        var btnAxeGo = axeGo.GetComponent<Button>();
        btnAxeGo.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.onUIPopupActive();
        });

        axeGo.equipmentCellname = axeFactory.MakeShopName(grade);
        axeGo.powerStat = axeFactory.MakeShopPowerStat(grade);
        axeGo.criticalHitAmount = axeFactory.MakeShopCriticalHitAmount(grade);
        axeGo.criticalHitChance = axeFactory.MakeShopCriticalHitChance(grade);
        axeGo.fireRateStat = axeFactory.MakeShopFireRateStat(grade);
        axeGo.recoveryAmount = axeFactory.MakeShopRecoveryAmount(grade);
        axeGo.itemPrice=axeFactory.MakeShopItemPrice(grade);

        axeGo.transform.GetChild(1).GetComponent<Image>().sprite = axeFactory.MakeShopIconImage(grade);
        axeGo.transform.GetChild(0).GetComponent<Image>().color = axeFactory.MakeFrameColor(grade);
        return axeGo;
    }

    //Arrow
    public EquipmentCell CreatShopArrow(string grade, Transform content)
    {
        AbsShopFactory arrowFactory = new ShopArrowFactory();
        var arrowGo = Instantiate(this.arrowPrefab).GetComponent<EquipmentCell>();
        var audioSource = this.audioGo.GetComponent<AudioSource>();

        var btnArrowGo = arrowGo.GetComponent<Button>();
        btnArrowGo.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.onUIPopupActive();
        });

        arrowGo.equipmentCellname = arrowFactory.MakeShopName(grade);
        arrowGo.powerStat = arrowFactory.MakeShopPowerStat(grade);
        arrowGo.criticalHitAmount = arrowFactory.MakeShopCriticalHitAmount(grade);
        arrowGo.criticalHitChance = arrowFactory.MakeShopCriticalHitChance(grade);
        arrowGo.fireRateStat = arrowFactory.MakeShopFireRateStat(grade);
        arrowGo.recoveryAmount = arrowFactory.MakeShopRecoveryAmount(grade);
        arrowGo.itemPrice=arrowFactory.MakeShopItemPrice(grade);

        arrowGo.transform.GetChild(1).GetComponent<Image>().sprite = arrowFactory.MakeShopIconImage(grade);
        arrowGo.transform.GetChild(0).GetComponent<Image>().color = arrowFactory.MakeFrameColor(grade);
        return arrowGo;
    }

    //Wand
    public EquipmentCell CreatShopWand(string grade, Transform content)
    {
        AbsShopFactory wandFactory = new ShopWandFactory();
        var wandGo = Instantiate(this.wandPrefab).GetComponent<EquipmentCell>();
        var audioSource = this.audioGo.GetComponent<AudioSource>();

        var btnWandGo = wandGo.GetComponent<Button>();
        btnWandGo.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.onUIPopupActive();
        });

        wandGo.equipmentCellname = wandFactory.MakeShopName(grade);
        wandGo.powerStat = wandFactory.MakeShopPowerStat(grade);
        wandGo.criticalHitAmount = wandFactory.MakeShopCriticalHitAmount(grade);
        wandGo.criticalHitChance = wandFactory.MakeShopCriticalHitChance(grade);
        wandGo.fireRateStat = wandFactory.MakeShopFireRateStat(grade);
        wandGo.recoveryAmount = wandFactory.MakeShopRecoveryAmount(grade);
        wandGo.itemPrice=wandFactory.MakeShopItemPrice(grade);

        wandGo.transform.GetChild(1).GetComponent<Image>().sprite = wandFactory.MakeShopIconImage(grade);
        wandGo.transform.GetChild(0).GetComponent<Image>().color = wandFactory.MakeFrameColor(grade);
        return wandGo;
    }

    //Food
    public EquipmentCell CreatShopFood(string grade, Transform content)
    {
        AbsShopFactory foodFactory = new ShopFoodFactory();
        var foodGo = Instantiate(this.foodPrefab,content).GetComponent<EquipmentCell>();
        var audioSource = this.audioGo.GetComponent<AudioSource>();

        var btnFoodGo = foodGo.GetComponent<Button>();
        btnFoodGo.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.onUIPopupActive();
        });

        foodGo.equipmentCellname = foodFactory.MakeShopName(grade);
        foodGo.powerStat = foodFactory.MakeShopPowerStat(grade);
        foodGo.criticalHitAmount = foodFactory.MakeShopCriticalHitAmount(grade);
        foodGo.criticalHitChance = foodFactory.MakeShopCriticalHitChance(grade);
        foodGo.fireRateStat = foodFactory.MakeShopFireRateStat(grade);
        foodGo.recoveryAmount = foodFactory.MakeShopRecoveryAmount(grade);
        foodGo.itemPrice=foodFactory.MakeShopItemPrice(grade);

        foodGo.transform.GetChild(1).GetComponent<Image>().sprite = foodFactory.MakeShopIconImage(grade);
        foodGo.transform.GetChild(0).GetComponent<Image>().color = foodFactory.MakeFrameColor(grade);
        //foodGo.name = foodFactory.MakeShopIconImage(grade).ToString().Replace("(Clone)(UnityEngine.Sprite)","");
        //Debug.LogFormat("<color=red>foodName:{0}</color>",foodGo.name);
        return foodGo;
    }
}
