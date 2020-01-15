using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Generic;
using Generic.Manager;

public class GameDirector : MonoBehaviour
{
    // Parameters
    private int _balloonConter;
    private int _throwConter;
    private int _score;
    private int resultScore;
    private int BalloonLimit = 10;
    private int ThrowLimit = 100;
    private float TimeLimit = 30.0f;
    private GameObject timerText;
    private GameObject scoreText;
    private GameObject BalloonCountText;
    private GameObject ThrowCountText;
    private float _time;
    private GameModeController gameMode; //for ReadGameMode
    private GameSceneManager gameSceneMng;
    private ScoreManager scoreMng;

    private GameObject ShootingModeButton;
    public Sprite _MasterBall;
    public Sprite _Hammer;
    private bool spriteFlg = true;
    private bool buttonFlg = false;

    // properties
    #region properties
    public int BalloonCounter
    {
        get{
            return _balloonConter;
        }
        set{
            _balloonConter = value;
        }
    }

    public int ThrowCounter
    {
        get{
            return _throwConter;
        }
        set{
            _throwConter = value;
        }
    }
    public int Score
    {
        get{
            return _score;
        }
        private set{
            _score = value;
        }
    }

    public float TimeValue
    {
        get{
            return _time;
        }
        private set{
            _time = value;
        }
    }

    #endregion
    
    #region methods

    //CountScore
    public void DestroyCount(){
        Score += 1;
    }

    public void HitCount(){
        Score += 1;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameSceneMng = new GameSceneManager();
        scoreMng = new ScoreManager(DataManager.service);

        timerText = GameObject.Find("Timer");
        scoreText = GameObject.Find("Score");
        BalloonCountText = GameObject.Find("BalloonCount");
        ThrowCountText = GameObject.Find("ThrowCount");
        gameMode = GameObject.Find ("ModeSwitcher").GetComponent<GameModeController>();
        BalloonCountText.GetComponent<Text>().text=BalloonCounter.ToString("F0")+"/"+BalloonLimit;
        ThrowCountText.GetComponent<Text>().text=ThrowCounter.ToString("F0")+"/"+ThrowLimit;
        ShootingModeButton = GameObject.Find("ShootingModeButtun");
        ShootingModeButton.transform.Translate (0, -300, 0);//暫定措置：ボタンを画面外に出す。
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameMode.GameMode)
        {
            case GameModeController.eGameMode.None:
                TimeValue = TimeLimit;
                resultScore=0;
                break;
            case GameModeController.eGameMode.Balloon:
                TimeValue -= Time.deltaTime;
                timerText.GetComponent<Text>().text= _time.ToString("F1");//F1 は書式指定子
                BalloonCountText.GetComponent<Text>().text=BalloonCounter.ToString("F0")+"/"+BalloonLimit;
                if(_time < 0|| BalloonCounter == 10)
                {
                    gameMode.GameMode=GameModeController.eGameMode.WaitTime;
                }
                break;
            case GameModeController.eGameMode.WaitTime:
                TimeValue = TimeLimit;
                break;
            case GameModeController.eGameMode.Shooting:
                if(buttonFlg == false){
                    ShootingModeButton.transform.Translate (0, 300, 0);//暫定措置：ボタンを画面内に戻す。
                    buttonFlg = !buttonFlg;
                }
                scoreText.GetComponent<Text>().text = Score.ToString("F0");
                TimeValue -= Time.deltaTime;
                timerText.GetComponent<Text>().text = TimeValue.ToString("F1");//F1 は書式指定子
                BalloonCountText.GetComponent<Text>().text = BalloonCounter.ToString("F0")+"/"+BalloonLimit;
                ThrowCountText.GetComponent<Text>().text = ThrowCounter.ToString("F0")+"/"+ThrowLimit;
                if(TimeValue < 0 || BalloonCounter == 0|| ThrowCounter/ThrowLimit == 1) {
                    var record = ConvertScoreToRecord();
                    scoreMng.RegisterRecord(record);
                    gameSceneMng.ChangeScene(GameScenes.Result);
                }
                break;
        }
    }
    public void BackButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.Home);
    }

    public void ShootingModeButtonClicked()
    {
        //Debug.Log("ShootingModeChange!");
        var img = ShootingModeButton.GetComponent<Image> ();
        spriteFlg = !spriteFlg;
        img.sprite = (spriteFlg) ? _MasterBall : _Hammer;
    }

    public Record ConvertScoreToRecord()
    {
        var UserName = "Guest"; // consider later
        var timeScore = (int)(TimeValue / TimeLimit * 1000);
        var balloonScore = (int)((BalloonLimit - BalloonCounter) / BalloonLimit * 1000);
        var HitProbability = (int)((Score　*　1000)/　ThrowCounter);
        return new Record(UserName, timeScore, balloonScore, HitProbability, DateTime.Now); 
    }

    #endregion
}
