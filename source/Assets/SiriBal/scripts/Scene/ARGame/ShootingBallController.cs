using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBallController : MonoBehaviour
{
    public GameObject ShootingBallPrefab;
    public float ShootingForce = 200.0f;
	GameModeController gameMode;
    TouchTools touch;

    // Start is called before the first frame update
    void Start()
    {
        touch = GameObject.Find("GameDirector").GetComponent<TouchTools>();
        gameMode = GameObject.Find ("ModeSwitcher").GetComponent<GameModeController>();
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
        GameObject ShootingBall = Instantiate(ShootingBallPrefab) as GameObject;
        Ray ray = Camera.main.ScreenPointToRay(position);
        ShootingBall.transform.position = ray.origin;
        ShootingBall.GetComponent<ShootingBallWatcher>().Shoot(ray.direction.normalized * ShootingForce);
    }
}