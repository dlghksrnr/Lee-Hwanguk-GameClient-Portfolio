using SpriteGlow;
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    public GameObject coinPrefab;
    private const int PoolSize = 100;
    private List<GameObject> objectPool;

    public void Init(GameObject coinPrefab)
    {
        this.coinPrefab = coinPrefab;
        this.objectPool = new List<GameObject>();
        for (int i = 0; i < PoolSize; i++)
        {
            this.CreateInstance();
        }
    }

    private void CreateInstance()
    {
        var coinGo = Instantiate(this.coinPrefab, this.transform);
        coinGo.tag = "FieldCoin";
        var atlas = AtlasManager.instance.GetAtlasByName("UIEquipmentIcon");
        var sprite = atlas.GetSprite("Gold");

        var coinSP = coinGo.GetComponent<SpriteRenderer>();
        coinSP.sprite = sprite;
        coinSP.sortingLayerName = "Default";
        coinSP.sortingOrder = 50;

        coinGo.transform.localScale = new Vector3(3.79f, 3.79f, 3f);
        coinGo.name = sprite.name.Replace("(Clone)", "");

        var coinCol = coinGo.GetComponent<BoxCollider2D>();  
        coinCol.size = new Vector2(2f, 2f);

        var comp = coinGo.GetComponent<SpriteGlowEffect>();
        comp.GlowColor = Color.yellow;

        this.objectPool.Add(coinGo);
        coinGo.SetActive(false);
    }

    public GameObject GetObjectFromPool()
    {
        for (int i = 0; i < this.objectPool.Count; i++)
        {
            if (!this.objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }
        var newObj = Instantiate(this.coinPrefab, this.transform);
        this.objectPool.Add(newObj);
        return newObj;
    }
}