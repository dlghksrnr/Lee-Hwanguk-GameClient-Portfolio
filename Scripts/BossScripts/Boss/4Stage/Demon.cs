using System.Collections;
using UnityEngine;

public class Demon : AbsBossClass
{
    private enum eState
    {
        NONE = 0,
        WALK, 
        ATTACKPATTERN11, 
        ATTACKPATTERN12,
        ATTACKPATTERN13,
        ATTACKPATTERN14,
        RUN,
        DIE
    }
    private enum eMoveType
    {
        MOVEPATTERN1,
        MOVEPATTERN2, 
        MOVEPATTERN3 
    }
    private eMoveType moveType;
    [Header("Stat")]


    private Rigidbody2D rb;
    private Collider2D col2D;
    private Animator anim;


    [SerializeField]
    private Transform parabolaFirePoint;
    [SerializeField]
    private Transform laserFirePoint;
    [SerializeField]
    private GameObject combatAttackCol;
    [SerializeField]
    private GameObject FrameAttackCol;
    [SerializeField]
    private GameObject splashAttackCol;
    [SerializeField]
    private GameObject NextStagePortal;
 
    private AttackPattern02 attackPattern2; 
    private AttackPattern14 attackPattern14; 


    [Header("target")]
    [SerializeField]
    private Transform targetPos;
    [SerializeField]
    private UIBossHealthBar uiBossHealthBar;

    void Start()
    {

        this.player = FindObjectOfType<PlayerShell>().player.transform;
        Debug.LogFormat("targetOn:{0}", this.player.transform.position);

        this.rb = this.GetComponent<Rigidbody2D>();
        this.anim = this.GetComponent<Animator>();
        this.col2D = GetComponent<Collider2D>();
        this.attackPattern2 = new AttackPattern02();
        this.attackPattern14 = new AttackPattern14();

        this.rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        this.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        this.rb.bodyType = RigidbodyType2D.Kinematic;
        this.rb.simulated = true;

        this.playerDamage = InfoManager.instance.statInfo.BattleRate;
        this.bossMoveSpeed = 2f;
        this.combatAttackCol.gameObject.SetActive(false);
        this.FrameAttackCol.gameObject.SetActive(false);
        this.isTransitioningPhase2 = false;
        this.isTransitioningPhase3 = false;
        this.isDead = false;
        this.col2D.isTrigger = false;
        this.moveType = eMoveType.MOVEPATTERN1;
        this.Init();

        this.patternCorutine = this.Phase1();
        StartCoroutine(this.patternCorutine);

        this.maxHP = 2500000 * InfoManager.instance.gameInfo.roundCnt;
        Debug.LogFormat("demon roundCnt :{0}", InfoManager.instance.gameInfo.roundCnt);
        this.hp = this.maxHP;
        this.defenseRate = 50;
        this.uiBossHealthBar = FindObjectOfType<UIBossHealthBar>();
        this.uiBossHealthBar.DemonInit();
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.PlayerDie, this.PlayerDie);

    }

    public override void Init(MonsterGenerator monsterGenerator = null, int maxHP = 0, int defenseRate = 0)
    {
        base.Init(monsterGenerator, maxHP, defenseRate);
    }

    public override IEnumerator Phase1()
    {
        Debug.Log("<Color=red>Phase1</Color>");
        this.moveType = eMoveType.MOVEPATTERN1;
        while (true)
        {
            this.anim.SetInteger("stat", (int)eState.WALK);
            yield return new WaitForSeconds(Random.Range(2, 5));
            this.isAttack = true;
            this.AnimationEventTest11();
            yield return new WaitForSeconds(1.7f);
            this.anim.SetInteger("stat", (int)eState.WALK);
            this.combatAttackCol.gameObject.SetActive(false);
            this.isAttack = false;
            yield return new WaitForSeconds(Random.Range(2, 5));
            this.isAttack = true;
            this.AnimationEventTest12();
            yield return new WaitForSeconds(2.1f);
            this.isAttack = false;
        }
    }
    public override IEnumerator Phase2()
    {
        Debug.Log("<Color=red>Phase2</Color>");
        this.moveType = eMoveType.MOVEPATTERN2;
        while (true)
        {
            this.anim.SetInteger("stat", (int)eState.WALK);
            yield return new WaitForSeconds(Random.Range(2, 5));
            this.AnimationEventTest13();
            yield return new WaitForSeconds(1.769f);
            this.anim.SetInteger("stat", (int)eState.WALK);
            this.isAttack = false;
            yield return new WaitForSeconds(0.1f);
            this.splashAttackCol.SetActive(false);

            yield return new WaitForSeconds(Random.Range(2, 5));
            this.isAttack = true;
            this.AnimationEventTest11();
            yield return new WaitForSeconds(1.7f);
            this.anim.SetInteger("stat", (int)eState.WALK);
            this.combatAttackCol.gameObject.SetActive(false);
            this.isAttack = false;
        }
    }

    public override IEnumerator Phase3()
    {
        Debug.Log("<Color=red>Phase3</Color>");
        this.moveType = eMoveType.MOVEPATTERN3;
        while (true)
        {
            this.anim.SetInteger("stat", (int)eState.RUN);
            yield return new WaitForSeconds(Random.Range(2, 5));
            this.isAttack = true;
            this.AnimationEventTest12();
            yield return new WaitForSeconds(2.1f);
            this.isAttack = false;
            this.anim.SetInteger("stat", (int)eState.RUN);

            yield return new WaitForSeconds(Random.Range(2, 5));
            this.isAttack = true;
            this.AnimationEventTest14();
            yield return new WaitForSeconds(5f);
            this.anim.SetInteger("stat", (int)eState.RUN);
            this.combatAttackCol.gameObject.SetActive(false);
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
        if (this.moveType == eMoveType.MOVEPATTERN1)
        {
            this.bossMoveSpeed = 2f;
        }
        else if (this.moveType == eMoveType.MOVEPATTERN2)
        {
            this.bossMoveSpeed = 4f;
        }

        if (this.direction.magnitude > 1f)
        {
            this.direction.Normalize();
        }

        this.rb.velocity = this.direction * this.bossMoveSpeed;

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
    //AttackPattern11(플레이어 방향으로 대쉬 + 근접 공격)
    private void AnimationEventTest11()
    {
        this.anim.SetInteger("stat", (int)eState.ATTACKPATTERN11);
    }
    //private void AttackPattern11()
    //{
    //    Debug.Log("AttackPattern 11");
    //}

    //AttackPattern12(근접 공격)
    private void AnimationEventTest12()
    {
        Debug.Log("AttackPattern 12");
        this.anim.SetInteger("stat", (int)eState.ATTACKPATTERN12);
    }
    //private void AttackPattern12()
    //{
    //}

    //AttackPattern13(도약 + 포물선 bullet)
    private void AnimationEventTest13()
    {
        this.anim.SetInteger("stat", (int)eState.ATTACKPATTERN13);
    }
    private void AttackPattern13()
    {
        this.isAttack = true;
        this.splashAttackCol.SetActive(true);
        this.attackPattern2.Pattern(this.targetPos, this.parabolaFirePoint, this.rb, this.transform);

    }

    //AttackPattern14(Laser)
    private void AnimationEventTest14()
    {
        this.anim.SetInteger("stat", (int)eState.ATTACKPATTERN14);
    }
    private void AttackPattern14()
    {
        this.attackPattern14.Pattern(this.targetPos, this.laserFirePoint, this.rb, this.transform);
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
    //Die Coruotine
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
        yield return new WaitForSeconds(3f);
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
