using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;
using Generic.Manager;

public class ShootingBallController : MonoBehaviour
{
    private GameObject GameDirector;
    public float ShootingForce = 200.0f;
	GameModeController gameMode;
    TouchTools touch;
    WeaponHolder weaponHolder;
    public int ShootingMode;
    List<Weapons> availableWeapons; //武器管理の参照先変更により削除してもよい？
    int currentWeaponIndex;

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
        currentWeaponIndex = 0;
        weaponHolder = GameObject.Find("WeaponHolder").GetComponent<WeaponHolder>();
    }

    // Update is called once per frame
    void Update()
    {
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

    void Shooting(Vector3 position)
    {
        //var shootingPrefab = weaponHolder.GetWeaponObjectByKey(availableWeapons[currentWeaponIndex]);
        HeroWeaponStatus currentweapon = WeaponData2.Entity.HeroWeaponList[currentWeaponIndex];
        var shootingPrefab = currentweapon.WeaponPrefab;
        if (shootingPrefab == null){return;}

        GameObject ShootingObj = Instantiate(shootingPrefab) as GameObject;
        Ray ray = Camera.main.ScreenPointToRay(position);
        ShootingObj.transform.position = ray.origin + ray.direction.normalized ;
        ShootingObj.GetComponent<ShootingWatcher>().Shoot(ray.direction.normalized * ShootingForce);
    
        //投げ回数のカウント
        GameDirector.GetComponent<GameDirector>().ThrowCounter += 1;
    }

    public void ShootingModeButtonClicked()
    {
        int weaponcount = WeaponData2.Entity.HeroWeaponList.Count;
        ++currentWeaponIndex;
        //if (currentWeaponIndex > availableWeapons.Count - 1)
        if (currentWeaponIndex > weaponcount - 1)
        {
            currentWeaponIndex = 0;
        }
    }
}