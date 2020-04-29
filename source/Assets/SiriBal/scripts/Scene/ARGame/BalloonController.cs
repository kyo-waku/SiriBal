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
		var gameDirectorRef = GameDirector.GetComponent<GameDirector>();
		if(gameDirectorRef != null)
		{
			gameDirectorRef.BalloonCounter += 1;
			// バルーンのステータスをStage情報から取得する
			balloonPrefab.gameObject.GetComponent<AirBalloonWatcher>().BalloonHP   = gameDirectorRef.stage.BalloonHP;
			balloonPrefab.gameObject.GetComponent<AirBalloonWatcher>().isAction    = gameDirectorRef.stage.IsBalloonAction;
			balloonPrefab.gameObject.GetComponent<AirBalloonWatcher>().AttackSpan  = gameDirectorRef.stage.AttackSpan;

			if (gameDirectorRef.stage.BalloonWeaponKey == Generic.Weapons.Missile)
			{
				balloonPrefab.gameObject.GetComponent<AirBalloonWatcher>().AttackWeapon = WeaponData2.Entity.EnemyWeaponList[0].WeaponPrefab;
			}
			else
			{
				balloonPrefab.gameObject.GetComponent<AirBalloonWatcher>().AttackWeapon = null;
			}
		}
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

	public void CreateRandomBalloon(int balloonCount = 10)
	{
		cameraTransform = mainCamera.transform;
		//バルーンをランダムに生成
		# if UNITY_EDITOR
			//ランダム範囲
			for (int i = 0; i < balloonCount; i++) {
				float RandomPositionX = Random.Range(-15,15)/10.0f;
				float RandomPositionY = Random.Range(-30,20)/10.0f;
				float RandomPositionZ = Random.Range(0,5);

				Vector3 RandomPosition = new Vector3(RandomPositionX, RandomPositionY, RandomPositionZ);
				CreateBalloon (new Vector3 (cameraTransform.position.x + 0.0f, cameraTransform.position.y, cameraTransform.position.z + 9.0f) + RandomPosition);
			}
			GameDirector.GetComponent<GameDirector>().DescriptionClicked();
		# else
			//ランダム範囲(暫定。実機見ながら調整する)
			for (int i = 0; i < balloonCount; i++) {
				Vector3 RandomPosition = GenerateRandomPosition(30, 10, 30, 5);
				CreateBalloon(cameraTransform.position + RandomPosition);
			}
			GameDirector.GetComponent<GameDirector>().DescriptionClicked();
		#endif
	}

	// 中央マージンつきランダム座標生成関数（正負の振り分けは完全ランダム）
	private Vector3 GenerateRandomPosition(int xDist, int yDist, int zDist, int centerMargin = 0)
	{
		float x,y,z = 0f;

		// X
		if (Random.Range(-1, 1) > 0)
		{
			x = Random.Range(-1*xDist, -1*centerMargin)/10.0f;
		}
		else
		{
			x = Random.Range(centerMargin, xDist)/10.0f;
		}

		// Y
		if (Random.Range(-1, 1) > 0)
		{
			y = Random.Range(-1*yDist, -1*centerMargin)/10.0f;
		}
		else
		{
			y = Random.Range(centerMargin, yDist)/10.0f;
		}

		// Z
		if (Random.Range(-1, 1) > 0)
		{
			z = Random.Range(-1*zDist, -1*centerMargin)/10.0f;
		}
		else
		{
			z = Random.Range(centerMargin, zDist)/10.0f;
		}

		return new Vector3(x,y,z);
	}
	public void PresetArrangement(List<Vector3> positions)
	{
		cameraTransform = mainCamera.transform;
		if(positions != null)
		{
			foreach(var position in positions)
			{
				CreateBalloon(cameraTransform.position + position);
			}
		}
	}

}
