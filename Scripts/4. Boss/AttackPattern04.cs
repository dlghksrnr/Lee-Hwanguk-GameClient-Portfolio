using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class AttackPattern04 : AbsBossAttackPattern
{
    public override void Pattern(Transform target, Transform firePoint, Rigidbody2D rb, Transform trans)
    {
        var bulDir = (target.position - firePoint.transform.position).normalized;
        GameObject bul = BulletPool.instance.GetBullet();
         
        bul.transform.rotation = target.transform.rotation;
        bul.transform.position = firePoint.transform.position;
       
        bul.SetActive(true);
        bul.GetComponent<EvilWizardBullet>().SetMoveDirection(bulDir);
    }
}
