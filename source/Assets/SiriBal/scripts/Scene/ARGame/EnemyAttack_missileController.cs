using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;
using Generic.Manager;

public class EnemyAttack_missileController : MonoBehaviour
{
    GameObject director;
    GameModeController controlGameMode;
    [SerializeField]
    GameObject particle;

    Rigidbody rigidbody;
    GameObject PlayerCamera;
    float MovingForce = 25.0f;
    float timeOut = 15.0f;
    float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
        controlGameMode = GameObject.Find ("ModeSwitcher").GetComponent<GameModeController>();
        director = GameObject.Find("GameDirector");

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

       //Detect Collision
    void OnCollisionEnter(Collision other)
    {
        if (controlGameMode == null) return;
        if (controlGameMode.GameMode == GameModeController.eGameMode.Shooting)//get point! if gameMode is shooting
        {
            if (other.gameObject.tag == "player_sphere")
            {
                if (director != null)
                {
                    director.GetComponent<GameDirector>().Damaged();
                    director.GetComponent<GameDirector>().EnemyAttackHitCount += 1;
                }
                Destroy(gameObject);
            }
            else if (other.gameObject.tag == "enemy_attack")
            {
                // ミサイル同士の衝突は爆発するパーティクル
                if (particle != null)
                {
                    particle.transform.localScale = new Vector3(0.01f ,0.01f ,0.01f); // ちょと小さくしておく
                    Instantiate (particle, gameObject.transform.position, gameObject.transform.rotation);
                }
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject); // しれっと消す
            }
        }
    }
}
