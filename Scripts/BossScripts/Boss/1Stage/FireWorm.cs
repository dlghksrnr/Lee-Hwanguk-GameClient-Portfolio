using System.Collections;
using UnityEngine;

public class FireWorm : AbsBossClass
{
    private enum eState
    {
        None = 0,
        Walk, 
        AttackPattern01, 
        AttackPattern02,
        AttackPattern03,
        Die
    }
    private enum eMoveType
    {
        MovePattern1,
        MovePattern2,
        MovePattern3
    }
    private eMoveType moveType;

    [SerializeField]
    protected GameObject NextStagePortal;
    public Transform targetPos;
    public Rigidbody2D rb;
    [SerializeField]
    protected Transform firePoint;
    public Animator anim;
    public Collider2D col2D;
    private AttackPattern01 attackPattern01;
    private AttackPattern02 attackPattern02;
    private AttackPattern03 attackPattern03;

    private UIBossHealthBar uiBossHealthBar;

    public override void Init(MonsterGenerator monsterGenerator = null, int maxHP = 0, int defenseRate = 0)
    { 
        base.Init(monsterGenerator, maxHP, defenseRate);
        this.gameObject.SetActive(true);
        this.rb = this.GetComponent<Rigidbody2D>();
        this.anim = this.GetComponent<Animator>();
        this.col2D = GetComponent<Collider2D>();

        this.attackPattern01 = new AttackPattern01();
        this.attackPattern02 = new AttackPattern02();
        this.attackPattern03 = new AttackPattern03();
        this.rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        this.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        this.rb.bodyType = RigidbodyType2D.Kinematic;
        this.rb.simulated = true;
    }

    public override void StartBossFight()
    {
        this.playerDamage = InfoManager.instance.statInfo.BattleRate;
        this.maxHP = 100000*InfoManager.instance.gameInfo.roundCnt;
        this.hp = this.maxHP;
        this.defenseRate = 20;
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
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.PlayerDie, this.PlayerDie);
    }

    public override IEnumerator Phase1()
    {
        while (true)
        {
            this.moveType = eMoveType.MovePattern1;
            this.anim.SetInteger("stat", (int)eState.Walk);
            yield return new WaitForSeconds(3f);
            this.isAttack = true;
            this.AnimationEventTest01();
            yield return new WaitForSeconds(5f);
            this.isAttack = false;
        }
    }
    public override IEnumerator Phase2()
    {
        StartCoroutine(Phase2Color(this.phase2Color));
        this.moveType = eMoveType.MovePattern2;
        while (true)
        {
            this.anim.SetInteger("stat", (int)eState.Walk);
            yield return new WaitForSeconds(3f);
            this.isAttack = true;
            this.AnimationEventTest02();
            yield return new WaitForSeconds(4f);
            this.AnimationEventTest03();
            yield return new WaitForSeconds(2f);
            this.isAttack = false;
        }
    }
    public override IEnumerator Phase3()
    {
        StartCoroutine(this.Phase3Color(this.phase3Color));
        this.moveType = eMoveType.MovePattern3;
        while (true)
        {
            this.anim.SetInteger("stat", (int)eState.Walk);
            yield return new WaitForSeconds(3f);
            this.isAttack = true;
            this.AnimationEventTest02();
            yield return new WaitForSeconds(4f);
            this.AnimationEventTest03();
            yield return new WaitForSeconds(2f);
            this.isAttack = false;
        }
    }

    private void FixedUpdate()
    {
        if (this.player != null)
        {
            this.direction = (this.player.position - this.transform.position).normalized;
            RaycastHit2D hit = Physics2D.CircleCast(this.transform.position, 32f, Vector2.zero, 0f, LayerMask.GetMask("Player"));
            if (hit.collider != null)
            {
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
            this.bossMoveSpeed = 2.5f;
        }
        else if (this.moveType == eMoveType.MovePattern2)
        {
            this.bossMoveSpeed = 3.0f;
        }
        else if (this.moveType == eMoveType.MovePattern3)
        {
            this.bossMoveSpeed = 4f;
        }

        if (this.direction.magnitude > 1f)
        {
            this.direction.Normalize();
        }

        this.rb.velocity = this.direction * this.bossMoveSpeed;

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

    private void AnimationEventTest01()
    {
        this.anim.SetInteger("stat", (int)eState.AttackPattern01);
    }
    private void AttackPattern01()
    {
        this.attackPattern01.Pattern(this.targetPos, this.firePoint, this.rb, this.transform);
    }

    private void AnimationEventTest02()
    {
        this.anim.SetInteger("stat", (int)eState.AttackPattern02);
    }
    private void AttackPattern02()
    {
        this.attackPattern02.Pattern(this.player, this.firePoint, this.rb, this.transform);
    }

    private void AnimationEventTest03()
    {
        this.anim.SetInteger("stat", (int)eState.AttackPattern03);
    }
    private void AttackPattern03()
    {
        this.attackPattern03.Pattern(this.player, this.firePoint, this.rb, this.transform);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            this.col2D.isTrigger = false;
        }

        if (collision.transform.gameObject.tag == "PlayerBullet")
        {
            this.TakeBulletDamage(collision.gameObject.GetComponent<Bullet>().damage);
             var hpPer=(float)this.hp/this.maxHP;
            if (hpPer <= 0.7f && !this.isTransitioningPhase2)
            {
                this.isTransitioningPhase2 = true;

                if (this.patternCorutine != null)
                {
                    StopCoroutine(this.patternCorutine);
                }
                this.patternCorutine = null;

                StartCoroutine(TransitionToPhase2());
            }
            else if (hpPer <= 0.3f && !this.isTransitioningPhase3)
            {
                this.isTransitioningPhase3 = true; 

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
        AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.DUNGEONBG);
        this.anim.SetBool("isDead", true);
        this.rb.simulated = false;
        yield return new WaitForSeconds(2f);
        this.anim.SetBool("isDead", false);
        EventDispatcher.Instance.Dispatch<UIAnnounceDirector.eAnnounceType>(EventDispatcher.EventName.UIAnnounceDirectorStartAnnounce,
           UIAnnounceDirector.eAnnounceType.STAGE);
        EventDispatcher.Instance.Dispatch<Transform, bool>(EventDispatcher.EventName.ChestItemGeneratorMakeChest,
            this.transform.parent.parent, true);
        yield return new WaitForSeconds(3f);

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
    }
}
