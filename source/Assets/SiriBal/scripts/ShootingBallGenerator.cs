using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBallGenerator : MonoBehaviour
{
    public GameObject ShootingBallPrefab;
    public float ShootingForce = 2000.0f;
	ControlGameMode controlGameMode;
	GameObject modeSwitcher;
    // Start is called before the first frame update
    void Start()
    {
        modeSwitcher = GameObject.Find ("ModeSwitcher");
        controlGameMode = modeSwitcher.GetComponent<ControlGameMode>();
    }

    // Update is called once per frame
    void Update()
    {
        // ShootingBall Mode
		if(!controlGameMode.IsShootingBall)
		{
			return;
		}

        #if UNITY_EDITOR // UnityEditor Mode
        //ボールを飛ばすよ
        if (Input.GetMouseButtonDown(0))
        {
            GameObject ShootingBall = Instantiate(ShootingBallPrefab) as GameObject;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ShootingBall.transform.position = ray.origin;
            ShootingBall.GetComponent<ShootingBallConrtoller>().Shoot(ray.direction.normalized * ShootingForce);

            //デバッグ用
            /*
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 5);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10.0f))
            {
                Debug.Log(hit.collider.gameObject.transform.position);
            }
            */
        }
#else
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
                GameObject ShootingBall = Instantiate(ShootingBallPrefab) as GameObject;
				var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                Ray ray = Camera.main.ScreenPointToRay(screenPosition);
                ShootingBall.transform.position = ray.origin;
                ShootingBall.GetComponent<ShootingBallConrtoller>().Shoot(ray.direction.normalized * ShootingForce);
			}
        }
#endif
    }
}
 