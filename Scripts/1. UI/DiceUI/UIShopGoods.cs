using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopGoods : MonoBehaviour
{
    public Image imgGold;
    public Text txtGold;
    public System.Action onCurrentGold;

    public enum eShopSceneType
    {
        SANCTUARY,
        DUNGEON
    }
    public eShopSceneType type;
    public void Init()
    {
        if (this.type == eShopSceneType.SANCTUARY)
        {
            this.txtGold.text = InfoManager.instance.possessionAmountInfo.goldAmount.ToString() + "Gold";
            this.onCurrentGold = () => {
                this.txtGold.text = InfoManager.instance.possessionAmountInfo.goldAmount.ToString() + "Gold";
            };
            EventDispatcher.Instance.AddListener(EventDispatcher.EventName.UIShopGoodsCurrentGold, this.onCurrentGold);
        }

        else if(this.type == eShopSceneType.DUNGEON)
        {
            this.txtGold.text = InfoManager.instance.possessionAmountInfo.dungeonGoldAmount.ToString() + "Gold";
            this.onCurrentGold = () => {
                this.txtGold.text = InfoManager.instance.possessionAmountInfo.dungeonGoldAmount.ToString() + "Gold";
            };
            EventDispatcher.Instance.AddListener(EventDispatcher.EventName.UIShopGoodsCurrentGold, this.onCurrentGold);
        }
    }

}
