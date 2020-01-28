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
    private GameObject timerUI;
    private GameObject scoreText;
    private GameObject BalloonCountText;
    private GameObject ThrowCountText;
    private float _time;
    private GameModeController gameMode; //for ReadGameMode
    private GameSceneManager gameSceneMng;
    private ScoreManager scoreMng;
    private GameObject ShootingModeButton;
    
    [SerializeField]
    private GameObject ShadeUI;
    [SerializeField]
    private GameObject DescriptionUI;
    private GameObject LoadBalGen;
    public Sprite _MasterBall;
    public Sprite _Hammer;
    private bool spriteFlg = true;
    private bool buttonFlg = false;
    public bool bJudgeGenerateLoadingBalloon = false; //LoadingBalloonを生成したか判定
    public bool bJudgeUpdateLoadingBalloonPosMinY = false; //LoadingBalloonのY座標の最小値を更新済みか判定
    public float LoadingBalloonPosMinY = -1.0f; //LoadingBalloonのY座標最小値
    public float LoadingBalloonPosMinYMinus = -1.0f; //LoadingBalloonのY座標最小値がマイナスの時
    public float LoadingBalloonPosMinYPlus = 1.0f;　//LoadingBalloonのY座標最小値がプラスの時



    // properties
    #region properties
    public int BalloonCounter
    {
        get
        {
            return _balloonConter;
        }
        set
        {
            _balloonConter = value;
        }
    }

    public int ThrowCounter
    {
        get
        {
            return _throwConter;
        }
        set
        {
            _throwConter = value;
        }
    }
    public int Score
    {
        get
        {
            return _score;
        }
        private set
        {
            _score = value;
        }
    }

    public float TimeValue
    {
        get
        {
            return _time;
        }
        private set
        {
            _time = value;
        }
    }

    #endregion

    #region methods

    //CountScore
    public void DestroyCount()
    {
        Score += 1;
    }

    public void HitCount()
    {
        Score += 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameSceneMng = new GameSceneManager();
        scoreMng = new ScoreManager(DataManager.service);

        timerText = GameObject.Find("Timer");
        timerUI = GameObject.Find("TimerIcon");
        timerUI.GetComponent<TimerUiController>();
        scoreText = GameObject.Find("Score");
        BalloonCountText = GameObject.Find("BalloonCount");
        ThrowCountText = GameObject.Find("ThrowCount");
        gameMode = GameObject.Find("ModeSwitcher").GetComponent<GameModeController>();
        BalloonCountText.GetComponent<Text>().text = BalloonCounter.ToString("F0") + "/" + BalloonLimit;
        ThrowCountText.GetComponent<Text>().text = ThrowCounter.ToString("F0") + "/" + ThrowLimit;
        ShootingModeButton = GameObject.Find("ShootingModeButtun");
        ShootingModeButton.transform.Translate(0, -300, 0);//暫定措置：ボタンを画面外に出す。

        //ShadeUI = GameObject.Find("Shade");
        ShadeUI.gameObject.SetActive(false);
        //DescriptionUI = GameObject.Find("Description");
        DescriptionUI.gameObject.SetActive(false);
        LoadBalGen = GameObject.Find("LoadingBalloonGenerator");

    }

    // Update is called once per frame
    void Update()
    {
        switch (gameMode.GameMode)
        {
            case GameModeController.eGameMode.None:
                TimeValue = TimeLimit;
                resultScore = 0;
                break;
            case GameModeController.eGameMode.Balloon:
                TimeValue -= Time.deltaTime;
                timerText.GetComponent<Text>().text = _time.ToString("F1");//F1 は書式指定子
                timerUI.GetComponent<TimerUiController>().TimerCount(TimeValue, TimeLimit);//TimerUIの更新
                BalloonCountText.GetComponent<Text>().text = BalloonCounter.ToString("F0") + "/" + BalloonLimit;
                if (_time < 0 || BalloonCounter == 10)
                {
                    gameMode.GameMode = GameModeController.eGameMode.WaitTime;
                    LoadBalGen.GetComponent<LoadingBalloonGenerator>().GenerateLoadingBalloons();
                }
                break;
            case GameModeController.eGameMode.WaitTime:
                TimeValue = TimeLimit;
                break;
            case GameModeController.eGameMode.Shooting:
                if (buttonFlg == false)
                {
                    ShootingModeButton.transform.Translate(0, 300, 0);//暫定措置：ボタンを画面内に戻す。
                    buttonFlg = !buttonFlg;
                }
                scoreText.GetComponent<Text>().text = Score.ToString("F0");
                TimeValue -= Time.deltaTime;
                timerText.GetComponent<Text>().text = TimeValue.ToString("F1");//F1 は書式指定子
                timerUI.GetComponent<TimerUiController>().TimerCount(TimeValue, TimeLimit);//TimerUIの更新
                BalloonCountText.GetComponent<Text>().text = BalloonCounter.ToString("F0") + "/" + BalloonLimit;
                ThrowCountText.GetComponent<Text>().text = ThrowCounter.ToString("F0") + "/" + ThrowLimit;
                if (TimeValue < 0 || BalloonCounter == 0 || ThrowCounter / ThrowLimit == 1)
                {
                    var record = ConvertScoreToRecord();
                    scoreMng.RegisterRecord(record);
                    gameSceneMng.ChangeScene(GameScenes.Result);
                }
                break;
        }

        //LoadingBalloon生成時は画面が隠れているか判定
        if (bJudgeHideScreenByLoadingBalloon() == true)
        {
            ControlDispWaitingScreen(true); //待機画面表示
        }

        //Debug用
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadBalGen.GetComponent<LoadingBalloonGenerator>().GenerateLoadingBalloons();
        }

        //Debug用
        if (Input.GetKeyDown(KeyCode.S))
        {
            ControlDispWaitingScreen(false);
        }

        //Debug用
        if (Input.GetKeyDown(KeyCode.D))
        {
            ControlDispWaitingScreen(true);
        }

    }

    private bool bJudgeHideScreenByLoadingBalloon() //LoadingBalloonで画面が隠れているか判定
    {
        bool bReturn = false;

        //LoadingBalloonで画面が隠れているか判定
        if (bJudgeGenerateLoadingBalloon == true)
        {
            if (bJudgeUpdateLoadingBalloonPosMinY == true && LoadingBalloonPosMinY > 0.0f)
            {
                bReturn = true;
                bJudgeGenerateLoadingBalloon = false;
            }
            LoadingBalloonPosMinY = LoadingBalloonPosMinYPlus;
            bJudgeUpdateLoadingBalloonPosMinY = false;
        }
        return bReturn;
    }

    public void ControlDispWaitingScreen(bool bSetActive) //待機画面の表示制御
    {
        ShadeUI.gameObject.SetActive(bSetActive);
        DescriptionUI.gameObject.SetActive(bSetActive);
    }


    public void ShadeClicked() //ShadeUIクリック時
    {
        ControlDispWaitingScreen(false);
        gameMode.GameMode = GameModeController.eGameMode.Shooting;
    }

    public void BackButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.Home);
    }

    public void ShootingModeButtonClicked()
    {
        //Debug.Log("ShootingModeChange!");
        var img = ShootingModeButton.GetComponent<Image>();
        spriteFlg = !spriteFlg;
        img.sprite = (spriteFlg) ? _MasterBall : _Hammer;
    }

    public Record ConvertScoreToRecord()
    {
        var UserName = "Guest"; // consider later
        var timeScore = (int)(TimeValue / TimeLimit * 1000);
        var balloonScore = (int)((BalloonLimit - BalloonCounter) / BalloonLimit * 1000);
        int HitProbability;
        if (ThrowCounter != 0)
        {
            HitProbability = (int)((Score * 1000) / ThrowCounter);
        }
        else
        {
            HitProbability = 0;
        }
        return new Record(UserName, timeScore, balloonScore, HitProbability, DateTime.Now);
    }

    #endregion
}
