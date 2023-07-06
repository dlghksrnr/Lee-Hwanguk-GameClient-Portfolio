using UnityEngine;

public class AttackPattern05 : AbsBossAttackPattern
{
    public override void Pattern(Transform target, Transform firePoint, Rigidbody2D rb, Transform trans)
    {
        int bulletsAmount = 3;
        float startAngle = -20f; //시작 각도
        float endAngle = 20f; //끝나는 각도
        float angleStep = (endAngle - startAngle) / (bulletsAmount - 1); //bullet의 간격
        Vector2 targetDirection = target.position - firePoint.position; //방향 백터
        for (int i = 0; i < bulletsAmount; i++)
        {
            float angle = startAngle + angleStep * i;
            // 발사 각도로 회전값 계산
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            // 오브젝트 풀에서 총알 가져오기
            GameObject bullet = BulletPool.instance.GetBullet();
            bullet.transform.position = firePoint.position;
            Vector2 bulletDirection = rotation * targetDirection.normalized;
            bullet.GetComponent<EvilWizardBullet>().SetMoveDirection(bulletDirection);
            bullet.SetActive(true);
        }
    }
}
