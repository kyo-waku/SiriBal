using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_missileController : MonoBehaviour
{
    Rigidbody rigidbody;
    GameObject PlayerCamera;
    float MovingForce = 40.0f;
    float timeOut=8.0f;
    float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = GameObject.Find("MainCamera");//プレイヤーを認識する
        rigidbody = GetComponent<Rigidbody>();//rigidbodyの導入
        Vector3 vctr1 = transform.position;//このオブジェクトの座標を取得
        Vector3 vctr2 = PlayerCamera.transform.position;//プレイヤーカメラの座標を取得
        Vector3 vctr3 = vctr2 - vctr1;//このオブジェクトとプレイヤー間のベクトルを算出
        this.transform.rotation = Quaternion.FromToRotation(Vector3.down, -vctr3);//向きを適切に回転
        this.rigidbody.AddForce(MovingForce * (vctr3 / vctr3.magnitude));//プレイヤーに向かっていく向きに力を加える
    }

    // Update is called once per frame
    void Update()
    {
        //時間で消える
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= timeOut) {
            Destroy(gameObject);
        }
    }
}
