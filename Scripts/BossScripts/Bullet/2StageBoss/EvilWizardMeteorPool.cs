using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilWizardMeteorPool : MonoBehaviour
{
    public static EvilWizardMeteorPool instance;
    [SerializeField]
    private GameObject pooledMeteor;
    private bool notEnoughBulletsInPool = true;
    private List<GameObject> ResidueList;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        this.ResidueList = new List<GameObject>();

    }
    public GameObject GetMeteor()
    {
        if (this.ResidueList.Count > 0)
        {
            for(int i=0; i<this.ResidueList.Count; i++)
            {
                if (!this.ResidueList[i].activeInHierarchy)
                {
                    return this.ResidueList[i];
                }
            }
        }
        if(this.notEnoughBulletsInPool)
        {
            GameObject res = Instantiate(this.pooledMeteor);
            res.SetActive(false);
            this.ResidueList.Add(res);
            return res;
        }
        return null;
    }
}
