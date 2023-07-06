using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissilePool : MonoBehaviour
{
    public static HomingMissilePool instance;
    [SerializeField]
    private GameObject pooledHomingMissile;
    private bool notEnoughBulletsInPool = true;
    private List<GameObject> homingMissilesList;

    private void Awake()
    {
        instance= this;
    }
    void Start()
    {
        this.homingMissilesList = new List<GameObject>(); //homingMissiles list 초기화

    }
    public GameObject GetHomingMissile()
    {
        if (this.homingMissilesList.Count > 0)
        {
            for (int i = 0; i < this.homingMissilesList.Count; i++)
            {
                if (!this.homingMissilesList[i].activeInHierarchy) //부모 오브잭트의 활성화 체크
                {
                    return this.homingMissilesList[i];
                }
            }
        }

        if (this.notEnoughBulletsInPool)
        {
            GameObject bul = Instantiate(this.pooledHomingMissile);
            bul.SetActive(false);
            this.homingMissilesList.Add(bul);
            return bul;
        }
        return null;
    }
}
