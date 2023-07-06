using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIDice : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button closeBtn;
    public GameObject btnGo;
    public Button rollBtn;
    public DiceScript dice;
    public Button dim;
    public Text txtDicePrice;
    public System.Action onClosing;

    public UIdiceResultPopup resultPopup;
    public bool isEnugh;
    private bool isSpace;
    private List<string> curList;
    private int inventCellCount;
    private int dicePrice;
    public GameObject audioGo;

    private void OnEnable()
    {
        Debug.Log("DiceActive");
        this.txtDicePrice.text = DataManager.Instance.GetGamblePrice(InfoManager.instance.dungeonInfo.CurrentStageInfo).ToString() + "Gold";
    }
    public void Init()
    {
        var audioSource = this.audioGo.GetComponent<AudioSource>();

        this.isEnugh = true;
        this.isSpace = true;

        this.dice.Init();
        this.rollBtn = this.btnGo.GetComponent<Button>();

        this.closeBtn.onClick.AddListener(() => {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);

            this.onClosing();
        });
        this.dim.onClick.AddListener(() => {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);

            this.onClosing();
        });
        this.rollBtn.onClick.AddListener(() => {
            AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.UI_Close, audioSource);

            this.dice.fireWorksParticle.gameObject.SetActive(false);
            this.curList = default(List<string>);
            EventDispatcher.Instance.Dispatch<List<string>>(EventDispatcher.EventName.UICurrentInventoryList,
                out curList);
            this.inventCellCount = InfoManager.instance.inventoryInfo.InventoryCount;
            Debug.LogFormat("현재 인벤토리 갯수 : {0}, 현재 들고 있는 아이템 리스트 갯수 : {1}", inventCellCount, curList.Count);

            this.isSpace = curList.Count < inventCellCount;
            if (isSpace)
            {
                this.dicePrice=DataManager.Instance.GetGamblePrice(InfoManager.instance.dungeonInfo.CurrentStageInfo);
                Debug.LogFormat("DicePrice:{0}", this.dicePrice);
                this.isEnugh = InfoManager.instance.DecreaseDungeonGold(this.dicePrice);  // this.dicePrice 너무 비쌈, 가격을 낮출 필요가 있음.
                Debug.Log(InfoManager.instance.possessionAmountInfo.dungeonGoldAmount);
                if (isEnugh)
                {
                    this.dice.RollDice();
                    StartCoroutine(this.RollDiceBtnSetting());
                    this.dice.DiceResultImgInit();
                }
                else if (!isEnugh)
                {
                    StartCoroutine(this.CantDiceBtnSetting());
                }
            }
            else if (!isSpace)
            {
                StartCoroutine(this.CantDiceBtnSetting());
            }
            else if(this.dice.isRolling)
            {

                StartCoroutine(this.CantDiceBtnSetting());
            }
        });


        this.resultPopup.Init();
    }

    //ResultPopup
    public void OnPointerDown(PointerEventData eventData) //터치 범위 축소 필요(frame으로 터치하게)
    {
        this.resultPopup.gameObject.SetActive(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        this.resultPopup.gameObject.SetActive(false);
    }

    //DiceResult
    private IEnumerator RollDiceBtnSetting()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            this.closeBtn.interactable = false;
            this.dim.interactable = false;
            this.BtnGoColorDown();
            this.rollBtn.interactable = false;
            if (this.dice.rb.velocity.magnitude == 0)
            {
                this.dice.StopDice();
                this.rollBtn.interactable = true;
                this.BtnGoColorUp();
                this.dice.DiceResultImg();
                this.closeBtn.interactable = true;
                this.dim.interactable = true;

                //Percentage
                this.dice.GradePercentage();
                this.dice.TypePercentage();
                yield break;
            }           
        }
    }
    public IEnumerator CantDiceBtnSetting()
    {
        var btnTxt = this.btnGo.transform.GetChild(1).GetComponent<Text>();
        this.BtnGoColorDown();
        this.rollBtn.interactable = false;
        this.closeBtn.interactable = false;
        this.dim.interactable = false;
        Debug.LogFormat("{0}/{1}", isEnugh, isSpace);
        if (!isEnugh)
        {
            btnTxt.text = "Gold부족";
            yield return new WaitForSeconds(1f);
        }
        else if (!isSpace)
        {
            EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIInventoryDirectorMakeFullPopupText);
            btnTxt.text = "용량부족";
            yield return new WaitForSeconds(1f);
        }
        else if(this.dice.isRolling)
        {
            btnTxt.text = "Diamond!";
            Debug.Log("Dice Rolling!");
            yield return new WaitForSeconds(5f);
        }
        //yield return new WaitForSeconds(1f);
        this.BtnGoColorUp();
        this.rollBtn.interactable = true;
        this.closeBtn.interactable = true;
        this.dim.interactable = true;
        btnTxt.text = "Go!";
        this.isEnugh = true;
        this.isSpace= true;
    }

    //ButtonSetting
    private void BtnGoColorDown() //알파값 down
    {
        var imgBoxGo=this.btnGo.transform.Find("imgBox").GetComponent<Image>();
        var txtSelectGo = this.btnGo.transform.Find("txtSelect").GetComponent<Text>();

        Color imgBoxGoColor = imgBoxGo.color;
        imgBoxGoColor.a = 0.5f;
        imgBoxGo.color= imgBoxGoColor;

        Color txtSelectGoColor= txtSelectGo.color;
        txtSelectGoColor.a = 0.5f;
        txtSelectGo.color= txtSelectGoColor;
    }
    private void BtnGoColorUp() //알파값 up
    {
        var imgBoxGo = this.btnGo.transform.Find("imgBox").GetComponent<Image>();
        var txtSelectGo = this.btnGo.transform.Find("txtSelect").GetComponent<Text>();

        Color imgBoxGoColor = imgBoxGo.color;
        imgBoxGoColor.a = 1f;
        imgBoxGo.color = imgBoxGoColor;

        Color txtSelectGoColor = txtSelectGo.color;
        txtSelectGoColor.a = 1f;
        txtSelectGo.color = txtSelectGoColor;
    }

    private void OnDisable()
    {
        Debug.Log("DiceActiveOff");
        this.dice.fireWorksParticle.gameObject.SetActive(false);
    }
}
