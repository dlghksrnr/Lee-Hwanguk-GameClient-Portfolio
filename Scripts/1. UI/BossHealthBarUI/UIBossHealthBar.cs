using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHealthBar : MonoBehaviour
{
    public enum eHpType
    {
        BOSS,
        DEMON
    }
    private eHpType hpType;
    [SerializeField]
    private GameObject boss;
    public int bossMaxHp;
    public int bossHp;
    [SerializeField]
    private Image fill;
    private float fillHpBar;
    private Damageable bossDamageable;
    private Demon demon;
    private int demonMaxHp;
    private int demonHp;
    private bool isFilling;
    private float fillSpeed = 0.7f;
    
    private void Start()
    {
        this.fill.transform.parent.gameObject.SetActive(false);
        this.fill.enabled = false;
    }
    
    public void Init()
    {
        this.hpType = eHpType.BOSS;
        this.fill.transform.parent.gameObject.SetActive(true);
        this.fill.enabled = true;
        this.fillHpBar = this.fill.GetComponent<Image>().fillAmount;
        this.boss = GameObject.FindWithTag("Monster");

        this.bossDamageable = this.boss.GetComponent<Damageable>();
        if (bossDamageable != null)
        {
            this.bossMaxHp = bossDamageable.maxHP;
            this.bossHp = bossDamageable.hp;

            Debug.LogFormat("<color=red>bossName:{0}, maxHp:{1}, hp:{2}</color>",
            this.boss.name, bossMaxHp, bossHp);
        }

        fill.fillAmount = 0f;
        isFilling = true;
        StartCoroutine(FillCoroutine(bossMaxHp));
    }

    public void DemonInit()
    {
        this.hpType = eHpType.DEMON;
        this.fillHpBar = this.fill.GetComponent<Image>().fillAmount;
        this.demon = GameObject.FindObjectOfType<Demon>();
        if (this.demon != null)
        {
            this.demonMaxHp = demon.maxHP;
            this.demonHp = demon.hp;

            Debug.LogFormat("<color=blue>bossName:{0}, maxHp:{1}, hp:{2}</color>",
            this.demon.name, demonMaxHp, demonHp);
        }

        fill.fillAmount = 0f;
        isFilling = true;
        StartCoroutine(FillCoroutine(demonMaxHp));
    }

    private IEnumerator FillCoroutine(int maxHp)
    {
        float timer = 0f;

        while (timer <= fillSpeed)
        {
            float fillAmount = Mathf.Lerp(0f, 1f, timer / fillSpeed);
            fill.fillAmount = fillAmount;

            timer += Time.deltaTime;
            yield return null;
        }
        isFilling = false;
    }
    void FixedUpdate()
    {
        if (this.boss != null && this.hpType == eHpType.BOSS)
        {
            if (this.bossDamageable != null)
            {
                this.bossHp = this.bossDamageable.hp;
                //this.fill.fillAmount = (float)this.bossHp / this.bossMaxHp;
                this.fill.fillAmount = Mathf.Lerp(this.fill.fillAmount,
                    (float)this.bossHp / this.bossMaxHp, Time.deltaTime * 5f);
            }
        }
        if (this.demon != null && this.hpType == eHpType.DEMON)
        {
            this.demonHp = this.demon.hp;
            //this.fill.fillAmount=(float)this.demonHp/ this.demonMaxHp;
            this.fill.fillAmount = Mathf.Lerp(this.fill.fillAmount,
                (float)this.demonHp / this.demonMaxHp, Time.deltaTime * 5f);
        }
    }

}
