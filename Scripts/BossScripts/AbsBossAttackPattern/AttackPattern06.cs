using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern06 : AbsBossAttackPattern
{
    public override void Pattern(Transform target, Transform firePoint, Rigidbody2D rb, Transform trans)
    {
        int meteorAmount = 5;
        float radius = 7f;

        for (int i = 0; i < meteorAmount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            Vector3 meteorPosition = target.position + new Vector3(randomOffset.x, 5f, 0f);

            GameObject meteor = EvilWizardMeteorPool.instance.GetMeteor();
            if (meteor != null)
            {
                meteor.transform.position = meteorPosition;
                meteor.SetActive(true);
            }
        }

    }
}
