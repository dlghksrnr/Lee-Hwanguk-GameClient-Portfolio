using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIDiceDirector : MonoBehaviour
{
    public UIDice uiDice;
    public void Init()
    {
        this.uiDice.onClosing = () => {
            this.gameObject.SetActive(false);
        };
        this.uiDice.Init();

        this.gameObject.SetActive(false);
        this.uiDice.isEnugh = true;
    }

}
