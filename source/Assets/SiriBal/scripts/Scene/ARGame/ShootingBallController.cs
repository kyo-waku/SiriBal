using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Generic;
using Generic.Manager;

public class ShootingBallController : MonoBehaviour
{
    private GameObject GameDirector;
    public float ShootingForce = 200.0f;
	GameModeController gameMode;
    TouchTools touch;
    public int ShootingMode;
    List<Weapons> availableWeapons; //武器管理の参照先変更により削除してもよい？
    WeaponIds currentWeaponId;
    float ShootingIntervalTimer;

    // Start is called before the first frame update
    void Start()
    {
        touch = GameObject.Find("GameDirector").GetComponent<TouchTools>();
        gameMode = GameObject.Find ("ModeSwitcher").GetComponent<GameModeController>();
        ShootingMode=0;
        GameDirector = GameObject.Find("GameDirector");
        var stage = GameDirector.GetComponent<GameDirector>().stage;
        if( stage == null)
        {
            stage = DataManager.currentStage;
            if(stage == null)
            {
                stage = new Stage();
            }
        }
        stage.GetRegisteredShootingWeapons(out availableWeapons);   //武器管理の参照先変更により削除してもよい？
        currentWeaponId = WeaponIds.Stone; // 初期値
        ShootingIntervalTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 投げ間隔の計算用
        ShootingIntervalTimer += Time.deltaTime;
        // Shooting Mode
		if(GameModeController.eGameMode.Shooting != gameMode.GameMode)
		{
			return;
		}

        // TouchPhase Began
        if (touch.touchPhaseEx == TouchTools.TouchPhaseExtended.Began)
        {
            if (GameDirector.GetComponent<GameDirector>().NyusanPoint > 0)
            {
                //ボールを飛ばすよ
                #if UNITY_EDITOR
                Shooting(Input.mousePosition);
                #else
                Shooting(touch.touchStartPosition);
                #endif
            }
        }
    }

    // 連射性能を時間間隔へ変換する。定義はここ次第
    // rapidFireValue = 5 : 0.3 sec 間隔
    // ~ rapidFireValue = 1 : 1.1 sec 間隔
    float ShootingInterval(int rapidFireValue)
    {
        // ベタ書き（計算式に基づかない）が、実はこれが一番わかり易いと思う。
        switch(rapidFireValue)
        {
            case 1 : // 超遅い
                return 1f;
            case 2 : // まぁまぁ遅い
                return 0.7f;
            case 3 : // 普通
                return 0.5f;
            case 4 : // まぁまぁ早い
                return 0.3f;
            case 5 : // 爆速
            default :
                return 0.1f;
        }
    }

    void Shooting(Vector3 position)
    {
        //var shootingPrefab = weaponHolder.GetWeaponObjectByKey(availableWeapons[currentWeaponIndex]);
        HeroWeaponStatus currentWeapon = WeaponData.Entity.HeroWeaponList.Where(x => x.WeaponID == currentWeaponId).First();
        if (currentWeapon == null)
        {
            currentWeapon = WeaponData.Entity.HeroWeaponList[0];
        }
        if (ShootingIntervalTimer > ShootingInterval(currentWeapon.Rapidfire))
        {
            var shootingPrefab = currentWeapon.WeaponPrefab;
            if (shootingPrefab == null){return;}

            GameObject ShootingObj = Instantiate(shootingPrefab) as GameObject;
            // ウェポンのステータスをGameObjectに挿入する
            ShootingObj.AddComponent<WeaponProperties>();
            ShootingObj.GetComponent<WeaponProperties>().Initialize(currentWeapon);
            // 投げるためのForce計算
            Ray ray = Camera.main.ScreenPointToRay(position);
            ShootingObj.transform.position = ray.origin + ray.direction.normalized ;
            ShootingObj.GetComponent<ShootingWatcher>().Shoot(ray.direction.normalized * ShootingForce);
            // 投げ回数のカウント
            GameDirector.GetComponent<GameDirector>().ThrowCounter += 1;
            // 投げインターバルの初期化
            ShootingIntervalTimer = 0;
        }
    }

    public void ShootingModeButtonClicked()
    {
        var maxCount = WeaponData.Entity.HeroWeaponList.Count;
        currentWeaponId = NextAvailableWeaponId(currentWeaponId);
    }

    public WeaponIds NextAvailableWeaponId(WeaponIds currentId)
    {
        var availableWeaponIds = new List<WeaponIds>();
        foreach(var weapon in WeaponData.Entity.HeroWeaponList)
        {
            if(weapon.IsWeaponAcquired)
            {
                availableWeaponIds.Add(weapon.WeaponID);
            }
        }

        var nextId = currentId;
        // 複数個見つかった
        if(availableWeaponIds.Count > 1)
        {
            availableWeaponIds.Sort();
            for(var index = 0; index < availableWeaponIds.Count; index++)
            {
                if(availableWeaponIds[index] == currentId)
                {
                    nextId = (index + 1 == availableWeaponIds.Count)? availableWeaponIds[0]: availableWeaponIds[index + 1];
                }
            }
        }
        return nextId;
    }
}