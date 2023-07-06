using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern14 : AbsBossAttackPattern
{
    private float angle = 0f;
    private float pluseAngle = 30f;

    public override void Pattern(Transform target, Transform firePoint, Rigidbody2D rb, Transform trans)
    {
        for (int i = 0; i <= 1; i++)
        {
            float bulDirX = firePoint.transform.position.x + Mathf.Sin(((angle + 180f * i) * Mathf.PI) / 180f);
            float bulDirY = firePoint.transform.position.y + Mathf.Cos(((angle + 180f * i) * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - firePoint.transform.position).normalized;

            GameObject bul = BulletPool.instance.GetBullet();
            bul.transform.position = firePoint.transform.position;
            bul.transform.rotation = firePoint.transform.rotation;
            bul.SetActive(true);
            bul.GetComponent<FireWormBullet>().SetMoveDirection(bulDir);
        }

        angle += pluseAngle;
        if (angle >= 360f)
        {
            angle = 0f;
        }
    }
}
