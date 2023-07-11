<img src="https://img.shields.io/badge/Game%20Client%20Lee%20Hwanguk-8A2BE2" width="200" height="30">

- 유니티엔진 클라이언트 개발자 지원자 이환국의 포트폴리오 입니다.
- 게임 클라이언트 개발자 지망생입니다. 2022.12 ~2023.6 까지 직업 훈련을 받았습니다.
- 출시 작품에는 유니티엔진을 사용하여 개발한 "건즈 앤 레이첼스" 가 있습니다.
- 개발 단계에는 기획(컨셉,시스템,레벨기획) -> 개발(UI,Boss,암호화,코루틴제어툴) -> 퀄리티업(포스트프로세싱을 이용한 Bloom효과, 도트 리소스를 이용한 톤앤 매너정리, 파티클 효과) 순으로 진행하였습니다.<br>

  
- 개발자 블로그입니다 https://dlghksrnr.tistory.com/

***
📄 기획

- 게임 컨셉 기획 <br>
1. 게임 테마, 컨셉, 장르를 기획하고 시장조사를 진행했습니다. <br>
2. 레퍼런스 게임을 분석하고 조작방식과 간략한 시스템 기획을 진행했습니다. <br>
3. 게임의 코어사이클을 기획하고 유저의 진입단계와 몰입단계를 기획했습니다. <br>

- 시스템 기획  <br>
1. 유저의 성장 시스템 기획했습니다. <br>
2. 게임 재화 시스템을 기획했습니다. <br>
3. 맵 시스템을 기획했습니다. <br>

- 레벨 기획  <br>
1. 유저의 플레이타임을 정하고, 스테이지 수치벨런스(스테이지 진행 별로 몬스터들의 체력, 등장 수, 방어률, 넉백저항)을 기획했습니다.<br>
2. 플레이어의 능력치(장비 획득과 능력치 향상에 따른 플레이어 전투력상승치)를 기획했습니다. <br>
3. 게임에서 사용될 데이터를 정리한 데이터 테이블을 제작하였습니다. <br>
***
📄 개발에 참여한 목록입니다.

## UGUI
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: Sanctuary Shop UI <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: Dungeon Shop UI <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: Dice UI <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: BossHealthBar UI <br>

## Boss<br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: 1StageBoss <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: 2StageBoss <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: 3StageBoss <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: 4StageBoss <br>
 
## DataManager<br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: DataManager_Chest <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: DataManager_gamble <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: DataManager_shop(LHKDataManager_Partial) <br>

## DataTable<br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: chest_data <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: shop_data <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: gamble_data <br>

## InfoManager<br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: InfoManager <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: InfoManager_Player <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: InfoManager_PossessionAmount <br>
 &nbsp;&nbsp;&nbsp;&nbsp; :heavy_check_mark: InfoManager_Setting <br>
 ___

🧰 코루틴 조회 툴 개발
- 씬에서 동작중인 모든 코루틴을 조회하는 툴입니다.
- 해당 코루틴 버튼을 클릭하면 코루틴을 정의하고 실행하고있는 스크립트를 보여줄 수 있게 제작하였습니다.
- 게임이 실행 중일때 코루틴을 조회하고있다면 프레임드랍에 큰 영향을 미치는 버그가 있었습니다.
- 실행 중 계속 코루틴을 조회하는 것이 아닌 씬 전환 시 StartCoroutine키워드를 검색하고 목록을 보여주는 방법으로 버그를 개선했습니다.
- 씬이 전환될때에도 hierachy에 모든 StartCoroutine 키워드를 검색한다면 프레임 드랍에 영향이 있을 수 있었음으로 Refesh버튼을 클릭하면 조회하는 방식으로 개선했습니다.
- 자세한 사용법은 (https://github.com/dlghksrnr/CoroutineTrackerWindow) 에 README를 참고해주세요.
___

🔑 암호화/복호화 작업

___
⬆️ 퀄리티 업 작업

___
<img src="https://img.shields.io/badge/Release-F2BB13?style=flat&logo=gamedeveloper&logoColor=white">

![GraphicImage](https://github.com/dlghksrnr/Lee-Hwanguk-GameClient-Portfolio/assets/124248051/78bfa49e-fdaa-44d1-8ca9-11d6c8234d19)

- Title : GunsN'Rachels(건즈 앤 레이첼스)<br>
- Genre : Log-like, Hack and Slash, Shooting game<br>
- Engine : UnityEngine3D<br>
- Platform : Android, iOS<br>
- Release Date : 2023. 6. 7 (Android)  2023. 6. 16 (iOS)<br>
- Producer : Team Vizeon<br>

___
<img src="https://img.shields.io/badge/Download-F2BB13?style=flat&logo=gamedeveloper&logoColor=white">
⬇️ Download in Google PlayStore, AppStore <br>

:iphone: iOS : [AppStore Link][iOS Link]

[iOS Link]: https://apps.apple.com/kr/app/%EA%B1%B4%EC%A6%88%EC%95%A4%EB%A0%88%EC%9D%B4%EC%B2%BC%EC%8A%A4/id6450149470

:iphone: Android : [Google PlayStore Link][GooglePlayStore Link]

[GooglePlayStore Link]: https://play.google.com/store/apps/details?id=com.teamvizeon.gunsandrachels&hl=ko

___





