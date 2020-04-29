using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBalloonWatcher : MonoBehaviour
{
    // define balloon paramater
    Rigidbody rb;
    int ActionMode;
    public int BalloonHP;
    int BreakCount;
    float ActionSpan;
    public bool isAction = true;

    float timeElapsed;
    public int AttackSpan;
    float AttackTime;

    //find GameDirector for scoring
    GameObject director;
    GameObject PlayerCamera;

    public GameObject AttackWeapon;
    float WeaponClearance = 0.5f;

    GameModeController controlGameMode;//for ReadGameMode

    [SerializeField]
    GameObject particle;

    private const int MAXDISTANCE = 20;

    //Decision Balloon's Action
    void GetAction(int ActionNo){
        if (isAction == false) return; // Action なし

        float force = Random.Range(100.0f,200.0f);
        Vector3 UpperForce = new Vector3 (0.0f,force,0.0f);
        Vector3 sideForce = new Vector3 (force,0.0f, 0.0f);
        switch(ActionNo){
            case 0:
                //Rise
                rb.AddForce (UpperForce);
                break;
            case 1:
                rb.AddForce (1.2f*UpperForce);
                break;
            case 2:
                rb.AddForce (sideForce);
                break;
            case 3:
                rb.AddForce (-sideForce);
                break;
            case 4:
                rb.AddForce (-UpperForce);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        controlGameMode = GameObject.Find ("ModeSwitcher").GetComponent<GameModeController>();
        //Get Rigid Body
        rb = this.GetComponent<Rigidbody>();
        //Find GameDirector for Scoring
        director = GameObject.Find("GameDirector");
        PlayerCamera = GameObject.Find("MainCamera");//プレイヤーを認識する
        //AttackSpan = 4.0f;//デバッグ用の攻撃間隔
        AttackSpan = AttackSpan + Random.Range(0,5);
        //ActionSpan = Random.Range(5,15);
        //Decision 1st ActionSpan
        ActionSpan = Random.Range(3,5);

        // Initialize
        // BalloonHP = 1;
        BreakCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(controlGameMode.GameMode)
        {
            case GameModeController.eGameMode.None:
                break;
            case GameModeController.eGameMode.Balloon:
                break;
            case GameModeController.eGameMode.WaitTime:
                break;
            case GameModeController.eGameMode.Shooting:
                //ActionTimer
                timeElapsed += Time.deltaTime;
                if(timeElapsed >= ActionSpan) {
                    timeElapsed = 0.0f;//reset Timer
                    ActionSpan = Random.Range(5,10);//reset Action Span
                    //this.rigidbody.velocity=0.0f;
                    ActionMode = Random.Range(0,5);
                    GetAction(ActionMode);
                }
                //攻撃する
                AttackTime += Time.deltaTime;
                if (AttackTime >= AttackSpan){
                    AttackTime = 0.0f;
                    if (AttackWeapon != null)
                    {
                        AttackAction(WeaponClearance,AttackWeapon);
                    }
                }
                break;
        }
        
        // バルーンが離れすぎたら消す処理
        ByeByeBalloon();
    }

    //攻撃する
    void AttackAction(float Clearance, GameObject Attackprefab){
        Vector3 vctr1 = transform.position;//このオブジェクトの座標を取得
        Vector3 vctr2 = PlayerCamera.transform.position;//プレイヤーカメラの座標を取得
        Vector3 vctr3 = vctr2 - vctr1;//このオブジェクトとプレイヤー間のベクトルを算出
        GameObject go = Instantiate(Attackprefab) as GameObject;
        go.transform.position = vctr1 + Clearance * (vctr3/vctr3.magnitude);
    }
    // バルーンが遠くに行ってしまったよ、の処理（名前ふざけすぎた）
    private void ByeByeBalloon()
    {
        if (PlayerCamera != null)
        {
            var distance = Vector3.Distance(gameObject.transform.position, PlayerCamera.transform.position);
            if (distance > MAXDISTANCE)
            {
                this.director.GetComponent<GameDirector>().MissingBalloonCount += 1;
                Destroy(gameObject);
            }
        }
    }

    //Detect Collision
    void OnCollisionEnter(Collision other)
    {
        if (controlGameMode == null) return;

        if(controlGameMode.GameMode == GameModeController.eGameMode.Shooting)//get point! if gameMode is shooting
        {
            if (other.gameObject.tag == "player_shoot")
            {
                var weaponprops = other.gameObject.GetComponent<WeaponProperties>();
                if (weaponprops != null)
                {
                    BreakCount += weaponprops.Attack; // weapon から Attack を取得する
                }
                else
                {
                    BreakCount += 1; // weaponなしは困るけどとりあえず 1 にしておく
                }
                this.director.GetComponent<GameDirector>().HitCount();
                if(BalloonHP <= BreakCount)
                {
                    // Explosion
                    if (particle != null)
                    {
                        Instantiate (particle, transform.position,transform.rotation);
                    }
                    this.director.GetComponent<GameDirector>().DestroyedBalloonCount = 1;
                    Destroy(gameObject);
                }
            }
        }
    }
}
