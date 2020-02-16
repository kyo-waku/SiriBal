using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Generic;
using Generic.Manager;

public class GameDirector : MonoBehaviour
{
    // Key Parameters
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
    private BalloonController balloonController;

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
        SetupStageProperties(DataManager.currentStage); // ゲームシーンに来る前に登録しておくこと

        // タイマー　
        timerText = GameObject.Find("Timer");
        timerUI = GameObject.Find("TimerIcon");
        timerUI.GetComponent<TimerUiController>();
        // バルーン数　
        BalloonCountText = GameObject.Find("BalloonCount");
        BalloonCountText.GetComponent<Text>().text = BalloonCounter.ToString("F0") + "/" + stage.BalloonLimit;
        // 投げ数
        ThrowCountText = GameObject.Find("ThrowCount");
        ThrowCountText.GetComponent<Text>().text = ThrowCounter.ToString("F0") + "/" + stage.ShootingLimit;
        // ゲームモード
        gameMode = GameObject.Find("ModeSwitcher").GetComponent<GameModeController>();
        // 画面遷移用
        LoadBalGen = GameObject.Find("LoadingBalloonGenerator");
        // 説明文
        ShadeUI.gameObject.SetActive(false);
        DescriptionUI.gameObject.SetActive(false);
        // バルーンコントローラー
        balloonController = GameObject.Find("BalloonController").GetComponent<BalloonController>();

        switch(stage.BalloonArrangementMode)
        {
            // 自動セットアップから開始するパターン
            case Stage.ArrangementMode.Preset:
                // 登録数に満たない分はランダムに生成
                var randomCreate = 0;
                if(DefinedErrors.Pass == stage.GetRegisteredPositions(out var positions))
                {
                    randomCreate = (positions.Count < stage.BalloonLimit)? stage.BalloonLimit - positions.Count: 0;
                    balloonController.PresetArrangement(positions);
                    ShowDescription(stage.StageDescription);
                }
                else //自動配置に失敗したらランダムで処理する
                {
                    randomCreate = stage.BalloonLimit;
                }
                if (randomCreate > 0) // ランダムに作る数が登録されている場合
                {
                    balloonController.RandomBalloonButtonClicked(randomCreate);
                    ShowDescription("ランダムにセットされたバルーンをうちおとそう");
                }
                gameMode.GameMode = GameModeController.eGameMode.WaitTime;
                break;
            case Stage.ArrangementMode.Random:
                balloonController.RandomBalloonButtonClicked(stage.BalloonLimit);
                ShowDescription("ランダムにセットされたバルーンをうちおとそう");
                gameMode.GameMode = GameModeController.eGameMode.WaitTime;
                break;
            // 手動セットアップから開始するパターン
            case Stage.ArrangementMode.Manual:
            default:
                gameMode.GameMode = GameModeController.eGameMode.None;
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
                UpdateHeaderContents(TimeValue, BalloonCounter, -1);
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
                UpdateHeaderContents(TimeValue, BalloonCounter, ThrowCounter);
                if (TimeValue < 0 || BalloonCounter == 0 || ThrowCounter > stage.ShootingLimit) //ThrowConterを ==  で判定すると最後の1つを投げた瞬間に終わってしまう
                {
                    if (stage.GameClearCondition == Stage.ClearCondition.None) // クリア条件なし = 通常の点数制
                    {
                        var record = ConvertScoreToRecord();
                        DataManager.MyLatestRecord = record;
                        gameSceneMng.ChangeScene(GameScenes.Result);
                    }
                    else if (stage.GameClearCondition == Stage.ClearCondition.DestroyAll) // ウェポン獲得ゲームの場合
                    {
                        var wr = new WeaponResult();
                        wr.ClearFlag = (BalloonCounter == 0)? true : false;
                        stage.GetRegisteredShootingWeapons(out var weapons);
                        wr.Weapons = weapons;
                        DataManager.WResult = wr;
                        gameSceneMng.ChangeScene(GameScenes.WeaponResult);
                    }
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

    // ヘッダー要素の表示更新
    // 更新したくない場合は負の値をセットすればいい
    private void UpdateHeaderContents(float timeValue, int balloonCount, int throwCount)
    {
        if (timeValue >= 0)
        {
            timerText.GetComponent<Text>().text = timeValue.ToString("F1");//F1 は書式指定子
            timerUI.GetComponent<TimerUiController>().TimerCount(timeValue, stage.TimeLimit);//TimerUIの更新
        }
        if (balloonCount >= 0)
        {
            BalloonCountText.GetComponent<Text>().text = balloonCount.ToString("F0") + "/" + stage.BalloonLimit;
        }
        if (throwCount >= 0)
        {
            ThrowCountText.GetComponent<Text>().text = throwCount.ToString("F0") + "/" + stage.ShootingLimit;
        }
    }

    // 説明用画面の表示
    private void ShowDescription(string message)
    {
        ControlDispWaitingScreen(true);
        GameObject.Find("WaitingText").gameObject.GetComponent<Text>().text = message;
    }

    // LoadingBalloonで画面が隠れているか判定
    private bool bJudgeHideScreenByLoadingBalloon()
    {
        bool bReturn = false;

        // LoadingBalloonで画面が隠れているか判定
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

    public void ControlDispWaitingScreen(bool bSetActive) // 待機画面の表示制御
    {
        ShadeUI.gameObject.SetActive(bSetActive);
        DescriptionUI.gameObject.SetActive(bSetActive);
    }


    // ShadeUIクリック時
    public void ShadeClicked() 
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
        var img = ShootingModeButton.GetComponent<Image>();
        spriteFlg = !spriteFlg;
        img.sprite = (spriteFlg) ? _MasterBall : _Hammer;
    }

    // スコアの定義はここで決まる。
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
