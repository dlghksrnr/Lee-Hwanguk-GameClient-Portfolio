using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
    }
}
