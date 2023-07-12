using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : AbsBossClass 
{
    public enum eState
    {
        None = 0,
        Walk, 
        AttackPattern01, 
        Die
    }
    [Header("Stat")]

    [SerializeField]
    private Transform firePoint;
    private Rigidbody2D rb;
    private Collider2D col2D;
    private Animator anim;


    private AttackPattern01 attackPattern01;

    [Header("target")]
    [SerializeField]
    private Transform targetPos;
    [SerializeField]
    private UIBossHealthBar uiBossHealthBar;
    public GameObject demon;

    public override void Init(MonsterGenerator monsterGenerator = null, int maxHP = 0, int defenseRate = 0)
    {
        base.Init(monsterGenerator, maxHP, defenseRate);
        this.gameObject.SetActive(true);

        this.rb = this.GetComponent<Rigidbody2D>();
        this.anim = this.GetComponent<Animator>();
        this.col2D = GetComponent<Collider2D>();
        this.attackPattern01 = new AttackPattern01();
    }
    public override void StartBossFight()
    {
        this.playerDamage = InfoManager.instance.statInfo.BattleRate;
        this.maxHP = 500000 * InfoManager.instance.gameInfo.roundCnt;
        Debug.LogFormat("slime roundCnt :{0}", InfoManager.instance.gameInfo.roundCnt);
        this.hp = this.maxHP;
        this.defenseRate = 40;
        this.bossMoveSpeed = 3f;
        this.uiBossHealthBar = FindObjectOfType<UIBossHealthBar>();
        this.uiBossHealthBar.Init();
        this.isDead = false;
        this.col2D.isTrigger = false;
        this.patternCorutine = this.Phase1();
        StartCoroutine(this.patternCorutine);
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.PlayerDie, this.PlayerDie);

    }
    public override IEnumerator Phase1()
    {
        Debug.Log("<Color=red>Phase1</Color>");
        while (true)
        {
            this.anim.SetInteger("stat", (int)eState.Walk);
            yield return new WaitForSeconds(Random.Range(1, 5));
            this.isAttack = true;
            this.AnimationEventTest01();
            yield return new WaitForSeconds(Random.Range(1, 5));
            this.isAttack = false;
        }
    }
    public override IEnumerator Phase2()
    {
        yield break;
    }
    public override IEnumerator Phase3()
    {
        yield break;
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

        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        this.rb.velocity = direction * this.bossMoveSpeed;

        if(!this.isDead)
        {
            if (this.targetPos.position.x < this.transform.position.x)
            {
                this.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                this.transform.rotation = Quaternion.identity;
            }
        }

    }
    //AttackPattern07(Normal Bullet)
    private void AnimationEventTest01()
    {
        this.anim.SetInteger("stat", (int)eState.AttackPattern01);
    }
    private void AttackPattern01()
    {
        Debug.Log("Attack1");
        this.attackPattern01.Pattern(this.targetPos, this.firePoint, this.rb, this.transform);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            this.col2D.isTrigger = false;
        }

        if (collision.transform.gameObject.tag == "PlayerBullet")
        {
            this.TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
            
            if (this.hp <= 0)
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
        this.isAttack = true;
        this.anim.SetInteger("stat", (int)eState.Die);
        this.rb.simulated = false;
        yield return new WaitForSeconds(3.2f);
        Debug.Log("Die");

        this.patternCorutine = null;
        this.col2D.enabled = false;

        this.demon.SetActive(true);
        this.demon.transform.position = this.transform.position;
        this.demon.transform.rotation = this.transform.rotation;
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
