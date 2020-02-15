using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;

public class BalloonController : MonoBehaviour {

	Transform cameraTransform;
	
	[SerializeField]
	GameObject balloonPrefab;
	GameModeController gameMode;
	TouchTools touch;
	GameObject GameDirector;
	GameObject mainCamera;
	GameObject lastCreatedBalloon;
	Vector3 lastCreatedVector;
	Vector3 lastCreatedBalloonPosition;

	// Use this for initialization
	void Start () {
		touch = GameObject.Find("GameDirector").GetComponent<TouchTools>();
        gameMode = GameObject.Find ("ModeSwitcher").GetComponent<GameModeController>();
		GameDirector = GameObject.Find("GameDirector");
		mainCamera = GameObject.Find("MainCamera");
		cameraTransform = mainCamera.transform;
	}

	// atPosition座標にバルーンを置く
	GameObject CreateBalloon(Vector3 atPosition)
	{
		GameDirector.GetComponent<GameDirector>().BalloonCounter += 1;
		
		// バルーンのステータスをStage情報から取得する
		balloonPrefab.gameObject.GetComponent<AirBalloonWatcher>().BreakCount = GameDirector.GetComponent<GameDirector>().stage.BalloonHP;
		balloonPrefab.gameObject.GetComponent<AirBalloonWatcher>().isAction = GameDirector.GetComponent<GameDirector>().stage.IsBalloonAction;
		return Instantiate (balloonPrefab, atPosition, Quaternion.identity);
	}

	// Update is called once per frame
	void Update () {
		cameraTransform = mainCamera.transform;

		// Balloon Mode でないなら処理不要
		if(GameModeController.eGameMode.Balloon != gameMode.GameMode)
		{
			return;
		}

		// タッチ開始
		if (touch.touchPhaseEx == TouchTools.TouchPhaseExtended.Began)
		{
		#if UNITY_EDITOR 
			CreateBalloon (new Vector3 (cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z+15.0f));
		#else
			// (A)特徴点にRayをあてるならこっち
			var objectPosition = touchToARHitWorld(touch.touchStartPosition);
			// (B)Viewportで取るならこっち
			//var objectPosition = Camera.main.ScreenToViewportPoint(touch.touchStartPosition);

			// y方向は0にして、単位量に変換
			lastCreatedVector = objectPosition - cameraTransform.position;
			lastCreatedVector.y = 0;
			lastCreatedVector = lastCreatedVector / lastCreatedVector.magnitude; 

			lastCreatedBalloon = CreateBalloon(objectPosition);
			lastCreatedBalloonPosition = objectPosition;
		#endif
		}
		// スワイプ
		else if (touch.touchPhaseEx == TouchTools.TouchPhaseExtended.Moved)
		{
			# if UNITY_EDITOR
			# else
				var touchDiff = (touch.touchEndPosition.y - touch.touchStartPosition.y); //y方向だけでいい
				var weight = 100;
				if(lastCreatedBalloon != null)
				{
					lastCreatedBalloon.transform.position = (touchDiff > 0) ? 
															lastCreatedBalloonPosition + lastCreatedVector * touchDiff / weight:
															lastCreatedBalloonPosition;
				}	
			#endif
		}
		// 離した
		else if (touch.touchPhaseEx == TouchTools.TouchPhaseExtended.Ended)
		{
			// 初期化
			lastCreatedBalloon = null;
			lastCreatedVector = new Vector3();
			lastCreatedBalloonPosition = new Vector3();
		}
		else
		{
			// なにもしない
		}
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
											.HitTest(point, ARHitTestResultType.ARHitTestResultTypeFeaturePoint);
		if (hitResults.Count > 0)
		{
			foreach (var hitResult in hitResults)
			{
				return UnityARMatrixOps.GetPosition(hitResult.worldTransform);
			}
		}
		return new Vector3();
	}

	public void RandomBalloonButtonClicked(int balloonCount = 10)
	{	//バルーンをランダムに生成
		# if UNITY_EDITOR
			//ランダム範囲
			for (int i = 0; i < balloonCount; i++) {
				float RandomPositionX = Random.Range(-15,15)/10.0f;
				float RandomPositionY = Random.Range(-30,30)/10.0f;
				float RandomPositionZ = Random.Range(0,5);

				Vector3 RandomPosition = new Vector3(RandomPositionX, RandomPositionY, RandomPositionZ);
				CreateBalloon (new Vector3 (cameraTransform.position.x + 0.0f, cameraTransform.position.y, cameraTransform.position.z + 9.0f) + RandomPosition);
			}
			GameDirector.GetComponent<GameDirector>().ShadeClicked();
		# else
			//ランダム範囲(暫定。実機見ながら調整する)
			for (int i = 0; i < balloonCount; i++) {
				float RandomPositionX = Random.Range(-75,75)/10.0f;
				float RandomPositionY = Random.Range(-10,10)/10.0f;
				float RandomPositionZ = Random.Range(-75,75)/10.0f;
			
				Vector3 RandomPosition = new Vector3(RandomPositionX, RandomPositionY, RandomPositionZ);
				CreateBalloon(cameraTransform.position + RandomPosition);
			}
			GameDirector.GetComponent<GameDirector>().ShadeClicked();
		#endif
	}

	public void PresetArrangement(List<Vector3> positions)
	{
		if(positions != null)
		{
			foreach(var position in positions)
			{
				CreateBalloon(position);
			}
		}
	}

}
