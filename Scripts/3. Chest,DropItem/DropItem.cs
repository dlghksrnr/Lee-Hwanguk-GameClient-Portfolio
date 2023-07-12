using DG.Tweening;
using SpriteGlow;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private bool isSpace;
    private bool isPlayerHPNotFull = false;
    private bool isRelicNotFull = false;

    public int stepNum;
    public int stageNum;

    private float coinMaxHeight = 3f;
    private float coinDuration = 0.3f;

    public float floatHeight = 0.5f; // 두둥실 뜨는 높이
    public float floatDuration = 1f; // 두둥실 애니메이션 지속 시간

    private Vector3 coinStartPos;
    private Vector3 coinEndPos;

    [SerializeField] GameObject coinEffect;

    public void Init()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.coinStartPos = this.transform.position;
        this.coinEndPos = this.transform.position + Vector3.up * this.coinMaxHeight;
        this.FieldCoinAimStart();
        this.GetComponent<SpriteGlowEffect>().enabled = true;  
    }

    public void FloatingEffect(Vector3 startPos)
    {
        this.transform
            .DOMoveY(startPos.y + this.floatHeight, this.floatDuration)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void FieldCoinAimStart()
    {
        this.transform.DOLocalRotate(new Vector3(0, 720f, 0f), this.coinDuration * 2, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear);
        this.transform.DOMove(this.coinEndPos, this.coinDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                DOTween.Sequence()
                    .AppendInterval(0.1f)
                    .OnComplete(() =>
                    {
                        this.transform.DOMove(this.coinStartPos, this.coinDuration)
                        .SetEase(Ease.InQuad)
                        .OnComplete(() =>
                        {
                            this.transform.DOLocalRotate
                            (new Vector3(0, 360f, 0f), this.coinDuration * 2, RotateMode.LocalAxisAdd)
                            .SetEase(Ease.Linear)  
                            .SetLoops(-1);
                            this.GetComponent<BoxCollider2D>().enabled = true;
                        });
                    });
            });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string goName = this.gameObject.name;
        var relicNum = 0;
        if (collision.CompareTag("Player"))
        {
            Debug.LogFormat("dropItem:{0}", this.gameObject.name);
            switch (this.gameObject.tag)
            {
                case "Equipment":
                    EventDispatcher.Instance.Dispatch
                        (EventDispatcher.EventName.UIInventoryAddEquipment, goName, out this.isSpace);
                    this.GetEquipmentItem(collision.transform);
                    break;
                case "Food":
                    EventDispatcher.Instance.Dispatch<string, bool>
                        (EventDispatcher.EventName.DungeonSceneMainTakeFood,goName, out isPlayerHPNotFull);
                    this.GetFoodItem(collision.transform);
                    break;
                case "Relic":
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    EventDispatcher.Instance.Dispatch<string, int>
                        (EventDispatcher.EventName.PlayerShellTakeRelic,goName, out relicNum);
                    this.DOMoveToUIAndDestory(relicNum);
                    break;
                case "Weapon":
                    EventDispatcher.Instance.Dispatch<string>
                        (EventDispatcher.EventName.DungeonSceneMainTakeGun,goName);
                    this.GetWeaponItem();
                    break;
                case "Coin":
                    this.GetCoin();
                    break;
                case "Ether":
                    this.DOMoveToUIAndDestory();
                    break;
                case "FieldCoin":
                    this.DOMoveToUIAndDestory();
                    break;
            }
        }
    }
    public void GetEquipmentItem(Transform playerPos)
    {
        if (this.isSpace)
        {
            this.DOMoveToUIAndDestory();
        }
        else
        {
            EventDispatcher.Instance.Dispatch<Transform>
                (EventDispatcher.EventName.UIInventoryDirectorMakeFieldFullText, playerPos);
        }
    }

    /// <summary>
    /// isPlayerHPNotFull가 안됨
    /// </summary>
    /// <param name="playerPos"></param>
    public void GetFoodItem(Transform playerPos)
    {
        if (this.isPlayerHPNotFull)
        {
            var comp = Camera.main.transform.GetChild(4)
                .GetChild(0)
                .GetComponent<ParticleSystem>();
            comp.Play();
            Destroy(this.gameObject);
        } 
        else
        {
            EventDispatcher.Instance.Dispatch<Transform>
                (EventDispatcher.EventName.UIInventoryDirectorMakeFieldFullHealthText, playerPos);
        }
    }

    public void GetWeaponItem()
    {
        //이펙트 생성후 부수기
        Destroy(this.gameObject);
    }

    public void UpdateRelicUI()
    {
        EventDispatcher.Instance.Dispatch<string, int>
            (EventDispatcher.EventName.UIRelicDirectorTakeRelic,
            this.gameObject.name, out int temp);
        Destroy(this.gameObject);
    }
    public void GetCoin()
    {
        if (this.gameObject.tag == "FieldCoin")
        {
            var rewardGold = DataManager.Instance.GetRanFieldCoin();
            InfoManager.instance.IncreaseDungeonGold(rewardGold, true);
            EventDispatcher.Instance.Dispatch
                (EventDispatcher.EventName.UICurrencyDirectorUpdateGoldUI);
            var effect =GameObject.Instantiate(this.coinEffect);     
            var pos = this.transform.position;
            effect.transform.position = pos;    
            Destroy(effect,0.4f);
            this.gameObject.SetActive(false);
            
        }
        else
        {
            var rewardGold = DataManager.Instance.GetRewardGold(this.stepNum);
            Debug.LogFormat("<color=yellow>gold : {0}</color>", rewardGold);
            InfoManager.instance.IncreaseDungeonGold(rewardGold);
            EventDispatcher.Instance.Dispatch
                (EventDispatcher.EventName.UICurrencyDirectorUpdateGoldUI);
            for(int i = 0; i < 11; i++)
            {
                float delayTime = UnityEngine.Random.value;
                this.InstantiateCoin(delayTime);
            }
            Destroy(this.gameObject);
        }
    }

    private void InstantiateCoin(float delayTime, bool isEther = false)
    {
        var effect = GameObject.Instantiate(this.coinEffect);
        var pos = (Vector2)this.transform.position + UnityEngine.Random.insideUnitCircle;
        effect.transform.position = pos;
        if(isEther)
        {
            var glowComp = effect.GetComponent<SpriteGlowEffect>().GlowColor = Color.magenta;
            var spriteComp = effect.GetComponent<SpriteRenderer>().color = Color.magenta;
        }
         
        Destroy(effect, delayTime);
    }

    public void GetEhter()
    {
        var rewardEther = DataManager.Instance.GetRewardEther(this.stepNum);
        Debug.LogFormat("<color=yellow>ether : {0}</color>", rewardEther);
        InfoManager.instance.IncreaseEther(rewardEther);
        EventDispatcher.Instance.Dispatch
            (EventDispatcher.EventName.UICurrencyDirectorUpdateEtherUI);
        for (int i = 0; i < 11; i++)
        {
            float delayTime = UnityEngine.Random.value;
            this.InstantiateCoin(delayTime, true);
        }
        Destroy(this.gameObject);
    }


    /// <summary>
    /// 터치 반응 체크용
    /// </summary>
    public void ClickedItemCheck()
    {
        string goName = this.gameObject.name;
        var playerTrans = GameObject.FindWithTag("Player").transform;
        var relicNum = 0;
        switch (this.gameObject.tag)
        {
            case "Equipment":
                //인벤토리 용량 체크                        
                EventDispatcher.Instance.Dispatch(
                    EventDispatcher.EventName.UIInventoryAddEquipment,goName, out this.isSpace);
                this.GetEquipmentItem(playerTrans);
                break;
            case "Food":
                EventDispatcher.Instance.Dispatch<string, bool>
                    (EventDispatcher.EventName.DungeonSceneMainTakeFood,goName, out isPlayerHPNotFull);
                if(this.isPlayerHPNotFull) this.DOMoveToUIAndDestory();
                else
                {
                    EventDispatcher.Instance.Dispatch<Transform>
                        (EventDispatcher.EventName.UIInventoryDirectorMakeFieldFullHealthText, playerTrans);
                    this.gameObject.GetComponent<Collider2D>().enabled = true;
                }
                break;
            case "Relic":
                this.GetComponent<BoxCollider2D>().enabled = false;
                EventDispatcher.Instance.Dispatch<string, int>
                    (EventDispatcher.EventName.PlayerShellTakeRelic,goName, out relicNum);
                Debug.LogFormat("고대유물 넘버 : {0}", relicNum);
                this.DOMoveToUIAndDestory(relicNum);
                break;
            case "Weapon":
                EventDispatcher.Instance.Dispatch<string>
                    (EventDispatcher.EventName.DungeonSceneMainTakeGun,goName);
                this.GetWeaponItem();
                break;
            case "Coin":
                this.DOMoveToUIAndDestory();
                break;
            case "Ether":
                this.DOMoveToUIAndDestory();
                break;
            case "FieldCoin":
                this.DOMoveToUIAndDestory();
                break;
        }

    }

    private Vector3 targetPoint;
    private float duration = 0.5f;

    public void DOMoveToUIAndDestory(int rNum = 0)
    {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        this.transform.DOKill();
        var tag = this.gameObject.tag;
        var mainCamTrans = Camera.main.transform;
        this.transform.SetParent(mainCamTrans);
        var UITargeTrans = default(Transform);
        if (tag == "Equipment") UITargeTrans = mainCamTrans.GetChild(0);
        else if (tag == "Relic") UITargeTrans = mainCamTrans.GetChild(rNum);
        else UITargeTrans = mainCamTrans.GetChild(4);

        this.targetPoint = new Vector3(UITargeTrans.localPosition.x, UITargeTrans.localPosition.y,
            this.transform.localPosition.z);

        this.transform.DOLocalMove(this.targetPoint, this.duration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (tag == "Relic") this.UpdateRelicUI();
                else if (tag == "Coin") this.GetCoin();
                else if (tag == "FieldCoin") this.GetCoin();
                else if (tag == "Ether") this.GetEhter();
                else if (tag == "Food") this.GetFoodItem(UITargeTrans);
                else if (tag == "Equipment") this.ScaleAnim();
                else Destroy(this.gameObject);
            });
    }

    private void ScaleAnim()
    {
        EventDispatcher.Instance.Dispatch
            (EventDispatcher.EventName.UIInventoryDirectorButtonScaleAnim);
        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        this.transform.DOKill();
    }
}