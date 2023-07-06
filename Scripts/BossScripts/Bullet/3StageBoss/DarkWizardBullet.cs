using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkWizardBullet : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    public GameObject explosion;
    private Rigidbody2D rb;

    private void OnEnable()
    {
        moveSpeed = 3f; // 활성화 시 속도 초기화
        Invoke("Destroy", 5f);
    }

    private void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;

        if (moveSpeed < 10f)
        {
            moveSpeed += 3f * Time.fixedDeltaTime;
        }

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
            var explosionGo = Instantiate(this.explosion);
            explosionGo.transform.position = this.transform.position;
            EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.MainCameraControllerHitEffects);
        }
        if (collision.name == "BossRoomColider")
        {
            Debug.Log("BulletDestroy");
            this.Destroy();
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}

