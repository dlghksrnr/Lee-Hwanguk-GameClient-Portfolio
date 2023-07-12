using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilWizard : AbsBossClass
{
    private enum eState
    {
        None, 
        Walk, 
        AttackPattern01, 
        AttackPattern04, 
        AttackPattern05, 
        Die 
    }

    [SerializeField]
    private GameObject NextStagePortal;
    [SerializeField]
    private Transform evilWizardFirePoint;

    private Rigidbody2D evilWizardRb;
    private Collider2D evilWizardCol2D;
    private Animator evilWizardAnim;
    private new IEnumerator patternCorutine;

    [Header("Attack Pattern")]
    private AttackPattern04 attackPattern04;
    private AttackPattern05 attackPattern05;
    private AttackPattern06 attackPattern06;

    [SerializeField]
    private Transform targetPos;
    [SerializeField]
    private UIBossHealthBar uiBossHealthBar;

    public override void Init(MonsterGenerator monsterGenerator = null, int maxHP = 0, int defenseRate = 0)
    {
        base.Init(monsterGenerator, maxHP, defenseRate);
        this.evilWizardAnim = this.GetComponent<Animator>();
        this.evilWizardCol2D = GetComponent<BoxCollider2D>();
        this.evilWizardRb=GetComponent<Rigidbody2D>();
        this.attackPattern04 = new AttackPattern04();
        this.attackPattern05 = new AttackPattern05();
        this.attackPattern06 = new AttackPattern06();
    }
    public override void StartBossFight()
    {
        this.playerDamage = InfoManager.instance.statInfo.BattleRate;
        this.maxHP = 500000 * InfoManager.instance.gameInfo.roundCnt;
        Debug.LogFormat("evilwizard roundCnt :{0}", InfoManager.instance.gameInfo.roundCnt);
        this.hp = this.maxHP;
        this.defenseRate = 30;
        this.bossMoveSpeed = 2f;
        this.uiBossHealthBar = FindObjectOfType<UIBossHealthBar>();
        this.uiBossHealthBar.Init();

        this.isTransitioningPhase2 = false;
        this.isTransitioningPhase3 = false;
        this.isDead = false;
        this.evilWizardCol2D.isTrigger = false;
        this.patternCorutine = this.Phase1();
        StartCoroutine(this.patternCorutine);
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.PlayerDie, this.PlayerDie);

    }
    //Phase
    public override IEnumerator Phase1()
    {
        Debug.Log("<Color=red>EvilWizardPhase1</Color>");
        while (true)
        {
            this.evilWizardAnim.SetInteger("stat", (int)eState.Walk);
            yield return new WaitForSeconds(3f);
            this.isAttack = true;
            this.AnimationEventTest01();
            yield return new WaitForSeconds(5f);
            this.isAttack = false;
        }
    }
    public override IEnumerator Phase2()
    {
        Debug.Log("<Color=red>EvilWizardPhase2</Color>");
        StartCoroutine(this.Phase2Color(this.phase2Color));
        while (true)
        {
            this.evilWizardAnim.SetInteger("stat", (int)eState.Walk);
            yield return new WaitForSeconds(2f);
            this.isAttack = true;
            this.AnimationEventTest01();
            yield return new WaitForSeconds(4f);
            this.AnimationEventTest04();
            yield return new WaitForSeconds(3f);
            this.isAttack = false;
        }
    }
    public override IEnumerator Phase3()
    {
        Debug.Log("<Color=red>EvilWiazrdPhase3</Color>");
        StartCoroutine(Phase3Color(this.phase3Color));
        while (true)
        {
            this.evilWizardAnim.SetInteger("stat", (int)eState.Walk);
            yield return new WaitForSeconds(3f);
            this.isAttack = true;
            this.AnimationEventTest04();
            yield return new WaitForSeconds(4f);
            this.AnimationEventTest05();
            yield return new WaitForSeconds(4f);
            this.isAttack = false;
        }
    }
    
    //Move, Set Target
    private void FixedUpdate()
    {
        if (this.player != null)
        {
            this.direction = (this.player.position - this.transform.position).normalized;

            //Raycast를 통해 플레이어 감지
            RaycastHit2D hit = Physics2D.CircleCast(this.transform.position, 32f, Vector2.zero, 0f, LayerMask.GetMask("Player"));
            if (hit.collider != null)
            {
                // 플레이어를 발견한 경우 타겟을 향해 움직임
                this.targetPos = hit.collider.transform;
                if (!this.isAttack)
                {
                    this.MovePattern();
                }
                else
                {
                    this.evilWizardRb.velocity = Vector2.zero;
                }
            }
        }
    }
    public override void MovePattern()
    {
        if (this.direction.magnitude > 1f)
        {
            this.direction.Normalize();
        }

        this.evilWizardRb.velocity = this.direction * this.bossMoveSpeed;

        if (!this.isDead)
        {
            if (this.targetPos.position.x < this.transform.position.x)
            {
                this.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
            }
            else
            {
                this.transform.rotation = Quaternion.identity;
            }
        }

    }


    //AttackPattern01(Big Bullet)
    private void AnimationEventTest01()
    {
        this.evilWizardAnim.SetInteger("stat", (int)eState.AttackPattern01);
    }
    private void AttackPattern01()
    {
        Debug.Log("Attack1");
        this.attackPattern04.Pattern(this.targetPos, this.evilWizardFirePoint, this.evilWizardRb, this.transform);
    }
    //AttackPattern04(3 Way Bullet)
    private void AnimationEventTest04()
    {
        this.evilWizardAnim.SetInteger("stat", (int)eState.AttackPattern04);
    }
    private void AttackPattern04()
    {
        Debug.Log("Attack4");
        this.attackPattern05.Pattern(this.targetPos,this.evilWizardFirePoint, this.evilWizardRb, this.transform);
    }
    //AttackPattern05(Meteor)
    private void AnimationEventTest05()
    {
        this.evilWizardAnim.SetInteger("stat", (int)eState.AttackPattern05);
    }
    private void AttackPattern05()
    {
        Debug.Log("Attack5");
        this.attackPattern06.Pattern(this.targetPos, this.evilWizardFirePoint, this.evilWizardRb, this.transform);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var hpPer=(float)this.hp/this.maxHP;

        if (collision.gameObject.CompareTag("Untagged"))
        {
            this.evilWizardCol2D.isTrigger = false;
        }

        if (collision.transform.gameObject.tag == "PlayerBullet")
        {
            this.TakeBulletDamage(collision.gameObject.GetComponent<Bullet>().damage);

            //Debug.LogFormat("<color=white>현재 보스 체력 :{0}</color>", this.hp);
            if (hpPer <= 0.7f && !this.isTransitioningPhase2) //70%
            {
                this.isTransitioningPhase2 = true; //전환 중임을 표시
                //현재 실행 중인 코루틴 중지
                if (this.patternCorutine != null)
                {
                    StopCoroutine(this.patternCorutine);
                }
                this.patternCorutine = null;

                StartCoroutine(this.TransitionToPhase2());
            }
            else if (hpPer <= 0.3f && !this.isTransitioningPhase3) //30%
            {
                this.isTransitioningPhase3 = true; // 전환 중임을 표시

                // 현재 실행 중인 코루틴 중지
                if (this.patternCorutine != null)
                {
                    StopCoroutine(this.patternCorutine);
                }
                this.patternCorutine = null;

                StartCoroutine(TransitionToPhase3());
            }
            else if (this.hp <= 0)
            {
                this.isDead = true;
                if (this.isDead)
                {
                    StopAllCoroutines();
                    StartCoroutine(this.BossDie());
                }
            }
        }
    }
    public override IEnumerator BossDie()
    {
        this.isDead = true;
        this.evilWizardAnim.SetBool("isDead", true);
        this.evilWizardRb.simulated = false;
        yield return new WaitForSeconds(1.6f);
        this.evilWizardAnim.SetBool("isDead", false);
        AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.DUNGEONBG);
        EventDispatcher.Instance.Dispatch<UIAnnounceDirector.eAnnounceType>(EventDispatcher.EventName.UIAnnounceDirectorStartAnnounce,
            UIAnnounceDirector.eAnnounceType.STAGE);
        EventDispatcher.Instance.Dispatch<Transform, bool>(EventDispatcher.EventName.ChestItemGeneratorMakeChest,
            this.transform.parent.parent, true);
        yield return new WaitForSeconds(2f);
        Debug.Log("Die");
        this.uiBossHealthBar.transform.GetChild(0).gameObject.SetActive(false);

        this.NextStagePortal.SetActive(true);
        this.patternCorutine = null;
        this.evilWizardCol2D.enabled = false;
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// PlayerDie, BossMoveSpeed=0
    /// </summary>
    public void PlayerDie()
    {
        this.bossMoveSpeed = 0;
        Debug.LogFormat("<color=red>PlayerDie GameOver, BossMoveSpeed:{0}</color>", this.bossMoveSpeed);
    }

}
