using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;
using Generic.Manager;

public class ShootingBallController : MonoBehaviour
{
    public GameObject ShootingBallPrefab;
    public GameObject ShootingColaCanPrefab;
    public GameObject ShootingShoesPrefab;
    public GameObject ShootingMacePrefab;
    private GameObject GameDirector;
    public float ShootingForce = 200.0f;
	GameModeController gameMode;
    TouchTools touch;
    WeaponHolder weaponHolder;
    public int ShootingMode;
    List<Weapons> availableWeapons;
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
        stage.GetRegisteredShootingWeapons(out availableWeapons);
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
            //ボールを飛ばすよ
            #if UNITY_EDITOR
            Shooting(Input.mousePosition);
            #else
            Shooting(touch.touchStartPosition);
            #endif
        }
        
    }

    void Shooting(Vector3 position)
    {
        var shootingPrefab = weaponHolder.GetWeaponObjectByKey(availableWeapons[currentWeaponIndex]);
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
        ++currentWeaponIndex;
        if (currentWeaponIndex > availableWeapons.Count - 1)
        {
            currentWeaponIndex = 0;
        }
    }
}