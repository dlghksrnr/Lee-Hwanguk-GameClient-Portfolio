using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsBossClass : Damageable, IDungeonBossHandler //(Mono)
{
    public float bossMoveSpeed;
    public IEnumerator patternCorutine;
    public Vector3 direction;
    public bool isTransitioningPhase2; 
    public bool isTransitioningPhase3; 
    public float playerDamage;
    public bool isAttack;
    public Transform player;

    public string phase2Color = "#FF8D8D";
    public string phase3Color = "#FF0000";

    public virtual void StartBossFight() { }
    public abstract IEnumerator Phase1(); 
    public abstract IEnumerator Phase2();
    public abstract IEnumerator Phase3();
    public virtual void MovePattern() { }
    public virtual IEnumerator TransitionToPhase2()
    {

        yield return StartCoroutine(this.Phase2());

        this.isTransitioningPhase2 = false;
    }
    public virtual IEnumerator TransitionToPhase3()
    {

        yield return StartCoroutine(this.Phase3());

        this.isTransitioningPhase3 = false;
    }
    public abstract IEnumerator BossDie();


    public IEnumerator Phase2Color(string phase2Color)
    {
        Renderer rend = this.GetComponent<Renderer>();
        Color colorToBlink = Color.white;

        if (ColorUtility.TryParseHtmlString(phase2Color, out colorToBlink))
        {
            while (true)
            {
                rend.material.color = colorToBlink;
                yield return new WaitForSeconds(1f);
                rend.material.color = Color.white;
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public IEnumerator Phase3Color(string phase3Color) 
    {
        Renderer rend = this.GetComponent<Renderer>();
        Color colorToBlink = Color.white;

        if (ColorUtility.TryParseHtmlString(phase3Color, out colorToBlink))
        {
            while (true)
            {
                rend.material.color = colorToBlink;
                yield return new WaitForSeconds(0.5f);
                rend.material.color = Color.white;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDash"))
        {
            var dashAttack = collision.transform.parent.gameObject.GetComponent<DashAttack>();
            this.TakeBulletDamage(dashAttack.damage);
        }
    }
    public void Intializing()
    {
        this.Init();
    }
    public void InitStartBossFight(Transform playerTrans)
    {
        this.player = playerTrans;
        this.StartBossFight();
    }

}
