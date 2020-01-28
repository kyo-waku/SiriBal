using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBalloonController : MonoBehaviour
{
    private GameDirector GameDirector;
    int InitForceXMax = 50000; //初期にX方向に加える力の最大値
    int InitForceX = 0; //初期にX方向に加える力
    int InitForceY = 80000; //初期にY方向に加える力
    int InitForceZ = 0; //初期にZ方向に加える力
    float SideForce = 2.0f; //X方向に加える力係数
    float timeOut = 6.0f; //デストラクト時間
    float timeElapsed = 0.0f; //デストラクト時間
    float Amplitude = 500; //振幅
    int ForceDirection = 1; //力を加える方向
    int MinusDirection = -1; //マイナス方向に力を加える場合
    int PlusDirection = 1; //プラス方向に力を加える場合

    // Start is called before the first frame update
    void Start()
    {
        GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        //ランダムで力の向きを決める
        int RandomValue = Random.Range(0, 2);
        if (RandomValue == 0)
        {
            ForceDirection = MinusDirection;
        }
        else if (RandomValue == 1)
        {
            ForceDirection = PlusDirection;
        }
        else
        {
            Debug.Log("ExceptionError RandomForceDirection");
        }

        InitForceX = ForceDirection * Random.Range(0, InitForceXMax); //X方向はランダム
        GetComponent<Rigidbody>().AddForce(InitForceX, InitForceY, InitForceZ); //初期の力を加える
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        AddForceSinWave(rb);
        UpdateLoadingBalloonPosMinY();

        //暫定：時間で消える(そのうち画面外で消すように改良)
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= this.timeOut)
        {
            Destroy(gameObject);
        }
    }

    void AddForceSinWave(Rigidbody rb) //X方向にsin波で動くように力を加える
    {
        //速度の絶対値が振幅以上なら一定の力を加える
        if (rb.velocity.x <= -Amplitude)
        {
            ForceDirection = PlusDirection;
            rb.AddForce(SideForce * ForceDirection * rb.mass * Amplitude * Vector3.right);
        }

        //速度の絶対値が振幅以下なら速度に応じた力を加える
        else if (-Amplitude < rb.velocity.x && rb.velocity.x < Amplitude)
        {
            rb.AddForce(SideForce * rb.mass * Mathf.Sqrt((Mathf.Pow(Amplitude, 2) - Mathf.Pow(rb.velocity.x, 2))) * ForceDirection * Vector3.right);
        }

        //速度の絶対値が振幅以上なら一定の力を加える
        else if (Amplitude <= rb.velocity.x)
        {
            ForceDirection = MinusDirection;
            rb.AddForce(SideForce * ForceDirection * rb.mass * Amplitude * Vector3.right);
        }

        else
        {
            Debug.Log("ExceptionErrorAddForceSinWaveVelocity");
        }
    }

    void UpdateLoadingBalloonPosMinY() //LoadingBalloonのY座標の最小値を更新
    {
        if (GameDirector.JudgeGenerateLoadingBalloon == true)
        {
            if (this.transform.position.y < GameDirector.LoadingBalloonPosMinY)
            {
                GameDirector.LoadingBalloonPosMinY = this.transform.position.y;
            }
            GameDirector.bJudgeUpdateLoadingBalloonPosMinY = true;
        }
    }
}