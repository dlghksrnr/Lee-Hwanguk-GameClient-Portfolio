using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern03 : AbsBossAttackPattern
{

    public override void Pattern(Transform player, Transform firePoint, Rigidbody2D rb, Transform trans)
    {
        int bulletAmount = 3;
        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject bul = HomingMissilePool.instance.GetHomingMissile();
            bul.transform.position = firePoint.transform.position;
            bul.transform.rotation = firePoint.transform.rotation;
            bul.SetActive(true);
            bul.GetComponent<FireWormHomingMissile>().SetTarget(player); //target 오브잭트 찾기
        }
    }
}
