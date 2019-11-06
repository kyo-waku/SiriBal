﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class BalloonMaker : MonoBehaviour {

	public Transform cameraTransform;
	public GameObject balloonPrefab;
	public float createHeight;
	public float maxRayDistance = 30.0f;
    public float CreateBalloonDistance = 15.0f;
    public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer
	ControlGameMode controlGameMode;
	CommonTools tools;

	// Use this for initialization
	void Start () {
		tools = GameObject.Find("GameDirector").GetComponent<CommonTools>();
        controlGameMode = GameObject.Find ("ModeSwitcher").GetComponent<ControlGameMode>();
	}

	void CreateBalloon(Vector3 atPosition)
	{
		GameObject balloonGO = Instantiate (balloonPrefab, atPosition, Quaternion.identity);
	}

	// Update is called once per frame
	void Update () {
		cameraTransform = GameObject.Find("MainCamera").transform;
		// Balloon Mode
		if(ControlGameMode.eGameMode.Balloon !=controlGameMode.GameMode)
		{
			return;
		}

		if (tools.touchPhaseEx == CommonTools.TouchPhaseExtended.Began)
		{
		#if UNITY_EDITOR 

			CreateBalloon (new Vector3 (cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z));
		
		#else
			var screenPosition = Camera.main.ScreenToViewportPoint(tools.touchPosition);
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
