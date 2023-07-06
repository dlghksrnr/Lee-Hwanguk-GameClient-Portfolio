using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopPopup : MonoBehaviour
{
    [SerializeField]
    private Button dim; 
    [SerializeField]
    private Button btnClose; 
    
    public Button btnSelect; 
    [SerializeField]
    private Button btnCancel;

    public Image imgItem;
    public Image frame;
    [Header("EquipmentDetail")]
    public Text txtEquipmentNameDetail;
    public Text txtPowerStatDetail;
    public Text txtCriticalHitAmountDetail;
    public Text txtCriticalHitChanceDetail;
    public Text txtFireRateStatDetail;
    public Text txtEquipmentPriceDetail;

    [Header("FoodDetail")]
    public Text txtFoodNameDetail;
    public Text txtRecoveryAmount;
    public Text txtFoodPriceDetail;
    
    public System.Action onBuyItem;
    public System.Action onClosePopup;
    public System.Action onCurrentGold;
    public GameObject audioGo;
    public void Init()
    {
        var audioSource = this.audioGo.GetComponent<AudioSource>();
        this.dim.onClick.AddListener(() => { 
            this.gameObject.SetActive(false); 
        });
        this.btnClose.onClick.AddListener(() => {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.gameObject.SetActive(false);
        });
        this.btnSelect.onClick.AddListener(() => {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.gameObject.SetActive(false);
            this.onBuyItem();
            EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIShopGoodsCurrentGold);
        });
        this.btnCancel.onClick.AddListener(() => {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);
            this.gameObject.SetActive(false);
        });
        
        this.gameObject.SetActive(false);
    }
}
