using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BulletPool;

public class SplashBulletPool : MonoBehaviour
{
    public static SplashBulletPool instance; //인스턴스화

    [Header("Boss Bullet Type")]

    [SerializeField]
    public GameObject darkwizardsplashPooledBullet;

    private bool notEnoughBulletsInPool = true;

    private List<GameObject> bulletList;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        this.bulletList = new List<GameObject>(); //Bullet List초기화 
    }

    public GameObject GetBullet()
    {
        if (this.bulletList.Count > 0)
        {
            for (int i = 0; i < this.bulletList.Count; i++)
            {
                if (!this.bulletList[i].activeInHierarchy) //부모 오브잭트의 활성화 체크
                {
                    return this.bulletList[i];
                }
            }
        }

        if (this.notEnoughBulletsInPool)
        {
            GameObject bul = Instantiate(this.darkwizardsplashPooledBullet);
            bul.SetActive(false);
            this.bulletList.Add(bul);
            return bul;
            
        }
        return null;
    }
}

