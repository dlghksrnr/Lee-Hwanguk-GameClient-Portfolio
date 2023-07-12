using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public enum eBulletType
    {
        FIREWORM,
        EVILWIZARD,
        DARKWIZARD,
        SLIMEDEMON
    }
    public eBulletType bossType;

    public static BulletPool instance; //인스턴스화

    [Header("Boss Bullet Type")]
    [SerializeField]
    public GameObject fireWormPooledBullet;
    [SerializeField]
    public GameObject evilWizardPooledBullet;
    [SerializeField]
    public GameObject darkwizardPooledBullet;
    [SerializeField]
    public GameObject slimeDemonPooleedBullet;

    private bool notEnoughBulletsInPool = true;

    private List<GameObject> bulletList;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        if (this.transform.parent.name == "Forrest_Boss_LHK".Replace("Clone", ""))
        {
            this.bossType = eBulletType.FIREWORM;
        }
        else if (this.transform.parent.name == "Grave_Boss_LHK(Clone)") //Test
        {
            this.bossType = eBulletType.EVILWIZARD;
        }
        else if (this.transform.parent.name == "Temple_Boss_LHK(Clone)") //Test
        {
            this.bossType = eBulletType.DARKWIZARD;
        }
        else if (this.transform.parent.name == "HeartOfDevil_Boss_LHK(Clone)") //Test
        {
            this.bossType = eBulletType.SLIMEDEMON;
        }
        Debug.LogFormat("boss type :{0}", this.bossType);
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
            if (this.bossType == eBulletType.FIREWORM)
            {
                GameObject bul = Instantiate(this.fireWormPooledBullet);
                bul.SetActive(false);
                this.bulletList.Add(bul);
                return bul;
            }
            else if (this.bossType == eBulletType.EVILWIZARD)
            {
                GameObject bul = Instantiate(this.evilWizardPooledBullet);
                bul.SetActive(false);
                this.bulletList.Add(bul);
                return bul;
            }
            else if (this.bossType == eBulletType.DARKWIZARD)
            {
                GameObject bul = Instantiate(this.darkwizardPooledBullet);
                bul.SetActive(false);
                this.bulletList.Add(bul);
                return bul;
            }
            else if (this.bossType == eBulletType.SLIMEDEMON)
            {
                GameObject bul = Instantiate(this.slimeDemonPooleedBullet);
                bul.SetActive(false);
                this.bulletList.Add(bul);
                return bul;
            }
        }
        return null;
    }

}