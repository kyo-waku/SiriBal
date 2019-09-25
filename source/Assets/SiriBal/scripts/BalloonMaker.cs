﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class BalloonMaker : MonoBehaviour {

	public GameObject balloonPrefab;
	public float createHeight;
	public float maxRayDistance = 30.0f;
    public float CreateBalloonDistance = 15.0f;
    public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer
	private MaterialPropertyBlock props;
	ControlGameMode controlGameMode;
	CommonTools tools;

	// Use this for initialization
	void Start () {
		props = new MaterialPropertyBlock ();
		tools = GameObject.Find("GameDirector").GetComponent<CommonTools>();
        controlGameMode = GameObject.Find ("ModeSwitcher").GetComponent<ControlGameMode>();
	}

	void CreateBalloon(Vector3 atPosition)
	{
		GameObject balloonGO = Instantiate (balloonPrefab, atPosition, Quaternion.identity);
		
		float r = Random.Range(0.0f, 1.0f);
		float g = Random.Range(0.0f, 1.0f);
		float b = Random.Range(0.0f, 1.0f);

		props.SetColor("_InstanceColor", new Color(r, g, b));
	}

	// Update is called once per frame
	void Update () {

		// Balloon Mode
		if(ControlGameMode.eGameMode.Balloon !=controlGameMode.GameMode)
		{
			return;
		}

		if (tools.touchPhaseEx == CommonTools.TouchPhaseExtended.Began)
		{
		#if UNITY_EDITOR 

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			//we'll try to hit one of the plane collider gameobjects that were generated by the plugin
			//effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
			if (Physics.Raycast (ray, out hit, maxRayDistance, collisionLayer)) 
			{
				CreateBalloon (new Vector3 (hit.point.x, hit.point.y + createHeight, hit.point.z));

				//we're going to get the position from the contact point
				Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", hit.point.x, hit.point.y, hit.point.z));
			}
            //hitしなければ、ray上にバルーンを生成
            else
            {
                CreateBalloon(ray.origin + (ray.direction.normalized * CreateBalloonDistance));
            }

		#else

			Ray ray = Camera.main.ScreenPointToRay(tools.touchPosition);
			CreateBalloon(ray.origin + (ray.direction.normalized * CreateBalloonDistance));
			// List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, 
			// 	ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
			// if (hitResults.Count > 0) {
			// 	foreach (var hitResult in hitResults) {
			// 		Vector3 position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
			// 		CreateBalloon (new Vector3 (position.x, position.y + createHeight, position.z));
			// 		break;
			// 	}
			// }
			
		#endif

		}
    }

}
