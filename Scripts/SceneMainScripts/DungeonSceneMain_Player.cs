using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DungeonSceneMain : MonoBehaviour
{
    [Header("HP")]
    private bool isHitting;
    private float damagedTime;
    [System.NonSerialized]
    public int playerHP = 3;
    [System.NonSerialized]
    public int playerMaxHP = 3;
    [System.NonSerialized]
    public int playerExtraHP = 0;

    private int maxEXP = 1000;
    private bool isMaxLevel;

    public bool powerOverWhelming;

    private void SetSkillCoolTime()
    {
        var joystickDirector = this.director.joystickDirector;
        var playerShell = this.player.GetComponent<PlayerShell>();
        for (int i = 0; i < joystickDirector.joystickSkillDir.Length; i++)
        {
            int idx = i;

            joystickDirector.joystickSkillDir[idx].shootSkill += () =>
            {
                joystickDirector.UICoolTimeSkills[idx].GetComponent<UICoolTime>().
                Init(playerShell.gunShell.currentGun.gunForm.skillCoolTime[idx]);
            };

            joystickDirector.btnSkills[idx].onClick.AddListener(() =>
            {
                joystickDirector.UICoolTimeSkills[idx].GetComponent<UICoolTime>().
                Init(playerShell.gunShell.currentGun.gunForm.skillCoolTime[idx]);
            });
        }

        joystickDirector.btnDash.onClick.AddListener(() =>
        {
            float dashCoolTime = 2.1f - InfoManager.instance.charactoristicInfo.dashRecoverCharacteristic * 0.2f;
            joystickDirector.UICoolTimeDash.GetComponent<UICoolTime>().Init(dashCoolTime);
        });
    }

    public void OnPlayerExpUp(int monsterExp)
    {
        var gunCharacteristics = InfoManager.instance.charactoristicInfo;
        if (this.playerHP > 0)
        {
            if (gunCharacteristics.gunProficiencyLevel < 30)
            {
                gunCharacteristics.gunProficiencyEXP += monsterExp;
                this.director.expSliderDirector.BlinkEXPFill();

                if (gunCharacteristics.gunProficiencyEXP >= this.maxEXP)
                {
                    AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.Skill2_AssultRifle, sfxSource);
                    gunCharacteristics.gunProficiencyEXP = 0;
                    gunCharacteristics.gunProficiencyLevel++;
                    Instantiate(this.player.GetComponent<PlayerShell>().vfxLevelUp, this.player.transform);
                    Invoke("ShowUIGunCharacteristicChoice", 0.45f);
                    this.player.GetComponent<PlayerShell>().gunShell.SetGun();
                    if (gunCharacteristics.gunProficiencyLevel == 10)
                        this.director.joystickDirector.UICoolTimeSkills[1].gameObject.SetActive(false);
                    if (gunCharacteristics.gunProficiencyLevel == 20)
                        this.director.joystickDirector.UICoolTimeSkills[2].gameObject.SetActive(false);
                }
            }
            else 
            {
                gunCharacteristics.gunProficiencyEXP = maxEXP;

            } 

            //레벨 30 달성 능력치 버프
            if(gunCharacteristics.gunProficiencyLevel == 30)
            {
                if (!this.isMaxLevel)
                {
                    this.AchieveMaxLevel(this.player.GetComponent<PlayerShell>().gunShell.currentGun.gunForm.gunType);
                }
                this.isMaxLevel = true;
            }

            this.director.expSliderDirector.sliderExp.value = gunCharacteristics.gunProficiencyEXP;
            this.director.expSliderDirector.txtExp.text = string.Format("LV {0}", gunCharacteristics.gunProficiencyLevel);
        }
    }

    private void ShowUIGunCharacteristicChoice()
    {
        if (this.playerHP > 0) this.director.gunCharacteristicChoice.Show();
        else this.director.gunCharacteristicChoice.UnShow();
    }

    void FixedUpdate()
    {
        if (this.player != null)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(this.player.transform.position + new Vector3(0, -0.2f, 0), new Vector2(0.7f, 1.0f), 0);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Monster") || collider.CompareTag("MonsterBullet") || collider.CompareTag("MonsterMeleeAttack"))
                {
                    if (!this.isHitting && this.playerHP > 0 && !this.player.GetComponent<PlayerShell>().isDash)
                    {
                        if (!powerOverWhelming) StartCoroutine(this.PlayerDamagedRoutine(1));

                        if (collider.CompareTag("MonsterBullet"))
                        {
                            collider.gameObject.SetActive(false);
                        }
                    }
                }
            }

            if (Input.GetKeyDown("p"))
            {
                if (!this.powerOverWhelming)
                {
                    this.powerOverWhelming = true;
                    Debug.Log("무적상태");
                }
                else
                {
                    this.powerOverWhelming = false;
                    Debug.Log("무적해제");
                }
            }
        }
    }

    public bool TakeChestDamage(int damageAmount)
    {
        bool playerCanTake = true;
        if (this.playerHP + this.playerExtraHP - damageAmount <= 0) playerCanTake = false;
        else StartCoroutine(this.PlayerDamagedRoutine(damageAmount));

        return playerCanTake;
    }

    public IEnumerator PlayerDamagedRoutine(int damageAmount)
    {
        var playerShell = this.player.GetComponent<PlayerShell>();

        this.DamagePlayer(damageAmount, playerShell, true);

        if (this.playerHP > 0) StartCoroutine(this.playerBlinkRoutine(playerShell.playerSprite));
        yield return new WaitForSeconds(this.damagedTime);
        playerShell.playerSprite.color = new Color(255, 255, 255);
        this.isHitting = false;
    }

    private void DamagePlayer(int damageAmount, PlayerShell playerShell, bool isRegularHP)
    {
        var dice = 0.5f > Random.value;
        Debug.Log(dice);
        if (dice) AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.PlayerDamaged1, this.sfxSource);
        else AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.PlayerDamaged2, this.sfxSource);

        AudioManager.instance.Vibrate();

        this.director.healthDirector.BlinkHelathBar();
        this.isHitting = true;
        playerShell.playerSprite.color = new Color(128, 128, 128);

        if (this.playerExtraHP <= 0)
        {
            this.playerHP -= damageAmount;
        }
        else if (this.playerExtraHP > 0)
        {
            this.playerExtraHP -= damageAmount;
            if (this.playerExtraHP < 0)
            {
                this.playerHP += this.playerExtraHP;
                this.playerExtraHP = 0;
            } 
        }

        Debug.LogFormat("HP : {0}", this.playerHP);
        Debug.LogFormat("extraHP : {0}", this.playerExtraHP);

        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.MainCameraControllerHitEffects);
        this.PlayerHPViewr();

        if (this.playerHP <= 0)
        {
            this.HandlePlayerDeath(playerShell);
        }
    }

    private void HandlePlayerDeath(PlayerShell playerShell)
    {
        playerShell.isDead = true;
        playerShell.rBody2d.simulated = false;
        playerShell.anim.SetInteger("State", -1);
        playerShell.anim.Play("Dead", -1, 0);
        playerShell.eyes[0].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        playerShell.eyes[1].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.LaserLineInactivateLaser);
        var color = playerShell.gunShell.gun.gameObject.GetComponent<SpriteRenderer>().color;
        color.a = 0;
        playerShell.gunShell.gun.gameObject.GetComponent<SpriteRenderer>().color = color;
        this.director.gameOverDirector.GameOverAnimStart();
        this.director.gunCharacteristicChoice.UnShow();

        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.PlayerDie);

        var adMob = GameObject.FindObjectOfType<GoogleAdMobController>();
        if (adMob != null)
        {
            adMob.RequestBannerAd();
        }
    }

    private IEnumerator playerBlinkRoutine(SpriteRenderer playerSprite)
    {
        float blinkTime = 0;
        float blinkInterval = 0.1f;
        bool isBlink = false;

        while (blinkTime <= this.damagedTime)
        {
            var color = playerSprite.color;
            color.a = isBlink ? 1 : 0.5f;
            playerSprite.color = color;

            isBlink = !isBlink;
            blinkTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        var originColor = playerSprite.color;
        originColor.a = 1;
        playerSprite.color = originColor;
    }

    public bool TakeFood(string foodName)
    {
        bool isPlayerHPNotFull = true;

        if (foodName == "Iron_Food" || foodName == "Gold_Food")
        {
            if (this.playerHP > 0 && this.playerHP < this.playerMaxHP)
            {
                if (foodName == "Iron_Food")
                {
                    Debug.Log(foodName);
                    this.playerHP++;
                }
                else if (foodName == "Gold_Food")
                {
                    Debug.Log(foodName);
                    this.playerHP += 2;
                }

                this.playerHP = Mathf.Min(this.playerHP, 3);

                Debug.LogFormat("HP : {0}", this.playerHP);

            }
            else isPlayerHPNotFull = false;
        }
        else if(foodName == "Diamond_Food")
        {
            if (this.playerExtraHP <= 1)
            {
                if (foodName == "Diamond_Food")
                {
                    Debug.Log(foodName);
                    this.playerExtraHP += 1;
                }

                if (playerExtraHP > 2) this.playerExtraHP = 2;
                Debug.LogFormat("ExtraHP : {0}", this.playerExtraHP);
            }
            else isPlayerHPNotFull = false;
        }

        this.PlayerHPViewr();
        return isPlayerHPNotFull;
    }

    public void PlayerHPViewr()
    {
        this.director.healthDirector.extraHeartGOs[0].SetActive(this.playerExtraHP >= 1);
        this.director.healthDirector.extraHeartGOs[1].SetActive(this.playerExtraHP >= 2);

        this.director.healthDirector.healthGOs[2].SetActive(this.playerHP >= 3);
        this.director.healthDirector.healthGOs[1].SetActive(this.playerHP >= 2);
        this.director.healthDirector.healthGOs[0].SetActive(this.playerHP >= 1);
    }

    public void PlayerEnterPortal()
    {
        AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.EnterPortal, this.sfxSource);
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIJoystickDirectorStopJoyStick);
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.LaserLineInactivateLaser);
    }

    public void PlayerExitPortal()
    {
        AudioManager.instance.PlaySFXOneShot(AudioManager.eSFXMusicPlayList.ExitPortal, this.sfxSource);
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIJoystickDirectorActiveJoyStick);
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.LaserLineActivateLaser);
    }

    private void AchieveMaxLevel(string gunType)
    {
        Debug.LogFormat("!!!!!!!!!!!!!!!!!!!!!{0}", gunType);
        switch (gunType)
        {
            case "AssultRifle":
                InfoManager.instance.IncreasePowerStat(100);
                break;
            case "SniperRifle":
                InfoManager.instance.IncreaseCriticalHitAmountStat(100);
                break;
            case "ShotGun":
                InfoManager.instance.IncreaseFireRateStat(100);
                break;
            case "SubmachineGun":
                InfoManager.instance.IncreaseCriticalHitChanceStat(100);
                break;
            default:
                break;
        }
    }
}
