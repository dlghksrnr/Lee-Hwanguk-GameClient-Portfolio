using System;
using System.Collections.Generic;

public class EventDispatcher
{
    /// <summary>
    /// 메서드 사용키 (EventDispatcher enum 상수 등록후 사용 * 중복 메서드 등록 방지))
    /// </summary>
    public enum EventName
    {
        UINPCPopupUpdate,
        UINPCPopupActive,
        MainCameraControllerHitEffects,
        SanctuarySceneMainIntotheDungeon,
        DungeonMainToNextStage,
        DungeonMainPlayerToNextRoom,
        DungeonMainPlayerToSanctuary,
        DungeonSceneMainPlayerExpUp,
        UIInventoryAddCell,
        UIInventoryAddEquipment,
        UICurrentInventoryList,
        UIGameOverPopUp,
        UIDungeonDirectorUISetOff,
        UIPortalArrowControllerInitializingArrows,
        UIPortalArrowControllerStopAllArrowCorutines,
        UIDialogPanelRandomWeaponDialog,
        UIDialogPanelStartDialog,
        UIShopGoodsCurrentGold,
        ChestItemGeneratorMakeChest,
        ChestItemGeneratorMakeItemForChest,
        ChestItemGeneratorMakeItemForInventory,
        ChestItemGeneratorMakeFieldCoin,
        DungeonSceneMainTakeFood,
        DungeonSceneMainTakeGun,
        PlayerShellTakeRelic,
        DungeonSceneMainTakeChestDamage,
        UIRelicDirectorTakeRelic,
        UICurrencyDirectorUpdateGoldUI,
        UICurrencyDirectorUpdateEtherUI,
        UIJoystickDirectorStopJoyStick,
        UIJoystickDirectorActiveJoyStick,
        UIAnnounceDirectorStartAnnounce,
        UIDungeonLoadingDirectorStageLoading,
        UIDungeonLoadingDirectorSanctuarytLoading,
        LaserLineInactivateLaser,
        LaserLineActivateLaser,
        UIInventoryDirectorMakeFieldFullText,
        UIInventoryDirectorMakeFullPopupText,
        UIInventoryDirectorMakeFieldFullHealthText,
        UIInventoryDirectorMakeHealthPopupText,
        UIInventoryDirectorButtonScaleAnim,
        UIFieldItemPopupDirectorUpdatePopup,
        UIFieldItemPopupDirectorClosePopup,
        PlayerDie,
        Test,//테스트용 임시로 테스트 하시고 적용한 addlistner 코드 지워주세요.
    }
    private static EventDispatcher instance;

    private Dictionary<EventName, Delegate> listeners = new Dictionary<EventName, Delegate>();

    public static EventDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventDispatcher();
            }
            return instance;
        }
    }

    /// <summary>
    /// 반환값이 없는 메서드 혹은 인스턴스 메서드 등록시
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void AddListener(EventName eventName, Action listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            UnityEngine.Debug.LogWarning("<color=white>이미 해당 키로 메서드가 등록된 상태입니다. 기존 메서드를 덮어 씌웠습니다.</color>");
            listeners.Remove(eventName);
            listeners.Add(eventName, listener);
        }
        else
        {
            listeners.Add(eventName, listener);
        }
    }

    /// <summary>
    /// 인자 1개를 받으며 반환값이 없는 메서드 혹은 인스턴스 메서드 등록시
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void AddListener<T>(EventName eventName, Action<T> listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            UnityEngine.Debug.LogWarning("<color=white>이미 해당 키로 메서드가 등록된 상태입니다. 기존 메서드를 덮어 씌웠습니다.</color>");
            listeners.Remove(eventName);
            listeners.Add(eventName, listener);
        }
        else
        {
            listeners.Add(eventName, listener);
        }
    }

    /// <summary>
    /// 인자 2개를 받으며 반환값이 없는 메서드 혹은 인스턴스 메서드 등록시
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void AddListener<T1, T2>(EventName eventName, Action<T1, T2> listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            UnityEngine.Debug.LogWarning("<color=white>이미 해당 키로 메서드가 등록된 상태입니다. 기존 메서드를 덮어 씌웠습니다.</color>");
            listeners.Remove(eventName);
            listeners.Add(eventName, listener);
        }
        else
        {
            listeners.Add(eventName, listener);
        }
    }

    /// <summary>
    /// 인자 3개를 받으며 반환값이 없는 메서드 혹은 인스턴스 메서드 등록시
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void AddListener<T1, T2, T3>(EventName eventName, Action<T1, T2, T3> listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            UnityEngine.Debug.LogWarning("<color=white>이미 해당 키로 메서드가 등록된 상태입니다. 기존 메서드를 덮어 씌웠습니다.</color>");
            listeners.Remove(eventName);
            listeners.Add(eventName, listener);
        }
        else
        {
            listeners.Add(eventName, listener);
        }
    }

    /// <summary>
    /// 반환 값이 있는 메서드 등록시
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void AddListener<TResult>(EventName eventName, Func<TResult> listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            UnityEngine.Debug.LogWarning("<color=white>이미 해당 키로 메서드가 등록된 상태입니다. 기존 메서드를 덮어 씌웠습니다.</color>");
            listeners.Remove(eventName);
            listeners.Add(eventName, listener);
        }
        else
        {
            listeners.Add(eventName, listener);
        }
    }

    /// <summary>
    /// 인자 1개를 받으며 반환 값이 있는 메서드 등록시
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void AddListener<T1, TResult>(EventName eventName, Func<T1, TResult> listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            UnityEngine.Debug.LogWarning("<color=white>이미 해당 키로 메서드가 등록된 상태입니다. 기존 메서드를 덮어 씌웠습니다.</color>");
            listeners.Remove(eventName);
            listeners.Add(eventName, listener);
        }
        else
        {
            listeners.Add(eventName, listener);
        }
    }

    /// <summary>
    /// 인자 2개를 받으며 반환 값이 있는 메서드 등록시
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void AddListener<T1, T2, TResult>(EventName eventName, Func<T1, T2, TResult> listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            UnityEngine.Debug.LogWarning("<color=white>이미 해당 키로 메서드가 등록된 상태입니다. 기존 메서드를 덮어 씌웠습니다.</color>");
            listeners.Remove(eventName);
            listeners.Add(eventName, listener);
        }
        else
        {
            listeners.Add(eventName, listener);
        }
    }

    /// <summary>
    /// 반환값이 없는 메서드 삭제시
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void RemoveListener(EventName eventName, Action listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            listeners[eventName] = Delegate.Remove(listeners[eventName], listener);
            if (listeners[eventName] == null)
            {
                listeners.Remove(eventName);
            }
        }
    }

    /// <summary>
    /// 인자 1개를 받으며 반환값이 없는 메서드 삭제시
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void RemoveListener<T>(EventName eventName, Action<T> listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            listeners[eventName] = Delegate.Remove(listeners[eventName], listener);
            if (listeners[eventName] == null)
            {
                listeners.Remove(eventName);
            }
        }
    }

    /// <summary>
    /// 인자 2개를 받으며 반환값이 없는 메서드 삭제시
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void RemoveListener<T1, T2>(EventName eventName, Action<T1, T2> listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            listeners[eventName] = Delegate.Remove(listeners[eventName], listener);
            if (listeners[eventName] == null)
            {
                listeners.Remove(eventName);
            }
        }
    }
    /// <summary>
    /// 반환 값이 있는 메서드 제거시
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void RemoveListener<TResult>(EventName eventName, Func<TResult> listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            listeners[eventName] = Delegate.Remove(listeners[eventName], listener);
            if (listeners[eventName] == null)
            {
                listeners.Remove(eventName);
            }
        }
    }

    /// <summary>
    /// 인자 1개를 받으며 반환 값이 있는 메서드 제거시
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void RemoveListener<T1, TResult>(EventName eventName, Func<T1, TResult> listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            listeners[eventName] = Delegate.Remove(listeners[eventName], listener);
            if (listeners[eventName] == null)
            {
                listeners.Remove(eventName);
            }
        }
    }
    /// <summary>
    /// 인자 2개를 받으며 반환 값이 있는 메서드 제거시
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void RemoveListener<T1, T2, TResult>(EventName eventName, Func<T1, T2, TResult> listener)
    {
        if (listeners.ContainsKey(eventName))
        {
            listeners[eventName] = Delegate.Remove(listeners[eventName], listener);
            if (listeners[eventName] == null)
            {
                listeners.Remove(eventName);
            }
        }
    }

    /// <summary>
    /// 반환값이 없는 메서드 혹은 인스턴스 메서드 사용시
    /// </summary>
    /// <param name="eventName"></param>
    public void Dispatch(EventName eventName)
    {
        if (listeners.ContainsKey(eventName))
        {
            ((Action)listeners[eventName])();
        }
    }

    /// <summary>
    /// 반환값이 없는 메서드 혹은 인스턴스 메서드 사용시
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="eventParams"></param>
    /// <param name="eventParams2"></param>
    public void Dispatch<T>(EventName eventName, T eventParams = default)
    {
        if (listeners.ContainsKey(eventName))
        {
            ((Action<T>)listeners[eventName])(eventParams);
        }
    }

    /// <summary>
    /// 반환값이 없는 메서드 혹은 인스턴스 메서드 사용시
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="eventParams"></param>
    /// <param name="eventParams2"></param>
    public void Dispatch<T1, T2>(EventName eventName, T1 eventParams = default, T2 eventParams2 = default)
    {
        if (listeners.ContainsKey(eventName))
        {
            ((Action<T1, T2>)listeners[eventName])(eventParams, eventParams2);
        }
    }

    /// <summary>
    /// 반환값이 없는 메서드 혹은 인스턴스 메서드 사용시
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="eventParams"></param>
    /// <param name="eventParams2"></param>
    public void Dispatch<T1, T2, T3>(EventName eventName, T1 eventParams = default, T2 eventParams2 = default, T3 eventParams3 = default)
    {
        if (listeners.ContainsKey(eventName))
        {
            ((Action<T1, T2, T3>)listeners[eventName])(eventParams, eventParams2, eventParams3);
        }
    }

    /// <summary>
    /// 반환 값이 있는 메서드 혹은 인스턴스 메서드 사용시
    /// </summary>
    /// <typeparam name="TResult"> 반환 타입</typeparam>
    /// <param name="eventName">이벤트 등록 키</param>
    /// <param name="result">반환값</param>
    /// <exception cref="InvalidCastException"></exception>
    public void Dispatch<TResult>(EventName eventName, out TResult result)
    {
        if (listeners.ContainsKey(eventName))
        {
            var listener = listeners[eventName];
            if (listener is Func<TResult> func)
            {
                result = func();
            }
            else
            {
                throw new InvalidCastException($"Listener for event '{eventName}' is not a Func with the correct signature.");
            }
        }
        else
        {
            result = default;
        }
    }

    /// <summary>
    /// 인자를 1개 받고반환 값이 있는 메서드 혹은 인스턴스 메서드 사용시
    /// </summary>
    /// <typeparam name="TResult"> 반환 타입</typeparam>
    /// <param name="eventName">이벤트 등록 키</param>
    /// <param name="eventParams">매개변수</param>
    /// <param name="result">반환값</param>
    /// <exception cref="InvalidCastException"></exception>
    public void Dispatch<T, TResult>(EventName eventName, T eventParams, out TResult result)
    {
        if (listeners.ContainsKey(eventName))
        {
            var listener = listeners[eventName];
            if (listener is Func<T, TResult> func)
            {
                result = func(eventParams);
            }
            else
            {
                throw new InvalidCastException($"Listener for event '{eventName}' is not a Func with the correct signature.");
            }
        }
        else
        {
            result = default;
        }
    }

    /// <summary>
    /// 인자를 2개 받고반환 값이 있는 메서드 혹은 인스턴스 메서드 사용시
    /// </summary>
    /// <typeparam name="TResult"> 반환 타입</typeparam>
    /// <param name="eventName">이벤트 등록 키</param>
    /// <param name="eventParams">첫번째 인자</param>
    /// <param name="eventParams2">두번째 인자</param>
    /// <param name="result">반환값</param>
    /// <exception cref="InvalidCastException"></exception>
    public void Dispatch<T1, T2, TResult>(EventName eventName, T1 eventParams, T2 eventParams2, out TResult result)
    {
        if (listeners.ContainsKey(eventName))
        {
            var listener = listeners[eventName];
            if (listener is Func<T1, T2, TResult> func)
            {
                result = func(eventParams, eventParams2);
            }
            else
            {
                throw new InvalidCastException($"Listener for event '{eventName}' is not a Func with the correct signature.");
            }
        }
        else
        {
            result = default;
        }
    }


}