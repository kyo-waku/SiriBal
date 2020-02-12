using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic.Manager;
using Weapon;
using UnityEngine.XR.iOS;


public class WeaponChallengeDirector : MonoBehaviour
{
    // members
    [SerializeField]
    private GameObject balloonPrefab;
    private TouchTools touch;

    // shooting objects

    [SerializeField]
    private GameObject weapon1;

    // consts
    private const float ShootingForce = 500.0f;

    // game defines
    private Dictionary<int, Stage> stages; 
    private GameObject shootingPrefab;
    private GameObject mainCamera;

    private bool balloonPlaced = false;

    // Basic methods
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        touch = this.gameObject.GetComponent<TouchTools>();
    }
    void Update()
    {
        if (balloonPlaced)
        {
            if (touch.touchPhaseEx == TouchTools.TouchPhaseExtended.Began)
            {
                ShootingBall(touch.touchStartPosition);
            }
        }
        else
        {
            if (touch.touchPhaseEx == TouchTools.TouchPhaseExtended.Began)
            {
                InitializeStages(touchToARHitWorld(touch.touchStartPosition));
                
                if (GameSceneManager.WeaponChallengeLevelNum == 0)
                {
                    CreateStage(1);
                }
                else
                {
                    CreateStage(GameSceneManager.WeaponChallengeLevelNum);
                }

                balloonPlaced = true;
            }
        }
    }

    // Stage methods
    void InitializeStages(Vector3 position)
    {
        stages = new Dictionary<int, Stage>();
        // 1st stage
        var stage = new Stage(3, 30, 10, 1, "10投以内にバルーンをすべて落とそう");
        var pos = mainCamera.transform.position;
        stage.registerBalloonPositions(new Vector3(pos.x, pos.y, pos.z+3));
        stage.registerBalloonPositions(new Vector3(pos.x+1, pos.y, pos.z+3));
        stage.registerBalloonPositions(new Vector3(pos.x-1, pos.y, pos.z+3));
        stages.Add(1, stage);
        // 2nd stage
        // hogehoge~~
    }
    void CreateStage(int stageNumber)
    {
        stages.TryGetValue(stageNumber, out var stage);
        // Set Balloons
        for(var i = 0 ; i < stage.BalloonLimit; ++i)
        {
            CreateBalloon(stage.BalloonPositions[i]);
        }
        // Set Properties
        SetPrefab(stage.ShootItemID);
    }

    void SetPrefab(int id)
    {
        switch(id)
        {
            case 1:
                shootingPrefab = weapon1;
                break;
            default:
                shootingPrefab = weapon1;
                break;
        }
    }
    // Common methods

    GameObject CreateBalloon(Vector3 atPosition)
	{
		return Instantiate (balloonPrefab, atPosition, Quaternion.identity);
	}

    Vector3 touchToARHitWorld(Vector3 touchPosition){
		var viewPortPosition = Camera.main.ScreenToViewportPoint(touchPosition);
		ARPoint point = new ARPoint
		{
			x = viewPortPosition.x,
			y = viewPortPosition.y
		};
		// スクリーンの座標をWorld座標に変換
		List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface()
											.HitTest(point, ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
		if (hitResults.Count > 0)
		{
			foreach (var hitResult in hitResults)
			{
				return UnityARMatrixOps.GetPosition(hitResult.worldTransform);
			}
		}
		return new Vector3();
	}
    
    void ShootingBall(Vector3 position)
    {
        if (shootingPrefab == null) return;
        GameObject ShootingBall = Instantiate(shootingPrefab) as GameObject;
        Ray ray = Camera.main.ScreenPointToRay(position);
        ShootingBall.transform.position = ray.origin;
        ShootingBall.GetComponent<ShootingBallWatcher>().Shoot(ray.direction.normalized * ShootingForce);
    }
}

