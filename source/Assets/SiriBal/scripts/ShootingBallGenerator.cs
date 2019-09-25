using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBallGenerator : MonoBehaviour
{
    public GameObject ShootingBallPrefab;
    public float ShootingForce = 200.0f;
	ControlGameMode controlGameMode;
    CommonTools tools;

    // Start is called before the first frame update
    void Start()
    {
        tools = GameObject.Find("GameDirector").GetComponent<CommonTools>();
        controlGameMode = GameObject.Find ("ModeSwitcher").GetComponent<ControlGameMode>();
    }

    // Update is called once per frame
    void Update()
    {
        // Shooting Mode
		if(ControlGameMode.eGameMode.Shooting != controlGameMode.GameMode)
		{
			return;
		}

        // TouchPhase Begau 
        if (tools.touchPhaseEx == CommonTools.TouchPhaseExtended.Began)
        {
            //ボールを飛ばすよ
            #if UNITY_EDITOR

            GameObject ShootingBall = Instantiate(ShootingBallPrefab) as GameObject;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ShootingBall.transform.position = ray.origin;
            ShootingBall.GetComponent<ShootingBallConrtoller>().Shoot(ray.direction.normalized * ShootingForce);

            #else
            
            GameObject ShootingBall = Instantiate(ShootingBallPrefab) as GameObject;
            Ray ray = Camera.main.ScreenPointToRay(tools.touchPosition);
            ShootingBall.transform.position = ray.origin;
            ShootingBall.GetComponent<ShootingBallConrtoller>().Shoot(ray.direction.normalized * ShootingForce);
            
            #endif
        }
        
    }
}