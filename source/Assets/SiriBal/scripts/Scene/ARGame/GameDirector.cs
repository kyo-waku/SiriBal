using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private ItemGenerator itemGenerator;

    [SerializeField]
    private GameObject WeaponToggleButton;
    [SerializeField]
    private GameObject WeaponToggleButtonBase;

    [SerializeField]
    private GameObject DescriptionUI;
    [SerializeField]
    private GameObject YarikomiHeader;
    private int currentRank = 1;
    private GameObject LoadBalGen;
    private WeaponIds spriteId = WeaponIds.Stone;

    // properties
    #region properties
    private int ScorePoint{get; set;} = 0; 
    private int LifePoint{get; set;} = 100;
    public int NyusanPoint{get; set;} = 100;
    public int BalloonCounter{get; set;} = 0;
    public int DestroyedBalloonCount{get; set;} = 0;
    public int MissingBalloonCount{get; set;} = 0;
    public int EnemyAttackHitCount{get;  set;} = 0;

    // RecoveryRate (%)
    public int RecoveryRate {get; set;} = 0;
    private int RecoveryInterval {get; set;} = 10; //アイテム取得時の回復間隔[frame]
    private int RecoveryIntervalCount {get; set;} = 0; //アイテム取得時の回復間隔のカウント[frame]
    private bool IsRecovering {get; set;} = false; //回復中フラグ　true:回復中　false:回復中でない
    public int ThrowCounter{get; set;}
    public int Score{get; internal set;}
    public float TimeValue{get; internal set;}
    private float ItemGenTime{get; set;} = 0; //アイテム生成までの経過時間
    private float ItemGenInterval{get; set;} = 10; //アイテム生成の時間間隔
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
        // アイテムジェネレーター
        itemGenerator = GameObject.Find("ItemGenerator").GetComponent<ItemGenerator>();

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
        // ライフ関係の初期化
        InitializeCounts(true);
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
                    balloonController.CreateRandomBalloon(randomCreate);
                    ShowDescription("ランダムにセットされたバルーンをうちおとそう");
                }
                gameMode.GameMode = GameModeController.eGameMode.WaitTime;
                break;
            case Stage.ArrangementMode.Random:
                if (stage.GameClearCondition == Stage.ClearCondition.Yarikomi) // やりこみモード
                {
                    // とりあえず 10個 おいておく。あとから徐々に増える
                    balloonController.CreateRandomBalloon(stage.BalloonLimit);
                    ShowDescription("ひたすらバルーンをうちおとそう");
                }
                else
                {
                    balloonController.CreateRandomBalloon(stage.BalloonLimit);
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
            // --------
            // やりこみモード以外は一旦コメントアウトしておく
            // --------
            // TimeValue -= Time.deltaTime;
            // UpdateHeaderContents(TimeValue, (BalloonCounter - DestroyedBalloonCount), ThrowCounter);
            // if (TimeValue < 0 || BalloonCounter <= DestroyedBalloonCount || ThrowCounter > stage.ShootingLimit) //ThrowConterを ==  で判定すると最後の1つを投げた瞬間に終わってしまう
            // {
            //     if (stage.GameClearCondition == Stage.ClearCondition.None) // クリア条件なし = 通常の点数制
            //     {
            //         gameSceneMng.ChangeScene(GameScenes.Result);
            //     }
            //     else if (stage.GameClearCondition == Stage.ClearCondition.DestroyAll) // ウェポン獲得ゲームの場合
            //     {
            //         var wr = new WeaponResult();
            //         wr.ClearFlag = (BalloonCounter <= DestroyedBalloonCount)? true : false;
            //         stage.GetRegisteredShootingWeapons(out var weapons);
            //         if(weapons != null)
            //         {
            //             if(weapons.Count > 0)
            //             {
            //                 wr.Weapon = weapons[0]; // ウェポン獲得ゲームの場合、ウェポン数は基本的に1個
            //                 DataManager.WResult = wr;
            //                 gameSceneMng.ChangeScene(GameScenes.WeaponResult);
            //             }
            //         }
            //     }
            // }
        }
    }

    // やりこみモード専用処理
    // スコア：バルーンを倒した数に関連して増える。
    // ライフ：時間が立つ、物を投げる、と減る。バルーンを倒すと増える。アイテムでも増える。ゼロでゲーム終了
    private void YarikomiProc()
    {
        // 時間計測
        TimeValue += Time.deltaTime;
        ItemGenTime += Time.deltaTime;
        // Missing処理
        ShowMissing();
        // スコア計算
        ScorePoint = ScorePoint + DestroyedBalloonCount * currentRank * ((100 - LifePoint)/10 + 20); // 死にかけはポイント高め
        // ライフポイント計算
        UpdateLifePoint();
        // 乳酸ポイント計算
        UpdateNyusanPoint();
        // ランクアップ処理
        RankUpYarikomi(ScorePoint);
        // 定期的に増えるバルーン(DestroyedBalloonCountの初期化タイミングに注意)
        if (DestroyedBalloonCount > 0 || MissingBalloonCount > 0)
        {
            balloonController.CreateRandomBalloon(DestroyedBalloonCount + MissingBalloonCount);
            DestroyedBalloonCount = 0;
            MissingBalloonCount = 0;
        }
        // 定期的に増えるアイテム
        if (ItemGenTime > ItemGenInterval)
        {
            itemGenerator.CreateRandomItem(1);
            ItemGenTime = 0;
        }
        // ヘッダーの更新
        UpdateYarikomiHeaderContents(ScorePoint, LifePoint, NyusanPoint);
        if (LifePoint <= 0) // ゲーム終了
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
            scoreMng.SaveYarikomiScoreToLocal(ScorePoint);
        }

        // 初期化処理は最後
        InitializeCounts();
    }

    private void UpdateNyusanPoint()
    {
        NyusanPoint -= ThrowCounter; // ここでウエポンのステータスに応じた重みをかければOK
    }
    private void UpdateLifePoint()
    {
        //徐々に回復
        if(RecoveryRate > 0)
        {
            IsRecovering = true; //回復中フラグON
            RecoveryIntervalCount++;
            if(RecoveryIntervalCount > RecoveryInterval)
            {
                LifePoint++;
                RecoveryRate--;
                RecoveryIntervalCount = 0;
                if(LifePoint > 100){RecoveryRate = 0;} //LifePointが上限に達したら回復終了
            }
        }
        else
        {
            IsRecovering = false; //回復中フラグOFF
        }

        LifePoint = LifePoint
                    + (int)DestroyedBalloonCount * stage.BalloonHP  // Balloonを倒したら、そのHP分だけ回復できる
                    - (int)MissingBalloonCount * 5          // Missingの重みはでかい・・・
                    - (int)TimeValue                        // 1秒あたり1減る
                    // - (int)(ThrowCounter / (stage.BalloonHP * 2))   // 投げるほど減る --> この要素はNyusanPointにあるのでこちらからは削除する
                    - (int)(EnemyAttackHitCount * 8);   // 攻撃されると減る

        LifePoint = LifePoint < 0 ? 0 : LifePoint; // LifePointはマイナスにしない
        LifePoint = LifePoint > 100?  100 : LifePoint; // LifePointは上限が100

    }

    private void InitializeCounts(bool IsLifeInitialize = false)
    {
        if (IsLifeInitialize)
        {
            LifePoint = 100;
        }
        if(TimeValue > 1)
        {
            NyusanPoint += (int)TimeValue;
            NyusanPoint = NyusanPoint > 100? 100: NyusanPoint;
            TimeValue = 0;
        }
        ThrowCounter = 0;
        EnemyAttackHitCount = 0;
    }

    private void ShowMissing()
    {
        if(MissingBalloonCount > 0)
        {
            var obj = GameObject.Find("YarikomiDescription");
            if (obj != null)
            {
                obj.GetComponent<Text>().color = Color.red;
                obj.GetComponent<Text>().text = "Missing";
                obj.GetComponent<OriginalEffects>().SetUpFadeIn(0.5f);
            }
        }
    }
    private void ResultSubproc()
    {
        gameSceneMng.ChangeScene(GameScenes.YarikomiResult);
    }

    private void RankUpYarikomi(int score)
    {
        // ステージの切り替え ランクアップ！
        if (score > stage.RankUpThreshold && currentRank < StageData2.Entity.StageList.Count)
        {
            var obj = GameObject.Find("YarikomiDescription");
            if (obj != null)
            {
                obj.GetComponent<Text>().text = String.Format("ランクアップ!! \n {0} -> {1}", currentRank, currentRank + 1);
                obj.GetComponent<OriginalEffects>().SetUpFadeIn();
            }
            SetupStageProperties(new Stage(StageData2.Entity.StageList[currentRank]));
            balloonController.CreateRandomBalloon(StageData2.Entity.StageList[currentRank].BalloonLimit - StageData2.Entity.StageList[currentRank - 1].BalloonLimit);
            currentRank += 1;
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
    private void UpdateYarikomiHeaderContents(int score, int life, int nyusan)
    {
        var ScoreText = GameObject.Find("CurrentScore");
        ScoreText.GetComponent<Text>().text = score.ToString();
        var LifeBar = GameObject.Find("LIFEBar");
        LifeBar.GetComponent<Image>().fillAmount = (float)life/100;
        WeaponToggleButtonBase.GetComponent<Image>().fillAmount = (float)nyusan/100;
        float RGBSin = Mathf.Sin(Time.time * 5) / 2 + 0.5f;

        // GREEN: 50, 210, 90, 255
        // YELLOW: 255, 190, 0, 255
        // RED: 230, 90, 50, 255
        float GreenR = 50/255f;
        float GreenG = 210/255f;
        float GreenB = 90/255f;
        float YellowR = 255/255f;
        float YellowG = 190/255f;
        float YellowB = 0/255f;
        float RedR = 230/255f;
        float RedG = 90/255f;
        float RedB = 50/255f;

        if (life < 20)
        {
            //回復中は白点滅
            if(IsRecovering == true)
            {
                RedR += RGBSin;
                RedG += RGBSin;
                RedB += RGBSin;
                if(RedR > 255){RedR = 255;}
                if(RedG > 255){RedG = 255;}
                if(RedB > 255){RedB = 255;}
            }

            LifeBar.GetComponent<Image>().color = new Color(RedR, RedG, RedB, 255/255f);
        }
        else if (life < 50 && life >= 20)
        {
            //回復中は白点滅
            if(IsRecovering == true)
            {
                YellowR += RGBSin;
                YellowG += RGBSin;
                YellowB += RGBSin;
                if(YellowR > 255){YellowR = 255;}
                if(YellowG > 255){YellowG = 255;}
                if(YellowB > 255){YellowB = 255;}
            }

            LifeBar.GetComponent<Image>().color = new Color(YellowR, YellowG, YellowB, 255/255f);
        }
        else
        {
            //回復中は白点滅
            if(IsRecovering == true)
            {
                GreenR += RGBSin;
                GreenG += RGBSin;
                GreenB += RGBSin;
                if(GreenR > 255){GreenR = 255;}
                if(GreenG > 255){GreenG = 255;}
                if(GreenB > 255){GreenB = 255;}
            }

            LifeBar.GetComponent<Image>().color = new Color(GreenR, GreenG, GreenB, 255/255f);
        }

        if (nyusan < 20)
        {
            WeaponToggleButtonBase.GetComponent<Image>().color = new Color(230/255f, 90/255f, 50/255f, 255/255f);
        }
        else if (nyusan < 50 && nyusan >= 20)
        {
            WeaponToggleButtonBase.GetComponent<Image>().color = new Color(255/255f, 190/255f, 0/255f, 255/255f);
        }
        else
        {
            WeaponToggleButtonBase.GetComponent<Image>().color = new Color(50/255f, 210/255f, 90/255f, 255/255f);
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
        var img_base = WeaponToggleButtonBase.GetComponent<Image>();

        spriteId = NextAvailableWeaponId(spriteId);
        var weapon = WeaponData.Entity.HeroWeaponList.Where(x => x.WeaponID == spriteId).First();
        img.sprite = weapon.SelectedIcon;
        img_base.sprite = weapon.SelectedIcon;
    }

    public WeaponIds NextAvailableWeaponId(WeaponIds currentId)
    {
        var availableWeaponIds = new List<WeaponIds>();
        foreach(var weapon in WeaponData.Entity.HeroWeaponList)
        {
            if(weapon.IsWeaponAcquired)
            {
                availableWeaponIds.Add(weapon.WeaponID);
            }
        }

        var nextId = currentId;
        // 複数個見つかった
        if(availableWeaponIds.Count > 1)
        {
            availableWeaponIds.Sort();
            for(var index = 0; index < availableWeaponIds.Count; index++)
            {
                if(availableWeaponIds[index] == currentId)
                {
                    nextId = (index + 1 == availableWeaponIds.Count)? availableWeaponIds[0]: availableWeaponIds[index + 1];
                }
            }
        }
        return nextId;
    }

#endregion

#region ControlGameMode
    // DescriptionUIクリック時
    public void DescriptionClicked()
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

}
