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
1. UI
2. EventDispacher
3. Chest,DropItem
4. Boss
5. DataTable
6. DataManager
7. InfoManager
8. CoroutineTool
9. Encryption

##  1. UI
### 🛒 ShopUI
- 상점에는 '마을상점' 과 '던전상점' 으로 나뉘게됩니다. '마을상점' 과 '던전상점'은 서로 다른 기능을 가지고 있습니다. <br>
- 유저는 매번 다른 아이템을 구입 할 수 있도록 제작하였습니다. 추상팩토리 패턴을 이용하여 장비마다 다른 속성을 부여한 후, 상점 초기화 시점에서 매번 다른 장비를 생성하도록 제작하였습니다. <br>
- 마을상점에서 유저는 전투력 향상을 위한 '장비'를 구매할 수 있고, '인벤토리 용량'을 증가시킬 수 있습니다. <br>
- 던전상점에서 유저는 전투력 향상을 위한 '장비'를 구매할 수 있고, 체력 회복을 위한 '소모아이템'을 구매할 수 있습니다. <br>
- 각 씬의 메인스크립트인 SanctuaryMain와 DungeonMain은 각각 UISanctuaryDirector와 UIDungeonDirector을 포함하고 있습니다. <br>
- '마을상점' 과 '던전상점'은 중복되는 기능(장비 구매기능)이 있음으로 UIShopDirector에서 enum타입으로 SANCTUARY과 DUNGEON을 정의 하고, SanctuaryShopInit와 DungeonShopInit 메서드를 만들어 각각의 다른 기능(인벤토리 용량 증가, 소모아이템 구매)으로 동작하도록 설계하였습니다. <br>
- UI의 구조는 Canvas를 가장 최상단인 Director로 정의한 후 하위 목록인 UIShop->UIShopGride->UIShopPopup로 설계하였습니다. <br>
- UI는 UGUI를 사용하여 구성하였습니다 <br>

 #### 📄 scripts
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 UIShopDirector <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 UIShop <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 UIShopGride <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 UIShopPopup <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 UIShopGoods <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 UIShopPoupInvenVolume <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AbsShopFactory <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 ShopFactory <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 ShopSwordFactory <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 ShopAxeFactory <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 ShopArrowFactory <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 ShopWandFactory <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 ShopFoodFactory <br>
 
 ### ◻️ DiceUI
 - 던전 씬에서 유저는 '암상인' NPC를 통해 랜덤 주사위 시스템을 이용할 수 있습니다. <br>
 - 유저의 해당 스테이지에 따라 랜덤 주사위 이용 가격이 다르게 정해지며, 현재 스테이지는 InfoManager 클래스의 DungeonInfo에서 참조하고 있습니다. <br>
 - 'Go' 버튼을 클릭 시 유저의 현재 보유 재화와 인벤토리 여유공간을 체크 한 후 주사위가 돌아가게됩니다. 만약 보유재화가 부족하거나 인벤토리 용량이 가득 차있다면 코루틴으로 버튼 비활성을 제어하도록 제작하였습니다. <br>
 - 주사위는 툰쉐이더를 이용하여 3D오브잭트가 2D공간에서 보여질 수 있고, 랜더텍스쳐를 이용하여 UI공간에서만 보여지게 제작했습니다. <br>
 - 주사위는 매번 다른 모습으로 회전하는 연출을 보여주기 위해 랜덤한 힘을 주고, x,y,z축 모두 랜덤한 힘을 받기 위하여 AddTorque를 사용하여 연출을 표현했습니다. <br>
 - 랜덤 주사위의 결과는 총 4가지 등급의 아이템으로 Wood,Iron,Gold,Diamond 등급입니다.<br>
 - 랜덤 주사위의 result연출을 표현하기 위하여 DoTween을 이용하여 연출을 표현했습니다. <br>
 - Diamond 등급일 때 파티클을 이용하여 연출을 표현했습니다. <br>

 #### 📄 scripts  
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 UIDiceDirector <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 UIDice <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 DiceGoods <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 UIdiceResultPopup <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 DiceScript <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 phong1SG_2D <br>

 ### ◻️ BossHealthBarUI
 - Boss의 체력을 게임 씬에서 보여주는 UI입니다. <br>
 - UGUI의 Slide를 이용하여 제작하였습니다. <br>
 - UIBossHealthBar는 현재 스테이즈 Boss의 hp를 참조하고 있습니다. <br>
 - 유저가 Boss룸으로 입장할때 Boss의 HealthBar가 0~Max까지 Lerp하게 채워지는 연출을 위해 코루틴으로 제어하였습니다. <br>

 #### 📄 scripts
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 UIBossHealthBar <br>

## 2. EventDispatcher
 - 게임 내에서 발생하는 모든 이벤트를 관리하는 이벤트 디스패치 방식입니다.<br>
 - 모든 객체들에서 참조하기 위하여 싱글톤으로 제작하였습니다.<br>
 - 이벤트를 등록하고(AddListener) 호출하는(Dispatch) 메서드를 정의하고 반환값에는 모든타입을 사용 할 수있도록 제네릭<T>으로 정의하였습니다.<br>
 - 등록된 이벤트는 enum 타입으로 정의하였습니다. <br>

 #### 📄 scripts
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 EventDispatcher <br>
 

 
## 3. Chest, DropItem
 - 모든 씬에서 생성되는 Chest와 Item(장비)를 관리하는 오브잭트 입니다. <br>
 - Chest는 유저가 던전 룸을 클리어 하게되면 얻을 수 있는 드랍 상자입니다. <br>
 - 유저가 던전 룸을 클리어 한다면 이벤트를 호출한 후 Chest를 생성하도록 제작하였습니다. <br>
 - Item은 Chest에서 드랍되는 드랍아이템 입니다.(장비, 재화, 무기, 보조무기, 소모아이템)<br>
 - Chest를 중심으로 360도 방향으로 생성됩니다. 유저는 Item을 획득 하기위해 OnTriggerEnter2D를 이용하거나 UniRx 플러그인을 이용한 터치를 이용하여 획득할 수 있습니다. <br>
 - Item은 각각 다른 포지션을 갖고 있으며 서로 곂치거나 너무 좁은 간격으로 생성되지 않도록 Item마다 최소간격을 정의한 후 Physics2D.OverlapCircleAll를 이용하여 간격을 넓혀 주었습니다. 만약 겹치거나 간격이 충분 하지 않다면 다시 위치를 찾고 지정합니다. <br>
 - SpriteGlow 효과를 표현하기 위하여 AllIn

 #### 📄 scripts
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 ChestItemGenerator<br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 DropItem <br>
 


## 4. Boss<br>
 - 스테이지의 가장 마지가 룸에는 Boss룸으로 지정되어있습니다.<br>
 - 각 스테이지마다 출연하는 보스다 다르고, 각 보스마다 다른 공격패턴과 체력, 이동속도를 가지고 있습니다.<br>
 - 보스 마다 다른 공격패턴을 지니게 하기위해 전략패턴을 사용하여 제작하였습니다. <br>
 - 보스의 투사체인 Bullet은 최적화를 위하여 오브잭트 풀링을 이용하였습니다. <br>
 - 보스는 체력의 정도에 따라 행동 Phase가 변경되며 각 Phase는 코루틴으로 정의 한 후 공격패턴 보일 수 있습니다. <br>
 - 보스는 모두 특유의 애니메이션을 갖고 있음으로, AnimationEvent를 이용하여 해당 프레임에 공격 패턴이 동작하고 있습니다. <br>
 - 추상클래스를 모든 보스에게 상속 시킨 후 공통되는 기능들을 정의하였습니다. 만약 구현내용이 변경될 수 있는 메서드라면 abstract, 구현내용이 변경될 일이 없다면 virtual/override로 메서드를 정의하였습니다.<br>
 - 체력이 0 이하가 된 보스는 Chest를 출현시켜야 함으로 EventDispatcer에 정의된 Event발생을 통하여 ChestItemGenerator 스크립트에게 Event를 전달하고있습니다.<br>

 #### 📄 scripts
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 FireWorm <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 EvilWizard <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 DarkWizard <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 Slime <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 Demon <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AbsBossAttackPattern <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AbsBossClass <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AttackPattern01 <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AttackPattern02 <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AttackPattern03 <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AttackPattern04 <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AttackPattern05 <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AttackPattern06 <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AttackPattern07 <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AttackPattern08 <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AttackPattern09 <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 AttackPattern14 <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 BulletPool <br>
 
 ## 5. DataTable<br>
 - 게임 내에서 사용되는 DataTable 입니다. <br>
 - chest_data는 스테이지와 난이도마다 출현하는 상자의 종류, 드랍되는 장비의 갯수, 드랍 재화양에 대한 data입니다. <br>
 - shop_data는 상점에서 출현하는 장비의 이름, 등급, spritename, 타입, 스팩에 대한 data입니다. <br>
 - gamble_data는 해당 스테이지마다 다른 가격에대한 data입니다. <br>

 #### 📄 scripts
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 chest_data <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 shop_data <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 gamble_data <br>
 
 ## 6. DataManager<br>
 - DataManager는 게임 내에서 변하지 않는 데이터(chest_data,shop_data,gamble_data)에 대한 스크립트입니다.<br>
 - 외부의 스크립트에서 접근하기 위하여 싱글톤으로 제작되어있고, Load할때에는 JsonConvert.DeserializeObject를 이용하여 Json 데이터를 역직렬화 하고 불러오는 방식입니다.<br>
 - 사용되는 data의 종류가 많음으로 partial 클래스를 사용하여 제작되었습니다. <br>

 #### 📄 scripts
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 DataManager_Chest <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 DataManager_gamble <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 DataManager_shop(LHKDataManager_Partial) <br>

 ## 7. InfoManager<br>
 - InfoManager는 게임 내에서 변하는 데이터(현재 스테이지, 게임 설정값, 플레이어의 스텟 등)에 대한 스크립트 입니다.<br>
 - DataManager와 동일하게 싱글톤으로 제작하였고 Save할때는 JsonConvert.SerializeObject를 이용하여 직렬화, Load할때는 JsonConvert.DeserializeObject를 이용한 역직렬화 방식입니다. <br>

 #### 📄 scripts
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 InfoManager <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 InfoManager_Player <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 InfoManager_PossessionAmount <br>
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 InfoManager_Setting <br>
 ___

## 8. 🧰 Coroutine Tool
- 씬에서 동작중인 모든 코루틴을 조회하는 툴입니다.
- 해당 코루틴 버튼을 클릭하면 코루틴을 정의하고 실행하고있는 스크립트를 보여줄 수 있게 제작하였습니다.
- 게임이 실행 중일때 코루틴을 조회하고있다면 프레임드랍에 큰 영향을 미치는 버그가 있었습니다.
- 실행 중 계속 코루틴을 조회하는 것이 아닌 씬 전환 시 StartCoroutine키워드를 검색하고 목록을 보여주는 방법으로 버그를 개선했습니다.
- 씬이 전환될때에도 hierachy에 모든 StartCoroutine 키워드를 검색한다면 프레임 드랍에 영향이 있을 수 있었음으로 Refesh버튼을 클릭하면 조회하는 방식으로 개선했습니다.
- 자세한 사용법은 (https://github.com/dlghksrnr/CoroutineTrackerWindow) 에 README를 참고해주세요.

 #### 📄 scripts
 &nbsp;&nbsp;&nbsp;&nbsp; 🔴 CoroutineTrackerWindow <br>
___

## 9. 🔑 Encryption
- 게임 내에서 Save/Load 되는 데이터를 암호화/복호화 작업을 했습니다. <br>
- AES 대칭키 알고리즘을 이용하여 저장되어 있는 Info를 Load 할때에는 복호합니다<br>
- Info를 Sava 할때에는 암호화를 합니다<br>

#### 📄 scripts
&nbsp;&nbsp;&nbsp;&nbsp; 🔴 PlayerPrefsEncryption <br>

___
## 발매작
![GraphicImage](https://github.com/dlghksrnr/Lee-Hwanguk-GameClient-Portfolio/assets/124248051/78bfa49e-fdaa-44d1-8ca9-11d6c8234d19)

- 타이틀 : GunsN'Rachels(건즈 앤 레이첼스)<br>
- 장르 : Log-like, Hack and Slash, Shooting game<br>
- 앤진 : UnityEngine3D<br>
- 플랫폼 : Android, iOS<br>
- 출시일 : 2023. 6. 7 (Android)  2023. 6. 16 (iOS)<br>
- 제작 : Team Vizeon<br>
- 홍보영상 : https://www.youtube.com/watch?v=uf8yAuG5YM0
- 플레이 영상 : https://www.youtube.com/watch?v=wNlCI4cgI1s
___
## 다운로드 링크
⬇️ Download in Google PlayStore, AppStore <br>

:iphone: iOS : [AppStore Link][iOS Link]

[iOS Link]: https://apps.apple.com/kr/app/%EA%B1%B4%EC%A6%88%EC%95%A4%EB%A0%88%EC%9D%B4%EC%B2%BC%EC%8A%A4/id6450149470

:iphone: Android : [Google PlayStore Link][GooglePlayStore Link]

[GooglePlayStore Link]: https://play.google.com/store/apps/details?id=com.teamvizeon.gunsandrachels&hl=ko

___





