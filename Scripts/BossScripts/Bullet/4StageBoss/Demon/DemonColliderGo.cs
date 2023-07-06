using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonColliderGo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("<color=red>CombatAttack Hit!</color>");
            EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.MainCameraControllerHitEffects);
        }
    }

}
