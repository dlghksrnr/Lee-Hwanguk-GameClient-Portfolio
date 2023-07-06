using UnityEngine;

public abstract class AbsBossAttackPattern : MonoBehaviour
{

    public virtual void Pattern(Transform target, Transform firePoint, Rigidbody2D rb, Transform trans)
    {
    }

}