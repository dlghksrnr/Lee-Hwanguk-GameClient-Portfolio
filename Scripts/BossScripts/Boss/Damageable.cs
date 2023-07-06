using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [System.NonSerialized]
    public int exp = 18;
    [System.NonSerialized]
    public int maxHP;
    [System.NonSerialized]
    public int hp;
    [System.NonSerialized]
    public int defenseRate;
    [System.NonSerialized]
    public bool isSkilldDamaged;
    [SerializeField]
    protected GameObject damageTxtGO;
    [System.NonSerialized]
    public bool isDead;
    [System.NonSerialized]
    public bool isInvincibility;

    //독데미지
    protected Coroutine PoisonRoutine;
    protected int getPoisonCnt;
    protected int maxPoisonCnt;
    protected PoisonBullet poisonBullet;

    protected MonsterBulletPooler objectPooler;

    public virtual void Init(MonsterGenerator monsterGenerator = null, int maxHP = 0, int defenseRate = 0)
    {
        this.damageTxtGO = Resources.Load<GameObject>("Prefabs/Monsters/DamgePopup");
        this.poisonBullet = GameObject.FindObjectOfType<PoisonBullet>()?.GetComponent<PoisonBullet>();
        this.maxPoisonCnt = 7;
        this.objectPooler = MonsterBulletPooler.instance;
        this.isSkilldDamaged = false;
        this.isInvincibility = false;
    }

    public void TakeSkillDamage(int damage, float skillTime)
    {
        if (!this.isSkilldDamaged) this.TakeDamage(damage);
        if (!this.isDead && this.gameObject != null) StartCoroutine(SkillDamagedRoutine(skillTime));
    }

    protected IEnumerator SkillDamagedRoutine(float skillTime)
    {
        this.isSkilldDamaged = true;
        yield return new WaitForSeconds(skillTime);
        this.isSkilldDamaged = false;
    }

    public void TakeBulletDamage(int damage)
    {
        this.TakeDamage(damage);
        if (this.poisonBullet != null)
        {
            if (this.poisonBullet.IsPoison() && this.gameObject != null)
            {
                if (this.PoisonRoutine != null) StopCoroutine(this.PoisonRoutine);
                if (this.gameObject != null) this.PoisonRoutine = StartCoroutine(this.GetPoisonDamageRoutine());
            }
        }
    }

    protected virtual IEnumerator GetPoisonDamageRoutine()
    {
        while (true)
        {
            if (this.maxPoisonCnt <= this.getPoisonCnt || this.isDead) break;
            if (this.poisonBullet != null) this.poisonBullet.ShowVfxPoison(this.gameObject.transform);
            this.TakeDamage((InfoManager.instance.statInfo.criticalHitAmount + InfoManager.instance.statInfo.criticalHitChance) * 3);
            yield return new WaitForSeconds(1.2f);
            this.getPoisonCnt++;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        //데미지 크리티컬 적용
        double criticalHitChance = (double)InfoManager.instance.statInfo.criticalHitChance /
            ((double)InfoManager.instance.statInfo.criticalHitChance + 70d) * 88d;
        double criticalHitAmount = (double)InfoManager.instance.statInfo.criticalHitAmount /
            ((double)InfoManager.instance.statInfo.criticalHitAmount + 100d) * 15d;
        bool isCritial = false;

        damage = Mathf.RoundToInt(damage);
        damage -= damage * (this.defenseRate / 100);

        if (Random.value < criticalHitChance / 100)
        {
            damage = Mathf.RoundToInt((float)damage * (float)criticalHitAmount);
            isCritial = true;
        }

        GameObject go;
        if (this.objectPooler.damagePopupPool.Count > 0)
        {
            go = this.objectPooler.damagePopupPool.Dequeue();
            go.SetActive(true);
        }
        else
        {
            go = Instantiate(this.damageTxtGO);
            go.GetComponentInChildren<MeshRenderer>().sortingLayerName = "FlyingObject";
            go.GetComponentInChildren<MeshRenderer>().sortingOrder = 100;
        }
        go.GetComponent<DamagePopup>().Init(this.gameObject.transform.position, damage, isCritial);

        if(!this.isInvincibility) this.hp -= damage;
    }

    //대시 데미지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDash"))
        {
            var dashAttack = collision.transform.parent.gameObject.GetComponent<DashAttack>();
            this.TakeBulletDamage(dashAttack.damage);
        }
    }
}
