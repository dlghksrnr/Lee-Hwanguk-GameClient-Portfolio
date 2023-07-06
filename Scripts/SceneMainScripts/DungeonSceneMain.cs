using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public partial class DungeonSceneMain : MonoBehaviour
{
    [SerializeField]
    private UIDungeonDirector director;
    [SerializeField]
    public MapGenerator generator;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private MainCameraController mainCam;
    [SerializeField]
    private Grid astar;
    [SerializeField]
    private ChestItemGenerator chestItemGenerator;
    [SerializeField]
    private List<GameObject> portalEffectList;

    public GameObject player;

    public Action<Vector2, string> getPortalInfo;
    public Action setPlayer;
    public Action goBackToTheSanctuary;

    private bool isPlayerSpawned = false;
    private bool isBossClear = false;

    [SerializeField]
    private AudioSource sfxSource;

    [Tooltip("몬스터 생성 여부 (체크시 전체 몬스터 생성 오프")] [SerializeField] private bool MonsterGen = true;

    [Tooltip("생성하고 싶은 룸 수")]
    public int roomCount;


    //Test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) this.generator.DeActivePortalsAndRoomsForTest();
        if (Input.GetKeyDown(KeyCode.M)) this.MonsterGen = !this.MonsterGen;
    }

    public void Init()
    {
        this.director.Init();
        this.chestItemGenerator.Init();
        this.InitStage();
        this.playerHP = 3;
        this.damagedTime = 1;
        Physics2D.IgnoreLayerCollision(6, 9, true);

        EventDispatcher.Instance.AddListener<System.Action>(EventDispatcher.EventName.DungeonMainToNextStage,
           this.InitStage);
        EventDispatcher.Instance.AddListener<Vector2, string, Vector3>(EventDispatcher.EventName.DungeonMainPlayerToNextRoom,
            this.MoveToNextArea);
        EventDispatcher.Instance.AddListener(EventDispatcher.EventName.DungeonMainPlayerToSanctuary, this.goBackToTheSanctuary);
        EventDispatcher.Instance.AddListener<int>(EventDispatcher.EventName.DungeonSceneMainPlayerExpUp, this.OnPlayerExpUp);
        EventDispatcher.Instance.AddListener<string, bool>(EventDispatcher.EventName.DungeonSceneMainTakeFood, this.TakeFood);
        EventDispatcher.Instance.AddListener<int, bool>(EventDispatcher.EventName.DungeonSceneMainTakeChestDamage, this.TakeChestDamage);

        InfoManager.instance.gameInfo.isDungeonEntered = true;
    }

    private Transform InitRoom()
    {
        this.roomCount = this.CheckPlayerDungeonStatus();
        this.isBossClear = false;
        while (true)
        {
            for (int i = 0; i < this.generator.wholeRoomList.Count; i++) { Destroy(this.generator.wholeRoomList[i]); }
            this.generator.wholeRoomList.Clear();
            this.generator.Init();
            if (this.generator.wholeRoomList.Count == this.roomCount && this.generator.normalRoomList.Count != 0) break;
        }

        return this.generator.wholeRoomList.Find(x => x.transform.name.Contains("Start")).transform;
    }

    /// <summary>
    /// 플레이어 스테이지 이동시 실행
    /// </summary>
    public void InitStage(System.Action onComplete = null)
    {
        AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.Dungeon_FirstRoom);
        if (this.player != null)
        {
            this.player.transform.SetParent(null);
            this.player.gameObject.SetActive(false);
        }
        this.astar.transform.SetParent(null);
        var startRoomTrans = this.InitRoom();
        this.StartCoroutine(this.CoPlayerSpawnControl(startRoomTrans, onComplete));
        this.director.onMiniMapUpdatePopup(startRoomTrans.position);
        this.StartCoroutine(this.CoStartAnnounce());
    }
    /// <summary>
    /// 플레이어 스폰 제어
    /// </summary>
    /// <param name="startRoomTrans"></param>
    /// <returns></returns>
    private IEnumerator CoPlayerSpawnControl(Transform startRoomTrans, System.Action onComplete)
    {
        var startPos = startRoomTrans.position;
        this.mainCam.transform.position = new Vector3(startPos.x, startPos.y, -11);
        if (onComplete != null)
            onComplete();
        yield return new WaitForSeconds(0.3f);
        Debug.Log(startRoomTrans);
        var comp = startRoomTrans.GetComponentInChildren<ParticleSystem>();
        comp.Play();
        yield return new WaitForSeconds(0.2f);
        if (!this.isPlayerSpawned)
        {
            this.player = Instantiate(this.playerPrefab, startRoomTrans);
            this.player.gameObject.SetActive(true);
            this.isPlayerSpawned = true;
            this.SetSkillCoolTime();
            yield return null;
            this.mainCam.Init();
        }
        else if (this.isPlayerSpawned)
        {
            this.player.gameObject.SetActive(true);
            this.player.transform.SetParent(startRoomTrans);
            yield return null;
            this.mainCam.Init();
        }
        var SpawnPoint = startRoomTrans.GetChild(4).gameObject;
        SpawnPoint.GetComponent<SanctuaryPlayerSpawnPoint>().Init();
        var pos = SpawnPoint.transform.localPosition;
        this.player.transform.localPosition = new Vector3(pos.x, pos.y + 1, pos.z);
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIPortalArrowControllerInitializingArrows,
               startRoomTrans);
    }

    private int CheckPlayerDungeonStatus()
    {
        var roomCount = default(int);
        if (InfoManager.instance.dungeonInfo.CurrentStageInfo == 1) roomCount = 6;
        else if (InfoManager.instance.dungeonInfo.CurrentStageInfo == 2) roomCount = 7;
        else if (InfoManager.instance.dungeonInfo.CurrentStageInfo == 3) roomCount = 8;
        else if (InfoManager.instance.dungeonInfo.CurrentStageInfo == 4) roomCount = 10;
        return roomCount;
    }

    /// <summary>
    /// 플레이어 포탈 이동시 실행
    /// </summary>
    private void MoveToNextArea(Vector2 originPos, string portalName, Vector3 portalPos)
    {
        EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIPortalArrowControllerStopAllArrowCorutines);
        this.mainCam.isPlayerTeleporting = true;
        this.player.gameObject.SetActive(false);
        this.StartCoroutine(this.PlayerTransferToRoom(originPos, portalName, portalPos));
    }

    private IEnumerator PlayerTransferToRoom(Vector2 originPos, string portalName, Vector3 portalPos)
    {
        var objIndex = default(int);
        if (portalName.Contains("0"))
        {
            this.PlayerEnterPortal();
            objIndex = this.generator.wholeRoomList.FindIndex(obj => (Vector2)obj.transform.position == new Vector2(originPos.x, originPos.y + this.generator.mapDist));
            this.generator.wholeRoomList[objIndex].gameObject.SetActive(true);
            var spawnPos = this.generator.wholeRoomList[objIndex].transform.GetChild(2).position;
            this.MakePortalEffects(portalPos, this.generator.wholeRoomList[objIndex], portalName);
            yield return new WaitForSeconds(1f);
            this.mainCam.transform.position = new Vector3(spawnPos.x, spawnPos.y, -10);
            yield return new WaitForSeconds(0.5f);
            this.PlayerExitPortal();
            this.player.gameObject.SetActive(true);
            this.player.transform.SetParent(this.generator.wholeRoomList[objIndex].transform);
            this.player.transform.position = new Vector2(spawnPos.x, spawnPos.y + 2);
        }
        else if (portalName.Contains("1"))
        {
            this.PlayerEnterPortal();
            objIndex = this.generator.wholeRoomList.FindIndex(obj => (Vector2)obj.transform.position == new Vector2(originPos.x + this.generator.mapDist, originPos.y));
            this.generator.wholeRoomList[objIndex].gameObject.SetActive(true);
            var spawnPos = this.generator.wholeRoomList[objIndex].transform.GetChild(3).position;
            this.MakePortalEffects(portalPos, this.generator.wholeRoomList[objIndex], portalName);
            yield return new WaitForSeconds(1f);
            this.mainCam.transform.position = new Vector3(spawnPos.x, spawnPos.y, -10);
            yield return new WaitForSeconds(0.5f);
            this.PlayerExitPortal();
            this.player.gameObject.SetActive(true);
            this.player.transform.SetParent(this.generator.wholeRoomList[objIndex].transform);
            this.player.transform.position = new Vector2(spawnPos.x + 1, spawnPos.y);
        }
        else if (portalName.Contains("2"))
        {
            this.PlayerEnterPortal();
            objIndex = this.generator.wholeRoomList.FindIndex(obj => (Vector2)obj.transform.position == new Vector2(originPos.x, originPos.y - this.generator.mapDist));
            this.generator.wholeRoomList[objIndex].gameObject.SetActive(true);
            var spawnPos = this.generator.wholeRoomList[objIndex].transform.GetChild(0).position;
            this.MakePortalEffects(portalPos, this.generator.wholeRoomList[objIndex], portalName);
            yield return new WaitForSeconds(1f);
            this.mainCam.transform.position = new Vector3(spawnPos.x, spawnPos.y, -10);
            yield return new WaitForSeconds(0.5f);
            this.PlayerExitPortal();
            this.player.gameObject.SetActive(true);
            this.player.transform.SetParent(this.generator.wholeRoomList[objIndex].transform);
            this.player.transform.position = new Vector2(spawnPos.x, spawnPos.y - 1);
        }
        else if (portalName.Contains("3"))
        {
            this.PlayerEnterPortal();
            objIndex = this.generator.wholeRoomList.FindIndex(obj => (Vector2)obj.transform.position == new Vector2(originPos.x - this.generator.mapDist, originPos.y));
            this.generator.wholeRoomList[objIndex].gameObject.SetActive(true);
            var spawnPos = this.generator.wholeRoomList[objIndex].transform.GetChild(1).position;
            this.MakePortalEffects(portalPos, this.generator.wholeRoomList[objIndex], portalName);
            yield return new WaitForSeconds(1f);
            this.mainCam.transform.position = new Vector3(spawnPos.x, spawnPos.y, -10);
            yield return new WaitForSeconds(0.5f);
            this.PlayerExitPortal();
            this.player.gameObject.SetActive(true);
            this.player.transform.SetParent(this.generator.wholeRoomList[objIndex].transform);
            this.player.transform.position = new Vector2(spawnPos.x - 1, spawnPos.y);
        }
        this.mainCam.player = this.player;
        this.mainCam.isPlayerTeleporting = false;
        this.director.onMiniMapUpdatePopup(this.generator.wholeRoomList[objIndex].transform.position);
        var roomName = this.generator.wholeRoomList[objIndex].name;
        var go = Instantiate(this.portalEffectList[0]);
        go.transform.position = new Vector3(this.player.transform.position.x, this.player.transform.position.y + 1, 1);

        if (!(roomName.Contains("Hidden") || roomName.Contains("Boss") || roomName.Contains("Safe") || roomName.Contains("Start")))
        {
            var MonGen = this.generator.wholeRoomList[objIndex].GetComponent<MonsterGenerator>();
            if (MonGen.isFinished == false)
            {
                this.astar.gameObject.transform.SetParent(this.generator.wholeRoomList[objIndex].transform.GetChild(4));
                this.astar.transform.localPosition = Vector3.zero;

                if (this.MonsterGen)
                {
                    
                    this.astar.Init();
                    MonGen.Init();
                }
            }
            else
            {
                EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIPortalArrowControllerInitializingArrows,
              this.generator.wholeRoomList[objIndex].transform);
            }

            AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.DUNGEONBG);
        }
        else if (roomName.Contains("Hidden") || roomName.Contains("Safe") || roomName.Contains("Start"))
        {
            EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIPortalArrowControllerInitializingArrows,
               this.generator.wholeRoomList[objIndex].transform);
            if (roomName.Contains("Hidden"))
            {
                EventDispatcher.Instance.Dispatch<UIAnnounceDirector.eAnnounceType>(EventDispatcher.EventName.UIAnnounceDirectorStartAnnounce,
UIAnnounceDirector.eAnnounceType.HIDDENROOM);
                AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.Dungeon_HiddeonRoom);
            }
            else if (roomName.Contains("Safe")) 
            {
                EventDispatcher.Instance.Dispatch<UIAnnounceDirector.eAnnounceType>(EventDispatcher.EventName.UIAnnounceDirectorStartAnnounce,
             UIAnnounceDirector.eAnnounceType.SAFEHOUSE);
                AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.Dungeon_Shop);
            } 
            else if (roomName.Contains("Start"))
            {
                AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.Dungeon_FirstRoom);
            }
        }
        else if (roomName.Contains("LHK"))
        {
            var stageNum = InfoManager.instance.dungeonInfo.CurrentStageInfo;

            var bossComp = this.generator.wholeRoomList[objIndex].GetComponentInChildren<IDungeonBossHandler>();
            if (!this.isBossClear)
            {
                AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.NONE, true);
                AudioManager.instance.BGMusicControl(AudioManager.eBGMusicPlayList.DUNGEONBGBOSS);
                this.isBossClear = true;
                bossComp.InitStartBossFight(this.player.transform);
                EventDispatcher.Instance.Dispatch<UIAnnounceDirector.eAnnounceType>(EventDispatcher.EventName.UIAnnounceDirectorStartAnnounce,
                    UIAnnounceDirector.eAnnounceType.BOSS);
            }
            else
            {
                EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.UIPortalArrowControllerInitializingArrows,
                     this.generator.wholeRoomList[objIndex].transform);
            }
        }
    }

    private IEnumerator CoStartAnnounce()
    {
        yield return new WaitForSeconds(0.5f);
        EventDispatcher.Instance.Dispatch<UIAnnounceDirector.eAnnounceType>(EventDispatcher.EventName.UIAnnounceDirectorStartAnnounce,
            UIAnnounceDirector.eAnnounceType.DUNGEON);
    }

    private void MakePortalEffects(Vector2 originPos, GameObject nextRoom, string portalName)
    {
        GameObject effect = null;
        if (nextRoom.name.Contains("Safe"))
        {
            effect = this.portalEffectList.Find(x => x.name.Contains("SafeHouse"));
        }
        else if (nextRoom.name.Contains("Hidden"))
        {
            effect = this.portalEffectList.Find(x => x.name.Contains("Hidden"));
        }
        else if (nextRoom.name.Contains("LHK"))
        {
            effect = this.portalEffectList.Find(x => x.name.Contains("Boss"));
        }
        else
        {
            effect = this.portalEffectList.Find(x => x.name.Contains("Normal"));
        }

        if (effect == null)
        {
            Debug.LogWarning("Portal effect not found");
            return;
        }

        var go = Instantiate(effect, originPos, Quaternion.identity);
        var comp = go.GetComponent<ParticleSystem>();
        var main = comp.main;

        switch (portalName)
        {
            case "portal0":
                main.startRotationZ = Mathf.PI; 
                go.transform.position = new Vector3(originPos.x, originPos.y - 2, 1);
                break;
            case "portal1":
                main.startRotationZ = -Mathf.PI / 2; 
                go.transform.position = new Vector3(originPos.x - 2, originPos.y, 1);
                break;
            case "portal3":
                main.startRotationZ = Mathf.PI / 2; 
                go.transform.position = new Vector3(originPos.x + 2, originPos.y, 1);
                break;
            case "portal2":
                go.transform.position = new Vector3(originPos.x, originPos.y + 2, 1);
                break;
            default:
                Debug.LogWarning("Invalid portal name");
                break;
        }
        Destroy(go, 2f);
    }
}
