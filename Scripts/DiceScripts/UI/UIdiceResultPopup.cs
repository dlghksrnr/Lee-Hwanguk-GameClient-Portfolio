using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIdiceResultPopup : MonoBehaviour
{
    public DiceScript diceGo;
    public Image resultImgFrame;
    public Image resultImgIcon;
    public Text txtName;
    public Text txtPowerStat;
    public Text txtCriticalAmount;
    public Text txtCriticalChance;
    public Text txtRateStat;


    public void Init()
    {
        this.gameObject.SetActive(false);
    }
}
