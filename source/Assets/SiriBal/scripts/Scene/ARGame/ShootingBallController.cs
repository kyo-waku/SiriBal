using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBallController : MonoBehaviour
{
    public GameObject ShootingBallPrefab;
    public GameObject ShootingMacePrefab;
    private GameObject GameDirector;
    public float ShootingForce = 200.0f;
	GameModeController gameMode;
    TouchTools touch;
    public int ShootingMode;

    // Start is called before the first frame update
    void Start()
    {
        touch = GameObject.Find("GameDirector").GetComponent<TouchTools>();
        gameMode = GameObject.Find ("ModeSwitcher").GetComponent<GameModeController>();
        ShootingMode=0;
        this.GameDirector = GameObject.Find("GameDirector");
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
            ShootingBall(Input.mousePosition);
            #else
            ShootingBall(touch.touchPosition);
            #endif
        }
        
    }

    void ShootingBall(Vector3 position)
    {
        if(ShootingMode==0){
            GameObject ShootingBall = Instantiate(ShootingBallPrefab) as GameObject;
            Ray ray = Camera.main.ScreenPointToRay(position);
            ShootingBall.transform.position = ray.origin;
            ShootingBall.GetComponent<ShootingBallWatcher>().Shoot(ray.direction.normalized * ShootingForce);
        }
        else if(ShootingMode==1){
            GameObject ShootingMace = Instantiate(ShootingMacePrefab) as GameObject;
            Ray ray = Camera.main.ScreenPointToRay(position);
            ShootingMace.transform.position = ray.origin;
            ShootingMace.GetComponent<ShootingMaceWatcher>().Shoot(ray.direction.normalized * ShootingForce);
        }
        //投げ回数のカウント
        GameDirector.GetComponent<GameDirector>().ThrowCounter += 1;
    }

    public void ShootingModeButtonClicked()
    {
        
        switch(ShootingMode)
        {
            case 0:
                ShootingMode=1;
                break;
            case 1:
                ShootingMode=0;
                break;
            default:
                ShootingMode=0;
                break;
        }
    }
}