using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern02 : AbsBossAttackPattern
{
    public override void Pattern(Transform player, Transform firePoint, Rigidbody2D rb, Transform trans)
    {
        int bulletsAmount = 15;
        float startAngle = 0f; // 시작 각도
        float endAngle = 360f; // 끝나는 각도
        float angleStep = (endAngle - startAngle) / bulletsAmount; // bullet의 간격

        for (int i = 0; i < bulletsAmount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector3 bulMoveVector = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            var bulDir = (player.position - firePoint.transform.position).normalized;
            GameObject bul = BulletPool.instance.GetBullet();
            bul.transform.position = firePoint.transform.position;

            // 회전값 수정
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, bulDir);
            bul.transform.rotation = rotation;

            // 회전 방향으로 이동 벡터 적용
            bul.GetComponent<FireWormBullet>().SetMoveDirection(bulMoveVector);

            bul.SetActive(true);
        }
    }
}
