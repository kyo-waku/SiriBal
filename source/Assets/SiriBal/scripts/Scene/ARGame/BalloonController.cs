using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class BalloonController : MonoBehaviour {

	public Transform cameraTransform;
	public GameObject balloonPrefab;
	//public float createHeight;
	//public float maxRayDistance = 30.0f;
    //ublic float CreateBalloonDistance = 15.0f;
    //public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer
	GameModeController gameMode;
	TouchTools touch;
	GameObject GameDirector;
	
	// Use this for initialization
	void Start () {
		touch = GameObject.Find("GameDirector").GetComponent<TouchTools>();
        gameMode = GameObject.Find ("ModeSwitcher").GetComponent<GameModeController>();
		this.GameDirector = GameObject.Find("GameDirector");
	}

	void CreateBalloon(Vector3 atPosition)
	{
		GameObject balloonGO = Instantiate (balloonPrefab, atPosition, Quaternion.identity);
	}

	// Update is called once per frame
	void Update () {
		// 参照しか渡さないならStartで1回だけ取得してもいいけど、わからないのでとりあえずここで毎回取る
		cameraTransform = GameObject.Find("MainCamera").transform;

		// Balloon Mode
		if(GameModeController.eGameMode.Balloon != gameMode.GameMode)
		{
			return;
		}

		if (touch.touchPhaseEx == TouchTools.TouchPhaseExtended.Began)
		{
			GameDirector.GetComponent<GameDirector>().BalloonCounter += 1;
			//Debug.Log(GameDirector.GetComponent<GameDirector>().BalloonCounter);
		#if UNITY_EDITOR 

			CreateBalloon (new Vector3 (cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z+15.0f));
		
		#else
			var screenPosition = Camera.main.ScreenToViewportPoint(touch.touchPosition);
			ARPoint point = new ARPoint
			{
				x = screenPosition.x,
				y = screenPosition.y
			};

			// スクリーンの座標をWorld座標に変換
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, ARHitTestResultType.ARHitTestResultTypeFeaturePoint);
			if (hitResults.Count > 0)
			{
				foreach (var hitResult in hitResults)
				{
					Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
					CreateBalloon(new Vector3(position.x, position.y, position.z));
					break;
				}
			}
			
		#endif

		}
    }

}
