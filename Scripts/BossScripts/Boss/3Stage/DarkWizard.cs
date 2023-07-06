using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkWizard : AbsBossClass 
{
    private enum eState
    {
        None = 0,
        Walk, 
        AttackPattern01, 
        AttackPattern02,
        AttackPattern03,
        Run,
        Die
    }
    private enum eMoveType
    {
        MovePattern1,
        MovePattern2
    }
    private eMoveType moveType;

    [SerializeField]
    private Transform firePoint;
    private Rigidbody2D rb;
    private Collider2D col2D;
    private Animator anim;

    [SerializeField]
    private GameObject NextStagePortal;

    private AttackPattern07 attackPattern07;
    private AttackPattern08 attackPattern08;
    private AttackPattern09 attackPattern09;

    [Header("target")]
    [SerializeField]
    private Transform targetPos;
    [SerializeField]
    private UIBossHealthBar uiBossHealthBar;

    public override void Init(MonsterGenerator monsterGenerator = null, int maxHP = 0, int defenseRate = 0)
    {
        base.Init(monsterGenerator, maxHP, defenseRate);
        this.gameObject.SetActive(true);
        this.rb = this.GetComponent<Rigidbody2D>();
        this.anim = this.GetComponent<Animator>();
        this.col2D = GetComponent<Collider2D>();
        this.attackPattern07 = new AttackPattern07();
        this.attackPattern08 = new AttackPattern08();
        this.attackPattern09 = new AttackPattern09();

        this.rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        this.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        this.rb.bodyType = RigidbodyType2D.Kinematic;
        this.rb.simulated = true;
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.PlayerDie, this.PlayerDie);
    }

    public override void StartBossFight()
    {
        this.playerDamage = InfoManager.instance.statInfo.BattleRate;
        this.maxHP = 1000000 * InfoManager.instance.gameInfo.roundCnt;
        Debug.LogFormat("darkwizard roundCnt :{0}", InfoManager.instance.gameInfo.roundCnt);
        this.hp = this.maxHP;
        this.defenseRate = 40;
        this.bossMoveSpeed = 2.5f;
        this.uiBossHealthBar = FindObjectOfType<UIBossHealthBar>();
        this.uiBossHealthBar.Init();

        this.isTransitioningPhase2 = false;
        this.isTransitioningPhase3 = false;
        this.isDead = false;
        this.col2D.isTrigger = false;
        this.moveType = eMoveType.MovePattern1;

        this.patternCorutine = this.Phase1();
        StartCoroutine(this.patternCorutine);
    }
    public override IEnumerator Phase1()
    {
        Debug.Log("<Color=red>Phase1</Color>");
        while (true)
        {
            this.anim.SetInteger("stat", (int)eState.Walk);
            yield return new WaitForSeconds(3f);
            this.isAttack = true;
            this.AnimationEventTest07();
            yield return new WaitForSeconds(5f);
            this.isAttack = false;
        }
    }
    public override IEnumerator Phase2()
    {
        Debug.Log("<Color=red>Phase2</Color>");
        StartCoroutine(this.Phase2Color(this.phase2Color));
        while (true)
        {
            this.anim.SetInteger("stat", (int)eState.Walk);
            yield return new WaitForSeconds(2f);
            this.isAttack = true;
            this.AnimationEventTest07();
            yield return new WaitForSeconds(4f);
            this.AnimationEventTest08();
            yield return new WaitForSeconds(3f);
            this.isAttack = false;
        }
    }
    public override IEnumerator Phase3()
    {
        Debug.Log("<Color=red>Phase3</Color>");
        this.moveType = eMoveType.MovePattern2;
        StartCoroutine(this.Phase3Color(this.phase3Color));
        while (true)
        {
            this.anim.SetInteger("stat", (int)eState.Run);
            yield return new WaitForSeconds(3f);
            this.isAttack = true;
            this.AnimationEventTest08();
            yield return new WaitForSeconds(4f);
            this.AnimationEventTest09();
            yield return new WaitForSeconds(3f);
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
                    this.rb.velocity = Vector2.zero;
                }
            }
        }

    }
    public override void MovePattern()
    {
        if (this.moveType == eMoveType.MovePattern1)
        {
            this.bossMoveSpeed = 2f;
        }
        else if (this.moveType == eMoveType.MovePattern2)
        {
            this.bossMoveSpeed = 3.5f;
        }

        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        this.rb.velocity = direction * this.bossMoveSpeed;

        if(!this.isDead)
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
    //AttackPattern07(Arrow Bullet)
    private void AnimationEventTest07()
    {
        this.anim.SetInteger("stat", (int)eState.AttackPattern01);
    }
    private void AttackPattern07()
    {
        Debug.Log("Attack1");
        this.attackPattern07.Pattern(this.targetPos, this.firePoint, this.rb, this.transform);
    }
    //AttackPattern08(Many Arrow Bullet)
    private void AnimationEventTest08()
    {
        this.anim.SetInteger("stat", (int)eState.AttackPattern02);
    }
    private void AttackPattern08()
    {
        Debug.Log("Attack1");
        this.attackPattern08.Pattern(this.targetPos, this.firePoint, this.rb, this.transform);
    }
    //AttackPattern09(Big Bullet)
    private void AnimationEventTest09()
    {
        this.anim.SetInteger("stat", (int)eState.AttackPattern03);
    }
    private void AttackPattern09()
    {
        Debug.Log("Attack1");
        this.attackPattern09.Pattern(this.targetPos, this.firePoint, this.rb, this.transform);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var hpPer = (float)this.hp / this.maxHP;
        if (collision.gameObject.CompareTag("Untagged"))
        {
            this.col2D.isTrigger = false;
        }

        if (collision.transform.gameObject.tag == "PlayerBullet")
        {
            this.TakeBulletDamage(collision.gameObject.GetComponent<Bullet>().damage);
            if (hpPer <= 0.7f && !isTransitioningPhase2) //70%
            {
                isTransitioningPhase2 = true; //전환 중임을 표시
                //현재 실행 중인 코루틴 중지
                if (this.patternCorutine != null)
                {
                    StopCoroutine(this.patternCorutine);
                }
                this.patternCorutine = null;

                StartCoroutine(TransitionToPhase2());
            }
            else if (hpPer <= 0.3f && !isTransitioningPhase3) //30%
            {
                isTransitioningPhase3 = true; // 전환 중임을 표시

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
        var BossrommTrans = this.transform.parent.parent.transform;
        for (int i = 0; i < 4; i++)
        {
            BossrommTrans.GetChild(i).gameObject.SetActive(true);
        }
        EventDispatcher.Instance.Dispatch<Transform>(EventDispatcher.EventName.UIPortalArrowControllerInitializingArrows,
          BossrommTrans);
        this.anim.SetBool("isDead", true);
        this.rb.simulated = false;
        yield return new WaitForSeconds(2f);
        this.anim.SetBool("isDead", false);
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
        this.col2D.enabled = false;
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
