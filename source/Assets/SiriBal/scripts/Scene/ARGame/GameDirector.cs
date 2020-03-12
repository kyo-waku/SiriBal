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
    private GameObject TimerText;
    private GameObject TimerUI;
    private GameObject BalloonCountText;
    private GameObject ThrowCountText;
    private GameModeController gameMode; //for ReadGameMode
    private GameSceneManager gameSceneMng;
    private ScoreManager scoreMng;
    private BalloonController balloonController;

    [SerializeField]
    private GameObject WeaponToggleButton;

    [SerializeField]
    private GameObject DescriptionUI;
    [SerializeField]
    private GameObject YarikomiHeader;
    private int currentRank = 1;
    private GameObject LoadBalGen;
    public Sprite _MasterBall;
    public Sprite _Hammer;
    private bool spriteFlg = true;

    // Stages
    public StageData yarikomiStage_rank2;
    public StageData yarikomiStage_rank3;

    // properties
    #region properties
    public int BalloonCounter{get; set;} = 0;
    public int DestroyedBalloonCount{get; set;} = 0;
    public int EnemyAttackHitCount{get;  set;} = 0;
    public int ThrowCounter{get; set;}
    public int Score{get; internal set;}
    public float TimeValue{get; internal set;}
    public bool bJudgeGenerateLoadingBalloon{get; internal set;} = false;
    public bool bJudgeUpdateLoadingBalloonPosMinY{get; internal set;} = false;
    public float LoadingBalloonPosMinY{get; internal set;} = -1.0f;
    public float LoadingBalloonPosMinYMinus{get; internal set;} = -1.0f;
    public float LoadingBalloonPosMinYPlus{get; internal set;} = 1.0f;

    #endregion

    //CountScore
    public void HitCount()
    {
        Score += 1;
    }

    public void Damaged()
    {
        var obj = GameObject.Find("DamagedEffect");
        if (obj != null)
        {
            obj.GetComponent<DamagedEffect>().IsDamaged = true;
        }
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

    // Set up initial UI components
    private void SetupUIComponents()
    {
        // タイマー　
        TimerText = GameObject.Find("Timer");
        TimerUI = GameObject.Find("TimerIcon");
        if (TimerUI != null)
        {
            TimerUI.GetComponent<TimerUiController>();
        }
        // バルーン数　
        BalloonCountText = GameObject.Find("BalloonCount");
        if (BalloonCountText != null)
        {
            BalloonCountText.GetComponent<Text>().text = (BalloonCounter - DestroyedBalloonCount).ToString("F0") + "/" + BalloonCounter;
        }
        // 投げ数
        ThrowCountText = GameObject.Find("ThrowCount");
        if (ThrowCountText != null)
        {
            ThrowCountText.GetComponent<Text>().text = ThrowCounter.ToString("F0") + "/" + stage.ShootingLimit;
        }
        // ゲームモード
        gameMode = GameObject.Find("ModeSwitcher").GetComponent<GameModeController>();
        // 画面遷移用
        LoadBalGen = GameObject.Find("LoadingBalloonGenerator");
        // 説明文
        DescriptionUI.gameObject.SetActive(false);
        // バルーンコントローラー
        balloonController = GameObject.Find("BalloonController").GetComponent<BalloonController>();

        // やりこみモード
        if (stage.GameClearCondition == Stage.ClearCondition.Yarikomi)
        {
            // やりこみモードでは、通常のヘッダー要素は使わない
            var header = GameObject.Find("Header");
            if (header != null)
            {
                header.SetActive(false);
            }
            // 専用ヘッダーをアクティブにする
            YarikomiHeader.SetActive(true);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        gameSceneMng = new GameSceneManager();
        scoreMng = new ScoreManager(DataManager.service);

        // ステージ情報のセットアップ
        SetupStageProperties(DataManager.currentStage); // ゲームシーンに来る前に登録しておくこと
        // UI コンポーネントのセットアップ
        SetupUIComponents();

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
                if (stage.GameClearCondition == Stage.ClearCondition.Yarikomi) // やりこみモード
                {
                    // とりあえず 10個 おいておく。あとから徐々に増える
                    balloonController.RandomBalloonButtonClicked(stage.BalloonLimit);
                    ShowDescription("ひたすらバルーンをうちおとそう");
                }
                else
                {
                    balloonController.RandomBalloonButtonClicked(stage.BalloonLimit);
                    ShowDescription("ランダムにセットされたバルーンをうちおとそう");
                }
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
            // ゲームの状態がバルーンを配置するモードの場合
            case GameModeController.eGameMode.Balloon:
                BalloonPlaceSubproc();
                break;
            // ゲームの状態が次のゲーム開始までの待機状態の場合
            case GameModeController.eGameMode.WaitTime:
                InitGameSettings();
                break;
            // ゲームの状態がバルーンを撃ち落とすモードの場合
            case GameModeController.eGameMode.Shooting:
                ShootingSubproc();
                break;
            // ゲームの状態が結果表示前野待機状態の場合
            case GameModeController.eGameMode.BeforeResult:
                break;
            // ゲームの状態がいずれでもない場合
            case GameModeController.eGameMode.None:
            default:
                InitGameSettings();
                break;
        }
        // ローディング画面制御用プロセス (バルーン配置　→　撃ち落とし　の切り替えなど)
        LoadingBalloonSubprocess();
    }

#region Subprocesses
    private void InitGameSettings()
    {
        TimeValue = stage.TimeLimit > 0? stage.TimeLimit: 0;
        resultScore = 0;
    }

    // バルーン配置
    private void BalloonPlaceSubproc()
    {
        TimeValue -= Time.deltaTime;
        UpdateHeaderContents(TimeValue, (BalloonCounter - DestroyedBalloonCount), -1);
        if (TimeValue < 0 || (BalloonCounter - DestroyedBalloonCount) >= stage.BalloonLimit)
        {
            gameMode.GameMode = GameModeController.eGameMode.WaitTime;
            LoadBalGen.GetComponent<LoadingBalloonGenerator>().GenerateLoadingBalloons();
        }
    }

    // 撃ち落としゲームの本編
    private void ShootingSubproc()
    {
        // Weapon切り替えボタンの表示
        var weaponToggle = WeaponToggleButton.gameObject;
        if (!weaponToggle.activeSelf)
        {
            weaponToggle.SetActive(true);
        }

        if (stage.GameClearCondition == Stage.ClearCondition.Yarikomi)
        {
            YarikomiProc();
        }
        else
        {
            TimeValue -= Time.deltaTime;
            UpdateHeaderContents(TimeValue, (BalloonCounter - DestroyedBalloonCount), ThrowCounter);
            if (TimeValue < 0 || BalloonCounter <= DestroyedBalloonCount || ThrowCounter > stage.ShootingLimit) //ThrowConterを ==  で判定すると最後の1つを投げた瞬間に終わってしまう
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
                    wr.ClearFlag = (BalloonCounter <= DestroyedBalloonCount)? true : false;
                    stage.GetRegisteredShootingWeapons(out var weapons);
                    if(weapons != null)
                    {
                        if(weapons.Count > 0)
                        {
                            wr.Weapon = weapons[0]; // ウェポン獲得ゲームの場合、ウェポン数は基本的に1個
                            DataManager.WResult = wr;
                            gameSceneMng.ChangeScene(GameScenes.WeaponResult);
                        }
                    }
                }
            }
        }
    }

    // やりこみモード専用処理
    // スコア：バルーンを倒した数に関連して増える。
    // ライフ：時間が立つ、物を投げる、と減る。バルーンを倒すと増える。アイテムでも増える。ゼロでゲーム終了
    private void YarikomiProc()
    {
        // 時間計測
        TimeValue += Time.deltaTime;

        // 定期的に増えるバルーン
        var makeBalloon = stage.BalloonLimit - (BalloonCounter - DestroyedBalloonCount);
        if (makeBalloon > 0)
        {
            balloonController.RandomBalloonButtonClicked(makeBalloon);
        }

        // スコア計算
        var score = DestroyedBalloonCount * 33;
        // ランクアップ処理
        RankUpYarikomi(score);

        // ライフポイント計算
        var life = (100 + DestroyedBalloonCount * currentRank)
                    - (int)TimeValue
                    - (int)(ThrowCounter / (stage.BalloonHP * 2))
                    - (int)(EnemyAttackHitCount * 3);

        life = life < 0 ? 0 : life;

        UpdateYarikomiHeaderContents(score, life);
        if (life <= 0) // ゲーム終了
        {
            gameMode.GameMode = GameModeController.eGameMode.BeforeResult;
            var obj = GameObject.Find("YarikomiDescription");
            if (obj != null)
            {
                obj.GetComponent<Text>().text = "そこまで!!";
                obj.GetComponent<Text>().color = new Color(0, 0, 0, 1); 
                obj.GetComponent<Text>().fontSize = 120;
            }
            Invoke("ResultSubproc", 4f);
            scoreMng.SaveYarikomiScoreToLocal(score);
        }
    }

    private void ResultSubproc()
    {
        gameSceneMng.ChangeScene(GameScenes.YarikomiResult);
    }

    private void RankUpYarikomi(int score)
    {
        // ステージの切り替え ランクアップ！
        if (score > 1000 && currentRank == 1)
        {
            var obj = GameObject.Find("YarikomiDescription");
            if (obj != null)
            {
                obj.GetComponent<Text>().text = "ランクアップ!! \n 1 -> 2";
                obj.GetComponent<OriginalEffects>().SetUpFadeIn();
            }
            SetupStageProperties(new Stage(yarikomiStage_rank2));
            currentRank = 2;
        }
        else if (score > 3000 && currentRank == 2)
        {            
            var obj = GameObject.Find("YarikomiDescription");
            if (obj != null)
            {
                obj.GetComponent<Text>().text = "ランクアップ!! \n 2 -> 3";
                obj.GetComponent<OriginalEffects>().SetUpFadeIn();
            }
            SetupStageProperties(new Stage(yarikomiStage_rank3));
            currentRank = 3;
        }
    }
#endregion 

#region LoadingUI
    // ローディング画面の制御
    private void LoadingBalloonSubprocess()
    {
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
#endregion

#region UpdateUIHeaders
    // ヘッダー要素の表示更新
    // 更新したくない場合は負の値をセットすればいい
    private void UpdateHeaderContents(float timeValue, int balloonCount, int throwCount)
    {
        if (timeValue >= 0)
        {
            if (TimerText != null)
            {
                TimerText.GetComponent<Text>().text = timeValue.ToString("F1");//F1 は書式指定子
            }
            if (TimerUI != null)
            {
                TimerUI.GetComponent<TimerUiController>().TimerCount(timeValue, stage.TimeLimit);//TimerUIの更新
            }
        }
        if (balloonCount >= 0)
        {
            if (BalloonCountText)
            {
                BalloonCountText.GetComponent<Text>().text = balloonCount.ToString("F0") + "/" + stage.BalloonLimit;
            }
        }
        if (throwCount >= 0)
        {
            if (ThrowCountText != null)
            {
                ThrowCountText.GetComponent<Text>().text = throwCount.ToString("F0") + "/" + stage.ShootingLimit;
            }
        }
    }

    // やりこみモード用ヘッダー更新
    // score : int
    // life : int(%)
    private void UpdateYarikomiHeaderContents(int score, int life)
    {
        var ScoreText = GameObject.Find("CurrentScore");
        ScoreText.GetComponent<Text>().text = score.ToString();
        var LifeBar = GameObject.Find("LIFEBar");
        LifeBar.GetComponent<Image>().fillAmount = (float)life/100;

        // GREEN: 50, 210, 90, 255
        // RED: 230, 90, 50, 255
        if (life < 20)
        {
            LifeBar.GetComponent<Image>().color = new Color(230, 90, 50, 255);
        }
        else
        {
            LifeBar.GetComponent<Image>().color = new Color(50, 210, 90, 255);
        }
    }

    // 説明用画面の表示
    private void ShowDescription(string message)
    {
        ControlDispWaitingScreen(true);
        GameObject.Find("WaitingText").gameObject.GetComponent<Text>().text = message;
    }

    // 待機画面の表示制御
    public void ControlDispWaitingScreen(bool bSetActive)
    {
        DescriptionUI.gameObject.SetActive(bSetActive);
    }

    // 投げるWeaponを切り替えるボタン
    public void ShootingModeButtonClicked()
    {
        var img = WeaponToggleButton.GetComponent<Image>();
        spriteFlg = !spriteFlg;
        img.sprite = (spriteFlg) ? _MasterBall : _Hammer;
    }
#endregion

#region ControlGameMode
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

    // HOMEに戻る
    public void BackButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.Home);
    }

#endregion


    // スコアの定義はここで決まる。
    public Record ConvertScoreToRecord()
    {
        var UserName = "Guest"; // consider later
        var timeScore = (int)(1000 * TimeValue / stage.TimeLimit);
        var balloonScore = (int)( 1000 * (BalloonCounter - DestroyedBalloonCount) / BalloonCounter );
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
}
