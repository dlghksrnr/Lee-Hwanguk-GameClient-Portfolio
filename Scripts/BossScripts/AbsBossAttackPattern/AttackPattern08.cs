using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern08 : AbsBossAttackPattern
{
    public override void Pattern(Transform target, Transform firePoint, Rigidbody2D rb, Transform trans)
    {
        int bulletsAmount = 3;
        List<int> bulletIndices = new List<int>(); //총알 인덱스를 저장할 리스트 생성

        //총알 인덱스 랜덤 생성
        for (int i = 0; i < bulletsAmount; i++)
        {
            bulletIndices.Add(i);
        }
        //bulletIndices.Shuffle(); //리스트 요소를 랜덤하게 섞음

        float startAngle = -30f; //시작 각도
        float endAngle = 30f; //끝나는 각도
        float angleStep = (endAngle - startAngle) / (bulletsAmount - 1); //bullet의 간격
        Vector2 targetDirection = target.position - firePoint.position; //방향 백터

        for (int i = 0; i < bulletsAmount; i++)
        {
            float angle = startAngle + angleStep * bulletIndices[i]; //랜덤한 순서로 발사 각도 결정

            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            GameObject bullet = BulletPool.instance.GetBullet();
            bullet.transform.position = firePoint.position;
            Vector2 bulletDirection = rotation * targetDirection.normalized;
            bullet.GetComponent<DarkWizardBullet>().SetMoveDirection(bulletDirection);
            bullet.SetActive(true);
        }
    }
}
