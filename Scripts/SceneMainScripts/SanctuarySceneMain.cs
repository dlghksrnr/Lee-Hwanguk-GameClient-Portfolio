using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EventDispatcher;

public class SanctuarySceneMain : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private UISanctuaryDirector director;

    [SerializeField]
    private ChestItemGenerator chestItemGenerator;

    [SerializeField] private ParticleSystem dungeonPortalEffect;
    [SerializeField] private ParticleSystem spawnPortalEffect;

    private SanctuaryPlayerSpawnPoint spawnPoint;

    private PortalController portal;

    private List<KnightsController> KightList;

    private Camera mainCam;

    private System.Action onStartPortalAnim;

    public System.Action onintotheDungeon;

    public void Init()
    {       
        this.spawnPoint = GameObject.FindObjectOfType<SanctuaryPlayerSpawnPoint>();
        this.spawnPoint.Init();
        this.KightList = GameObject.FindObjectsOfType<KnightsController>().ToList();
        this.KightList.ForEach((x) => x.Init());
        this.portal = GameObject.FindObjectOfType<PortalController>();
        this.chestItemGenerator.Init();
       
        this.onStartPortalAnim = () =>
        {
            this.StartDungeonPortalAnim();
        };

        this.portal.onPlayerGoToThePortal = () =>
        {
            EventDispatcher.Instance.Dispatch<Action>(EventName.UIDialogPanelRandomWeaponDialog, this.onStartPortalAnim);
        };

        this.director.Init();

        this.StartCoroutine(this.CoInitPlayerSpawn());

        EventDispatcher.Instance.AddListener(EventName.SanctuarySceneMainIntotheDungeon, this.onintotheDungeon);
        AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.SANTUARYBG);
        if (InfoManager.instance.gameInfo.isDungeonEntered)
        {
            var adMob = GameObject.FindObjectOfType<GoogleAdMobController>();
            if(adMob != null)
            {
                adMob.DestroyBannerAd();
                adMob.ShowRewardedAd();
            }
        }

        if (!InfoManager.instance.gameInfo.isWatchedTutorialPopup)
        {
            this.director.tutorialDirector.Show();
            InfoManager.instance.gameInfo.isWatchedTutorialPopup = true;
        }
            InfoManager.instance.SaveGameInfo();
    }

    private IEnumerator CoInitPlayerSpawn()
    {
        var Pos = this.spawnPoint.transform.position;
        this.mainCam = Camera.main;
        this.mainCam.transform.position = new Vector3(Pos.x, Pos.y + 1, -11);
        yield return new WaitForSeconds(0.2f);
        this.spawnPortalEffect.Play();
        yield return new WaitForSeconds(0.2f);
        this.player = GameObject.Instantiate(this.player);
        this.player.transform.position = new Vector3(Pos.x, Pos.y + 1, Pos.z);
        this.mainCam.GetComponent<MainCameraController>().Init();
    }

    private void StartDungeonPortalAnim()
    {
        this.StartCoroutine(this.CoStartDungeonPortalAnim());
    }

    private IEnumerator CoStartDungeonPortalAnim()
    {

        this.dungeonPortalEffect.Play();
        yield return new WaitForSeconds(0.2f);
        this.player.SetActive(false);
        yield return new WaitForSeconds(1f);
        this.onintotheDungeon();
    }
}
