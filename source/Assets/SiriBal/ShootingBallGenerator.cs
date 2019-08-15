using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBallGenerator : MonoBehaviour
{
    public GameObject ShootingBallPrefab;
    public float ShootingForce = 2000.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ボールを飛ばすよ
        if (Input.GetKey(KeyCode.S) && Input.GetMouseButtonDown(0))
        {
            GameObject ShootingBall = Instantiate(ShootingBallPrefab) as GameObject;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldDir = ray.direction;
            ShootingBall.GetComponent<ShootingBallConrtoller>().Shoot(worldDir.normalized * ShootingForce);
        }
    }
}
