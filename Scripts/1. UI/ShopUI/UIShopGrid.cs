using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopGrid : MonoBehaviour
{
    public enum eShopType
    {
        SANCTUARY,
        DUNGEON
    }
    private eShopType type;

    public UIShopPopup uIShopPopup;
    public Transform equipmentContent;
    public Transform foodContent;
    private ShopFactory shopFactory;
    private bool isGoldEnough;
    public EquipmentCell clickedCell;

    private string currentPopupEquipmentName;
    public bool isPlayerHPNotFull = false;
    public GameObject audioGo;
    public void Init(UIShopDirector.eShopScene sceneType)
    {
        var audioSource = this.audioGo.GetComponent<AudioSource>();
        this.shopFactory = this.GetComponent<ShopFactory>();
        this.uIShopPopup.Init();

        this.shopFactory.onUIPopupActive = () =>
        {
            this.uIShopPopup.gameObject.SetActive(true);
            this.PopupDetail();
            this.PopupDetailFrameColor();
            this.PopupDetailIcon();
            this.CheckCurrentGold();
        };
        this.uIShopPopup.onBuyItem = () =>
        {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            if (this.clickedCell.CompareTag("Equipment"))
            {
                var isEmptyCell = false;
                EventDispatcher.Instance.Dispatch<string, bool>(EventDispatcher.EventName.UIInventoryAddEquipment,
                    this.currentPopupEquipmentName, out isEmptyCell);
                if (this.isGoldEnough && isEmptyCell)
                {
                    AcceptBuyItem();
                    
                }
                else
                {
                    EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIInventoryDirectorMakeFullPopupText);
                }
}
            else if(this.clickedCell.CompareTag("Food"))
            {
                var playerHP = GameObject.FindObjectOfType<DungeonSceneMain>().playerHP < 3;
                var isPlayerHPNotFull=false;

                if (this.isGoldEnough && playerHP)
                {
                    AcceptBuyItem();

                    EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.DungeonSceneMainTakeFood, 
                        this.clickedCell.name, out isPlayerHPNotFull);
                }
                else if(!this.isGoldEnough || !playerHP)
                {
                    this.ButtonAlphaDown();
                }			
            }

        };

        if (sceneType == UIShopDirector.eShopScene.SANCTUARY) this.type = eShopType.SANCTUARY;
        else this.type = eShopType.DUNGEON;
    }

    public List<EquipmentCell> GenerateAllItems(eShopType type)
    {
        List<EquipmentCell> itemList = new List<EquipmentCell>();

        string[] itemTypes = { "Sword", "Axe", "Arrow", "Wand" };
        string[] itemGrades = { "Wood", "Iron", "Gold", "Diamond" };

        int itemCount = (type == eShopType.SANCTUARY) ? 9 : 5;

        for (int i = 0; i < itemCount; i++)
        {
            string itemType = itemTypes[UnityEngine.Random.Range(0, itemTypes.Length)];
            string itemGrade = itemGrades[UnityEngine.Random.Range(0, itemGrades.Length)];

            EquipmentCell item = null;

            switch (itemType)
            {
                case "Sword":
                    item = this.shopFactory.CreatShopSword(itemGrade, this.equipmentContent);
                    break;
                case "Axe":
                    item = this.shopFactory.CreatShopAxe(itemGrade, this.equipmentContent);
                    break;
                case "Arrow":
                    item = this.shopFactory.CreatShopArrow(itemGrade, this.equipmentContent);
                    break;
                case "Wand":
                    item = this.shopFactory.CreatShopWand(itemGrade, this.equipmentContent);
                    break;
                default:
                    Debug.LogWarning("Unknown item type: " + itemType);
                    break;
            }

            if (item != null)
            {
                itemList.Add(item);
                RectTransform itemRectTransform = item.GetComponent<RectTransform>();
                if (itemRectTransform != null)
                {
                    itemRectTransform.localScale = Vector3.one;
                }
            }
        }
        foreach (var item in itemList)
        {
            item.transform.SetParent(this.equipmentContent);
        }
        return itemList;
    }

    public void SanctuaryShopCell()
    {
        List<EquipmentCell> itemList = this.GenerateAllItems(eShopType.SANCTUARY);
        foreach (var item in itemList)
        {
            RectTransform itemRectTransform = item.GetComponent<RectTransform>();
            if (itemRectTransform != null)
            {
                itemRectTransform.localScale = Vector3.one;
            }
        }
    }

    public void DungeonShopCell()
    {
        List<EquipmentCell> itemList = this.GenerateAllItems(eShopType.DUNGEON);
        foreach (var item in itemList)
        {
            RectTransform itemRectTransform = item.GetComponent<RectTransform>();
            if (itemRectTransform != null)
            {
                itemRectTransform.localScale = Vector3.one;
            }
        }
    }


    public void FoodCell()
    {
        this.shopFactory.CreatShopFood("Iron", this.foodContent);
        this.shopFactory.CreatShopFood("Gold", this.foodContent);
        
    }
    
    private void PopupDetail()
    {
        this.clickedCell = EventSystem.current.currentSelectedGameObject.GetComponent<EquipmentCell>();

        if (this.clickedCell.tag == "Equipment")
        {
            this.uIShopPopup.txtEquipmentNameDetail.text = clickedCell.equipmentCellname;

            if (this.clickedCell.equipmentCellname.Contains("검"))
            {
                this.uIShopPopup.txtPowerStatDetail.gameObject.SetActive(true);
                this.uIShopPopup.txtCriticalHitAmountDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtCriticalHitChanceDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtFireRateStatDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtPowerStatDetail.text = $"공격력 <color=#00FF00>{clickedCell.powerStat}</color> 증가";
            }
            else if(this.clickedCell.equipmentCellname.Contains("할버드"))
            {
                this.uIShopPopup.txtPowerStatDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtCriticalHitAmountDetail.gameObject.SetActive(true);
                this.uIShopPopup.txtCriticalHitChanceDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtFireRateStatDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtCriticalHitAmountDetail.text = $"치명타공격력 <color=#00FF00>{clickedCell.criticalHitAmount.ToString()}</color> 증가";
            }
            else if (this.clickedCell.equipmentCellname.Contains("지팡이"))
            {
                this.uIShopPopup.txtPowerStatDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtCriticalHitAmountDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtCriticalHitChanceDetail.gameObject.SetActive(true);
                this.uIShopPopup.txtFireRateStatDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtCriticalHitChanceDetail.text = $"치명타확률 <color=#00FF00>{clickedCell.criticalHitChance.ToString()}</color> 증가";
            }
            else if (this.clickedCell.equipmentCellname.Contains("활"))
            {
                this.uIShopPopup.txtPowerStatDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtCriticalHitAmountDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtCriticalHitChanceDetail.gameObject.SetActive(false);
                this.uIShopPopup.txtFireRateStatDetail.gameObject.SetActive(true);
                this.uIShopPopup.txtFireRateStatDetail.text = $"공격속도 <color=#00FF00>{clickedCell.fireRateStat.ToString()}</color> 증가";
            }

            if (this.type == eShopType.SANCTUARY)
            {
                this.uIShopPopup.txtEquipmentPriceDetail.text = $"{clickedCell.itemPrice.ToString()+ "Gold"}";
            }
            else if (this.type == eShopType.DUNGEON)
            {
                this.uIShopPopup.txtEquipmentPriceDetail.text = $"{clickedCell.itemPrice.ToString()+ "Gold"}";
            }
        }
        else if (this.clickedCell.tag == "Food")
        {
            this.clickedCell.name = this.clickedCell.equipmentCellname;
            if(this.clickedCell.equipmentCellname=="Iron_Food")
            {
                this.uIShopPopup.txtFoodNameDetail.text = "고기";
            }
            else
            {
                this.uIShopPopup.txtFoodNameDetail.text = "포션";
            }
            this.uIShopPopup.txtFoodPriceDetail.text = $"{clickedCell.itemPrice.ToString()+ "Gold"}";
            this.uIShopPopup.txtRecoveryAmount.text = $"체력 <color=#00FF00>{clickedCell.recoveryAmount.ToString()}</color>개 회복";
        }
    }
    private void PopupDetailFrameColor()
    {
        this.clickedCell = EventSystem.current.currentSelectedGameObject.GetComponent<EquipmentCell>();
        var color = clickedCell.transform.GetChild(0).GetComponent<Image>().color;
        this.uIShopPopup.frame.color = color;
    }
    private void PopupDetailIcon()
    {
        this.clickedCell = EventSystem.current.currentSelectedGameObject.GetComponent<EquipmentCell>();
        var sprite = clickedCell.transform.GetChild(1).GetComponent<Image>().sprite;
        this.uIShopPopup.imgItem.sprite = sprite;
        this.currentPopupEquipmentName = sprite.name.Replace("(Clone)", "");
    }

    /// <summary>
    /// 유저의 현재 금액과 클릭한 아이템의 가격 비교
    /// 구매 버튼 활성/비활성
    /// 버튼 알파값 0.5/1
    /// </summary>
    private void CheckCurrentGold()
    {
        if (this.type == eShopType.SANCTUARY)
        {
            if (InfoManager.instance.possessionAmountInfo.goldAmount >= this.clickedCell.itemPrice)
            {
                this.uIShopPopup.btnSelect.enabled = true;
                this.isGoldEnough = true;
                this.ButtonAlphaUp();
            }
            else
            {
                this.uIShopPopup.btnSelect.enabled = false;
                this.isGoldEnough = false;
                this.ButtonAlphaDown();
            }
        }
        else
        {
            if (InfoManager.instance.possessionAmountInfo.dungeonGoldAmount >= this.clickedCell.itemPrice)
            {
                this.uIShopPopup.btnSelect.enabled = true;
                this.isGoldEnough = true;
                this.ButtonAlphaUp();
            }
            else
            {
                this.uIShopPopup.btnSelect.enabled = false;
                this.isGoldEnough = false;
                this.ButtonAlphaDown();
            }

        }
    }

    /// <summary>
    /// 거래 수락, InfoManager에서 유저의 금액 차감하고 save
    /// </summary>
    private void AcceptBuyItem()
    {
        if (this.type == eShopType.SANCTUARY)
        {
            InfoManager.instance.DecreasePossessionGoods(this.clickedCell.itemPrice);
        }
        else
        {
            InfoManager.instance.DecreaseDungeonGold(this.clickedCell.itemPrice);
        }
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UICurrencyDirectorUpdateGoldUI);

        this.ClickedCellOff();
    }
    private void ButtonAlphaUp()
    {
        var img = this.uIShopPopup.btnSelect.transform.GetChild(0).GetComponent<Image>();
        var btnText = this.uIShopPopup.btnSelect.transform.GetChild(1).GetComponent<Text>();
        btnText.text = "구매";
        btnText.color = Color.white;
        Color color = img.color;
        color.a = 1f;
        img.color = color;
    }
    private void ButtonAlphaDown()
    {
        if (!this.isGoldEnough)
        {
            var img = this.uIShopPopup.btnSelect.transform.GetChild(0).GetComponent<Image>();
            var btnText = this.uIShopPopup.btnSelect.transform.GetChild(1).GetComponent<Text>();
            btnText.text = "골드부족";
            btnText.color = Color.red;
            Color color = img.color;
            color.a = 0.5f;
            img.color = color;
        }
        else if (this.isPlayerHPNotFull)
        {
            var img = this.uIShopPopup.btnSelect.transform.GetChild(0).GetComponent<Image>();
            var btnText = this.uIShopPopup.btnSelect.transform.GetChild(1).GetComponent<Text>();
            btnText.text = "체력Full";
            btnText.color = Color.red;
            Color color = img.color;
            color.a = 0.5f;
            img.color = color;
        }
    }

    private void ClickedCellOff()
    {
        this.clickedCell.gameObject.GetComponent<Button>().enabled = false;
        var frame = this.clickedCell.gameObject.transform.GetChild(0).GetComponent<Image>();
        var icon = this.clickedCell.gameObject.transform.GetChild(1).GetComponent<Image>();
        Color colorFrame = frame.color;
        Color colorIcon = icon.color;
        colorFrame.a = 0.5f;
        colorIcon.a = 0.5f;
        frame.color = colorFrame;
        icon.color = colorIcon;
    }

}