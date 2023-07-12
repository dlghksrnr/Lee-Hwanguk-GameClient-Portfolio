using DG.Tweening;
using SpriteGlow;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceScript : MonoBehaviour
{
    private enum ErewardGrade
    {
        Wood = 0,
        Iron = 1,
        Gold = 2,
        Diamond = 3
    }
    private enum ErewardType
    {
        Arrow = 0,
        Axe = 1,
        Sword = 2,
        Wand = 3
    }
    private ErewardType eRewardType;
    private ErewardGrade eRewardGrade;
    public UIdiceResultPopup uIdiceResultPopup;
    public Rigidbody rb;
    public GameObject[] resultImg;
    public ParticleSystem fireWorksParticle;
    public UIDice uiDice;
    public bool isRolling=false;
    public Tween diceTween;
    private SpriteGlowEffect glowEffect;

    public void Init()
    {
        this.rb = GetComponent<Rigidbody>();
        this.fireWorksParticle.gameObject.SetActive(false);
        this.glowEffect = this.resultImg[1].GetComponent<SpriteGlowEffect>();
        this.glowEffect.enabled = false;
    }
    public void RollDice()
    {
        this.rb.isKinematic = false;
        this.isRolling= true;
        float dirX = Random.Range(200, 700); 
        float dirY = Random.Range(200, 700);
        float dirZ = Random.Range(200, 700);
        this.transform.localPosition = new Vector3(0, 2f, 0); 
        this.transform.rotation = Quaternion.identity; 
        rb.AddForce(this.transform.up * 400); 
        rb.AddTorque(dirX, dirY, dirZ);

    }

    public void StopDice()
    {
        var fireWorksPos = GameObject.FindObjectOfType<MainCameraController>().transform.GetChild(6).
            GetComponent<Transform>().position;

        this.rb.isKinematic = true;
        if (this.eRewardGrade == ErewardGrade.Wood || this.eRewardGrade == ErewardGrade.Iron
            || this.eRewardGrade == ErewardGrade.Gold)
        {
            this.transform.DOMove(new Vector3(0, 0.5f, 0), 1f).SetEase(Ease.OutExpo);
        }

        else if (this.eRewardGrade == ErewardGrade.Diamond)
        {
            this.isRolling = true;

            Sequence sequence = DOTween.Sequence();

            sequence.AppendCallback(() =>
            {
                StartCoroutine(this.uiDice.CantDiceBtnSetting());
            });

            sequence.Append(this.transform.DOMove(new Vector3(0, 1.5f, 0), 0.75f).SetEase(Ease.OutExpo));

            sequence.AppendCallback(() =>
            {
                StartCoroutine(DoShakeCoroutine()); 

            });

            sequence.AppendCallback(() => StartCoroutine(CoFireWorksParticle(fireWorksPos)));

            sequence.Append(this.transform.DOMove(new Vector3(0, 3f, 0), 0.75f).SetEase(Ease.OutExpo));

            sequence.Play();

        }

        Quaternion targetRotation = Quaternion.Euler(0f, -360f, 0f);
        this.transform.DORotate(targetRotation.eulerAngles, 0.01f).SetEase(Ease.OutExpo);
    }
    private IEnumerator CoFireWorksParticle(Vector3 fireWorksPosition)
    {
        this.fireWorksParticle.gameObject.SetActive(true);

        this.fireWorksParticle.transform.position = new Vector3(fireWorksPosition.x, fireWorksPosition.y, fireWorksPosition.z + 22f);
        this.fireWorksParticle.Play();

        yield return new WaitForSeconds(15f);

        this.isRolling = false;
        this.fireWorksParticle.Stop();
        this.fireWorksParticle.gameObject.SetActive(false);
    }
    private IEnumerator DoShakeCoroutine()
    {
        Vector3 originalPosition = this.transform.position;

        float shakeDuration = 2f;
        float shakeStrength = 0.1f;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeStrength;
            this.transform.position = originalPosition + randomOffset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        this.transform.position = originalPosition;
    }

    public void GradePercentage()
    {
        int ranGrade =  Random.Range(0, 4);
        this.eRewardGrade = (ErewardGrade)ranGrade;

    }
    public void TypePercentage()
    {
        int ranType = Random.Range(0, 4);
        this.eRewardType = (ErewardType)ranType;
    }
    public string ResultIcon()
    {
        var resultGrade = this.eRewardGrade.ToString();
        var resultType = this.eRewardType.ToString();
        var resultIcon = resultGrade + "_" + resultType;

        EventDispatcher.Instance.Dispatch<string, bool>(EventDispatcher.EventName.UIInventoryAddEquipment,
            resultIcon, out bool shit);
       
        var resultName=DataManager.Instance.GetNameByDiceResultIcon(resultIcon);
        this.uIdiceResultPopup.txtName.text = resultName.name;
        this.uIdiceResultPopup.txtPowerStat.text = "공격력 : "+ resultName.powerStat.ToString();
        this.uIdiceResultPopup.txtCriticalAmount.text ="치명타 피해 : "+resultName.criticalHitAmount.ToString();
        this.uIdiceResultPopup.txtCriticalChance.text ="치명타 확률 : "+resultName.criticalHitChance.ToString();
        this.uIdiceResultPopup.txtRateStat.text ="연사력 : "+resultName.fireRateStat.ToString();
        return resultIcon;
    }
    public void ShowResultIcon()
    {
        var resultSprite = this.resultImg[0].GetComponent<SpriteRenderer>();
        this.glowEffect = this.resultImg[1].GetComponent<SpriteGlowEffect>();

        if (this.eRewardGrade==ErewardGrade.Diamond)
        {
            this.glowEffect.enabled = true;
        }
        else
        {
            this.glowEffect.enabled = false;
        }

        var resultSpriteAtlas = AtlasManager.instance.GetAtlasByName("UIEquipmentIcon");
        resultSprite.sprite = resultSpriteAtlas.GetSprite(ResultIcon());
        this.resultImg[0].gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

        this.uIdiceResultPopup.resultImgIcon.sprite = resultSprite.sprite;
        this.uIdiceResultPopup.resultImgIcon.rectTransform.localScale = Vector3.one;
        this.ShoewResultFrame();
    }

    public void ShoewResultFrame()
    {
        Color woodFrame = new Color();
        ColorUtility.TryParseHtmlString("#B05200", out woodFrame);

        Color ironFrame = new Color();
        ColorUtility.TryParseHtmlString("#C0B2A6", out ironFrame);

        Color goldFrame = new Color();
        ColorUtility.TryParseHtmlString("#FFEE00", out goldFrame);

        Color diamondFrame = new Color();
        ColorUtility.TryParseHtmlString("#00FFBC", out diamondFrame);

        var resultFrame = this.resultImg[1].GetComponent<SpriteRenderer>();
        switch (this.eRewardGrade)
        {
            case ErewardGrade.Wood:
                resultFrame.color = woodFrame;
                this.uIdiceResultPopup.resultImgFrame.color = woodFrame;
                break;
            case ErewardGrade.Iron:
                resultFrame.color = ironFrame;
                this.uIdiceResultPopup.resultImgFrame.color = ironFrame;
                break;
            case ErewardGrade.Gold:
                resultFrame.color = goldFrame;
                this.uIdiceResultPopup.resultImgFrame.color = goldFrame;
                break;
            case ErewardGrade.Diamond:
                resultFrame.color = diamondFrame;
                this.uIdiceResultPopup.resultImgFrame.color = diamondFrame;
                break;
        }
    }

    public void DiceResultImg()
    {
        this.resultImg[0].gameObject.SetActive(true);
        this.resultImg[1].gameObject.SetActive(true); 
        this.resultImg[2].gameObject.SetActive(false); 
        this.ShowResultIcon();

    }
    public void DiceResultImgInit()
    {
        this.resultImg[0].gameObject.SetActive(false); 
        this.resultImg[1].gameObject.SetActive(false); 
        this.resultImg[2].gameObject.SetActive(true); 
    }
}
