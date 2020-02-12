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
    public Stage stage;
    private int resultScore;

    // UI component references
    private GameObject timerText;
    private GameObject timerUI;
    private GameObject BalloonCountText;
    private GameObject ThrowCountText;
    private GameModeController gameMode; //for ReadGameMode
    private GameSceneManager gameSceneMng;
    private ScoreManager scoreMng;

    [SerializeField]
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

    // properties
    #region properties
    public int BalloonCounter{get; internal set;}
    public int ThrowCounter{get; internal set;}
    public int Score{get; internal set;}
    public float TimeValue{get; internal set;}
    public bool bJudgeGenerateLoadingBalloon{get; internal set;} = false;
    public bool bJudgeUpdateLoadingBalloonPosMinY{get; internal set;} = false;
    public float LoadingBalloonPosMinY{get; internal set;} = -1.0f;
    public float LoadingBalloonPosMinYMinus{get; internal set;} = -1.0f;
    public float LoadingBalloonPosMinYPlus{get; internal set;} = 1.0f;

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

    void SetupStageProperties(Stage currentStage)
    {
        if (currentStage == null)
        {
            stage = new Stage();
        }
        else
        {
            stage = currentStage;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameSceneMng = new GameSceneManager();
        scoreMng = new ScoreManager(DataManager.service);

        // ステージ情報のセットアップ
        SetupStageProperties(DataManager.currentStage);

        timerText = GameObject.Find("Timer");
        timerUI = GameObject.Find("TimerIcon");
        timerUI.GetComponent<TimerUiController>();
        BalloonCountText = GameObject.Find("BalloonCount");
        BalloonCountText.GetComponent<Text>().text = BalloonCounter.ToString("F0") + "/" + stage.BalloonLimit;
        ThrowCountText = GameObject.Find("ThrowCount");
        ThrowCountText.GetComponent<Text>().text = ThrowCounter.ToString("F0") + "/" + stage.ShootingLimit;
        gameMode = GameObject.Find("ModeSwitcher").GetComponent<GameModeController>();
        LoadBalGen = GameObject.Find("LoadingBalloonGenerator");

        ShadeUI.gameObject.SetActive(false);
        DescriptionUI.gameObject.SetActive(false);

        switch(stage.BalloonArrangementMode)
        {
            // 自動セットアップから開始するパターン
            case Stage.ArrangementMode.Preset:
            case Stage.ArrangementMode.Random:
                // Auto arrange function
                ShowDescription("　");
                gameMode.GameMode = GameModeController.eGameMode.WaitTime;
                break;

            // 手動セットアップから開始するパターン
            case Stage.ArrangementMode.Manual:
            default:
                ShowDescription("まずはバルーンをセットしよう");
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (gameMode.GameMode)
        {
            case GameModeController.eGameMode.None:
                TimeValue = stage.TimeLimit;
                resultScore = 0;
                break;
            case GameModeController.eGameMode.Balloon:
                TimeValue -= Time.deltaTime;
                timerText.GetComponent<Text>().text = TimeValue.ToString("F1");//F1 は書式指定子
                timerUI.GetComponent<TimerUiController>().TimerCount(TimeValue, stage.TimeLimit);//TimerUIの更新
                BalloonCountText.GetComponent<Text>().text = BalloonCounter.ToString("F0") + "/" + stage.BalloonLimit;
                if (TimeValue < 0 || BalloonCounter >= stage.BalloonLimit)
                {
                    gameMode.GameMode = GameModeController.eGameMode.WaitTime;
                    LoadBalGen.GetComponent<LoadingBalloonGenerator>().GenerateLoadingBalloons();
                }
                break;
            case GameModeController.eGameMode.WaitTime:
                TimeValue = stage.TimeLimit;
                break;
            case GameModeController.eGameMode.Shooting:
                if (buttonFlg == false)
                {
                    ShootingModeButton.gameObject.SetActive(true);//shootingModeButtonを表示
                    buttonFlg = !buttonFlg;
                }
                TimeValue -= Time.deltaTime;
                timerText.GetComponent<Text>().text = TimeValue.ToString("F1");//F1 は書式指定子
                timerUI.GetComponent<TimerUiController>().TimerCount(TimeValue, stage.TimeLimit);//TimerUIの更新
                BalloonCountText.GetComponent<Text>().text = BalloonCounter.ToString("F0") + "/" + stage.BalloonLimit;
                ThrowCountText.GetComponent<Text>().text = ThrowCounter.ToString("F0") + "/" + stage.ShootingLimit;
                if (TimeValue < 0 || BalloonCounter == 0 || ThrowCounter / stage.ShootingLimit == 1)
                {
                    var record = ConvertScoreToRecord();
                    DataManager.MyLatestRecord = record;
                    gameSceneMng.ChangeScene(GameScenes.Result);
                }
                break;
        }

        //LoadingBalloon生成時は画面が隠れているか判定
        if (bJudgeHideScreenByLoadingBalloon() == true)
        {
            ShowDescription("つぎはタップでバルーンをうちおとそう");
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

    private void ShowDescription(string message)
    {
        ControlDispWaitingScreen(true);
        GameObject.Find("WaitingText").gameObject.GetComponent<Text>().text = message;
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
        if(gameMode.GameMode == GameModeController.eGameMode.None)
        {
            gameMode.GameMode = GameModeController.eGameMode.Balloon;
        }
        else if (gameMode.GameMode == GameModeController.eGameMode.WaitTime)
        {
            gameMode.GameMode = GameModeController.eGameMode.Shooting;
        }
        else
        {
            //Undefined
        }
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
        var timeScore = (int)(1000 * TimeValue / stage.TimeLimit);
        var balloonScore = (int)( 1000 * (stage.BalloonLimit - BalloonCounter) / stage.BalloonLimit );
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
