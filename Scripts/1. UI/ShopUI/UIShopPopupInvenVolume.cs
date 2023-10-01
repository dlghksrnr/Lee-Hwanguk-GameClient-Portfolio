using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopPopupInvenVolume : MonoBehaviour
{
    public Button dim;
    public Button btnClose;

    public Button btnSelect;
    public Button btnCancle;

    public Text txtCurCount;
    public Text txtIcon;
    public Text txtIncCount;

    public Text txtPriceDetail;
    public Text txtCurrentGoldDetail;
    private bool isGoldEnough;
    public GameObject audioGo;

    private int rounCount;
    private int invecnPrice;
    public void Init()
    {
        this.rounCount = InfoManager.instance.gameInfo.roundCnt;
        var audioSource = this.audioGo.GetComponent<AudioSource>();
        this.txtCurCount.text = InfoManager.instance.inventoryInfo.InventoryCount.ToString();
        this.txtIncCount.text = (InfoManager.instance.inventoryInfo.InventoryCount + 1).ToString();

        this.invecnPrice = this.CalculateInventoryPrice(InfoManager.instance.inventoryInfo.InventoryCount);
        
        this.txtPriceDetail.text = invecnPrice.ToString();

        this.txtCurrentGoldDetail.text = InfoManager.instance.possessionAmountInfo.goldAmount.ToString();

        this.isGoldEnough = true;
        this.dim.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
        });

        this.btnClose.onClick.AddListener(() => {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.gameObject.SetActive(false);

        });

        this.btnCancle.onClick.AddListener(() => {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.gameObject.SetActive(false);
        });

        this.btnSelect.onClick.AddListener(() => {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.CheckEnoughGold();
            this.IncreaseInventoryVolum();
        });

    }
    public void CheckEnoughGold()
    {
        this.txtCurCount.text = InfoManager.instance.inventoryInfo.InventoryCount.ToString();
        this.txtIncCount.text = $"<color=#00FF00>{(InfoManager.instance.inventoryInfo.InventoryCount + 1).ToString()}</color>";
        this.txtCurrentGoldDetail.text = " / " + InfoManager.instance.possessionAmountInfo.goldAmount.ToString();

        int invenCount = InfoManager.instance.inventoryInfo.InventoryCount;
        int maxInvenCnt = InfoManager.instance.inventoryInfo.maxInvenCount;
        this.invecnPrice = this.CalculateInventoryPrice(InfoManager.instance.inventoryInfo.InventoryCount);


        if (InfoManager.instance.possessionAmountInfo.goldAmount >= this.CalculateInventoryPrice(InfoManager.instance.inventoryInfo.InventoryCount)&& //this.invecnPrice &&
            invenCount < (maxInvenCnt+(this.rounCount*2)))
        {
            EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UICurrencyDirectorUpdateEtherUI);
            this.isGoldEnough = true;
            this.txtCurCount.text = (invenCount).ToString();
            this.txtIncCount.text = (invenCount + 1).ToString();
            this.txtPriceDetail.text = this.CalculateInventoryPrice(InfoManager.instance.inventoryInfo.InventoryCount).ToString();
            this.OnSelectButton();
        }
        else
        {
            this.isGoldEnough = false;
            if(invenCount== (maxInvenCnt + (this.rounCount * 2)))
            {
                this.txtCurCount.text = "<Color=red>Max</Color>";
                this.txtIncCount.text = "<Color=red>Max</Color>";
                
                this.txtPriceDetail.text = this.CalculateInventoryPrice(InfoManager.instance.inventoryInfo.InventoryCount).ToString();
                this.OffSelectButton();
            }
            else if(InfoManager.instance.possessionAmountInfo.goldAmount< this.CalculateInventoryPrice(InfoManager.instance.inventoryInfo.InventoryCount))//this.invecnPrice) //2000
            {
                this.txtCurCount.text = "";
                this.txtIcon.text = "";
                this.txtIncCount.text = "<Color=red>Gold 부족</Color>";
                this.txtIncCount.rectTransform.anchoredPosition = new Vector3(-11f, -103f, 0f);
                this.txtPriceDetail.text = this.CalculateInventoryPrice(InfoManager.instance.inventoryInfo.InventoryCount).ToString();
                this.OffSelectButton();
            }
        }
    }

    /// <summary>
    /// 인벤 용량 증가메서드, 임시 최대 용량 9
    /// </summary>
    private void IncreaseInventoryVolum()
    {
        int invenCount = InfoManager.instance.inventoryInfo.InventoryCount;
        int maxInvenCnt = InfoManager.instance.inventoryInfo.maxInvenCount;
        this.invecnPrice = this.CalculateInventoryPrice(InfoManager.instance.inventoryInfo.InventoryCount);


        if (this.isGoldEnough)
        {
            invenCount =InfoManager.instance.IncreaseInventoryCount();
            this.txtCurCount.text = (invenCount).ToString();
            this.txtIncCount.text = $"<color=#00FF00>{(invenCount + 1).ToString()}</color>";
            this.ConsumptionGold();
            EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIInventoryAddCell);
            this.txtPriceDetail.text = this.CalculateInventoryPrice(InfoManager.instance.inventoryInfo.InventoryCount).ToString();
        }
        else if (!this.isGoldEnough)
        {
            if (invenCount == (maxInvenCnt + (this.rounCount * 2)))
            {
                this.txtCurCount.text = "<Color=red>Max</Color>";
                this.txtIncCount.text = "<Color=red>Max</Color>";
            }
            else if (InfoManager.instance.possessionAmountInfo.goldAmount < this.CalculateInventoryPrice(InfoManager.instance.inventoryInfo.InventoryCount))//this.invecnPrice) //2000
            {
                this.txtCurCount.text = "";
                this.txtIcon.text = "";
                this.txtIncCount.text = "<Color=red>Gold 부족</Color>"; 
                this.txtIncCount.rectTransform.anchoredPosition = new Vector3(-11f, -103f, 0f);
            }
        }
    }

    /// <summary>
    /// 인벤토리 용량 증가 가격 설정
    /// </summary>
    /// <param name="inventorySize"></param>
    /// <returns></returns>
    private int CalculateInventoryPrice(int inventorySize)
    {
        int basePrice = 5000;
        int maxPrice = 55000; 
        int priceIncrement = 5000; 
        int additionalIncrement = 3000;

        int extraSlots = Mathf.Max(0, inventorySize - 5); 

        int price = basePrice + (extraSlots * priceIncrement) + (extraSlots * (extraSlots - 1) / 2) * additionalIncrement;
        return Mathf.Min(price, maxPrice);
    }

    private void OffSelectButton()
    {
        //버튼 비활성화, 버튼 알파값 줄이기
        var btn = this.btnSelect.GetComponent<Button>();
        btn.enabled = false;

        var image = this.btnSelect.transform.GetChild(0).gameObject.GetComponent<Image>();
        Color color = image.color;
        color.a = 0.5f;
        image.color = color;
    }
    private void OnSelectButton()
    {
        //버튼 비활성화, 버튼 알파값 줄이기
        var btn = this.btnSelect.GetComponent<Button>();
        btn.enabled = true;

        var image = this.btnSelect.transform.GetChild(0).gameObject.GetComponent<Image>();
        Color color = image.color;
        color.a = 1f;
        image.color = color;
    }

    /// <summary>
    /// 용량증가 비용, 임시값 1300Gold
    /// </summary>
    private void ConsumptionGold()
    {
        InfoManager.instance.DecreasePossessionGoods(this.invecnPrice); //2000
        this.txtCurrentGoldDetail.text = " / " + InfoManager.instance.possessionAmountInfo.goldAmount.ToString();
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIShopGoodsCurrentGold);
    }
}
