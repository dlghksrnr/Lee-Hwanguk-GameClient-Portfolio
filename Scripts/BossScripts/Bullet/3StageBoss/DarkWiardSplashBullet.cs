using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DarkWiardSplashBullet : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    public GameObject explosion;
    private Rigidbody2D rb;
    private Collider2D col;

    private void OnEnable()
    {
        StartBulletAnimation();
    }

    private void Start()
    {
        this.moveSpeed = 5f;
        this.rb = GetComponent<Rigidbody2D>();
        this.col = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        //float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.velocity = moveDirection * moveSpeed;
    }

    public void SetMoveDirection(Vector2 dir)
    {
        this.moveDirection = dir.normalized;
    }
    private void Destroy()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //this.gameObject.SetActive(false);
            var explosionGo = Instantiate(this.explosion);
            explosionGo.transform.position = this.transform.position;
        }
        if (collision.name == "BossRoomColider")
        {
            Debug.Log("BulletDestroy");
            this.Destroy();
        }
    }

    private void StartBulletAnimation()
    {
        transform.localScale = new Vector3(2f,2f,2f);

        transform.DOScale(Vector3.one, 1f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            col.enabled = true;

            transform.DOScale(Vector3.zero, 2f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
                transform.localScale = Vector3.one;
            });
        });
    }
}
