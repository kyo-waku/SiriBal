using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class HomeSceneController : MonoBehaviour
{
    // COMMON
    private GameSceneManager _gameSceneMng;
    //----------

    //　STAGE DATA (Ref for scriptable objects)
    public StageData easyStage;
    public StageData normalStage;
    public StageData hardStage;
    public StageData yarikomiStage_rank1;

    //---------

    // RANK UI
    private ScoreManager _scoreManager;
    private List<Record> _records;
    private bool updateFlag;
    //---------

    // WEAPON UI
    [SerializeField]
    Sprite stone_on;
    [SerializeField]
    Sprite stone_off;
    [SerializeField]
    Sprite hammer_on;
    [SerializeField]
    Sprite hammer_off;
    //--------

    // GAME UI
    private enum StageIndices
    {
        easy = 1,
        normal,
        hard,
    }

    [SerializeField]
    GameObject DescriptionUI;

    // 画面動作制御用フラグ
    bool IsSwipeOutPlayMode = false;
    bool IsSwipeInStages = false;
    //------------

#region Start-Update
    // MAIN
    public void Start()
    {
        // 委譲
        _gameSceneMng = new GameSceneManager();
        _scoreManager = new ScoreManager(DataManager.service);

        // Top画面は回転したくない
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;

        // 情報更新フラグ
        updateFlag = true;
    }
    public void Update()
    {
        if(GameObject.Find("RankToggle").GetComponent<Toggle>().isOn)
        {
            UpdateRanks(GetRecords());
        }
        else if(GameObject.Find("WeaponToggle").GetComponent<Toggle>().isOn)
        {
            LoadWeaponResultFromCache();
        }

        // UIの更新(SWIPE)
        if(IsSwipeOutPlayMode) SwipeOutPlayModeUI();
        if(IsSwipeInStages) SwipeInStageUI();


    }
    //-----------
#endregion

#region GAME-UI
    public void GameStartButtonClicked()
    {
        var nextScene = GameScenes.Home;

        switch (GetActiveStageIndex())
        {
            case StageIndices.easy:
                DataManager.currentStage = new Stage(easyStage);
                nextScene = GameScenes.SeriousBalloon;
                break;
            case StageIndices.normal:
                DataManager.currentStage = new Stage(normalStage);
                nextScene = GameScenes.SeriousBalloon;
                break;
            case StageIndices.hard:
                DataManager.currentStage = new Stage(hardStage);
                nextScene = GameScenes.SeriousBalloon;
                break;
            default:
                break;
        }
        
        // 他の画面は回転してもOK
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        _gameSceneMng.ChangeScene(nextScene);
    }

    private StageIndices GetActiveStageIndex()
    {
        // Stages の toggle の状態を取得する
        var stg_easy = GameObject.Find("Easy").GetComponent<Toggle>();
        var stg_normal = GameObject.Find("Normal").GetComponent<Toggle>();
        var stg_hard = GameObject.Find("Hard").GetComponent<Toggle>();
        
        // なんもとれないとき
        var value = StageIndices.easy;

        if (stg_easy.isOn)
        {
            value = StageIndices.easy;
        }
        else if (stg_normal.isOn)
        {
            value = StageIndices.normal;
        }
        else if (stg_hard.isOn)
        {
            value = StageIndices.hard;
        }
        return value;
    }

    public void SingleModeButtonClicked()
    {
        // PlayModeのUIを消し、StageのUIを出す
        IsSwipeOutPlayMode = true;
        IsSwipeInStages = true;
    }

    public void PairModeButtonClicked()
    {
        var stage = new Stage(normalStage);
        stage.BalloonArrangementMode = Stage.ArrangementMode.Manual;
        DataManager.currentStage = stage;
        // 他の画面は回転してもOK
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        _gameSceneMng.ChangeScene(GameScenes.SeriousBalloon);
    }

    public void YarikomiModeButtonClicked()
    {
        var stage = new Stage(yarikomiStage_rank1);
        DataManager.currentStage = stage;
        // 他の画面は回転してもOK
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        _gameSceneMng.ChangeScene(GameScenes.SeriousBalloon);
    }

    private void SwipeOutPlayModeUI()
    {
        var playModeUI = GameObject.Find("PlayModes").GetComponent<RectTransform>();
        var left = playModeUI.offsetMin.x - 100;
        var right = playModeUI.offsetMax.x - 100;
        playModeUI.offsetMin = new Vector2(left, playModeUI.offsetMin.y);
        playModeUI.offsetMax = new Vector2(right, playModeUI.offsetMax.y);

        if (playModeUI.offsetMin.x < -1500 )
        {
            IsSwipeOutPlayMode = false;
        }
    }

    private void SwipeInStageUI()
    {
        var stageUI = GameObject.Find("Stages").GetComponent<RectTransform>();
        var left = stageUI.offsetMin.x - 100 < 0? 0 : stageUI.offsetMin.x - 100;
        var right = stageUI.offsetMax.x - 100;

        stageUI.offsetMin = new Vector2(left, stageUI.offsetMin.y);
        stageUI.offsetMax = new Vector2(right, stageUI.offsetMax.y);

        if (stageUI.offsetMin.x == 0 )
        {
            IsSwipeInStages = false;
            DescriptionUI.SetActive(true);
        }
    }

    // Swipe動作で移動した画面をもとに戻す
    public void InitializeSwipedUIs()
    {
        var playModeObj = GameObject.Find("PlayModes");
        if(playModeObj == null) return;
        var playModeUI = playModeObj.GetComponent<RectTransform>();
        playModeUI.offsetMin = new Vector2(0, playModeUI.offsetMin.y);
        playModeUI.offsetMax = new Vector2(0, playModeUI.offsetMax.y);
        IsSwipeOutPlayMode = false;

        var stageObj = GameObject.Find("Stages").GetComponent<RectTransform>();
        if (stageObj == null) return;
        var stageUI = stageObj.GetComponent<RectTransform>();
        stageUI.offsetMin = new Vector2(1500, stageUI.offsetMin.y);
        stageUI.offsetMax = new Vector2(1500, stageUI.offsetMax.y);
        IsSwipeInStages = false;

        DescriptionUI.SetActive(false);
    }
#endregion 

#region RANK-UI
    private List<Record> GetRecords()
    {
        var result = _scoreManager.GetAllRecords(out var records);
        if (result == DefinedErrors.Pass){
            if (records != null){
                return records;
            }
        }
        return null; // empty
    }

    private void UpdateRanks(List<Record> records)
    {
        if (records == null)
        {
            return ;
        }
        if (records.Count > 0 && updateFlag == true)
        {
            // 基本、固定で8個なのでベタ書きする
            var objName = "";
            for( var count = 0; count < records.Count && count < 8; ++count)
            {
                objName = "Score"+ (count+1);
                // 0個目:Label, 1個目:Score, 2個目:Name　の順にScoreにぶら下がっている。（これ崩さないでね・・・）
                GameObject.Find(objName).transform.GetChild(0).gameObject.GetComponent<Text>().text = (count+1).ToString();
                GameObject.Find(objName).transform.GetChild(1).gameObject.GetComponent<Text>().text = records[count].GameScore().ToString();
                GameObject.Find(objName).transform.GetChild(2).gameObject.GetComponent<Text>().text = records[count].UserName;
            }
            
            // 8個に満たない場合、使っていないランクを非表示にする
            if (records.Count < 8)
            {
                for (var i = records.Count; i < 8; ++i)
                {
                    objName = "Score" + (i+1);
                    GameObject.Find(objName).SetActive(false);
                }
            }
            updateFlag = false; // 更新完了
        }
    }

#endregion

#region WEAPON-UI
    private void LoadWeaponResultFromCache()
    {
        var stone = PlayerPrefs.GetInt(Weapons.Stone.ToString(), 0);
        if (stone == 1)
        {
            GameObject.Find("Weapon1").GetComponent<Image>().sprite = stone_on;
            // Stone Button Be Active
        }
        var hammer = PlayerPrefs.GetInt(Weapons.Hammer.ToString(), 0);
        if (hammer == 1)
        {
            GameObject.Find("Weapon2").GetComponent<Image>().sprite = hammer_on;
            // Hammer Button Be Active
        }
    }

    // Weapon番号でゲームスタートする
    // 番号は基本的にWeaponsのEnum定義どおりに使うこと
    public void WeaponGameStartButtonClicked(int weapon)
    {
        var nextScene = GameScenes.Home;
        switch((Weapons)weapon)
        {
            case Weapons.Stone:
                var stage = StageDefines.StoneStage;
                foreach (var position in StageDefines.StoneStageArrangement)
                {
                    stage.RegisterBalloonPosition(position);
                }
                stage.RegisterShootingWeaponKeys(new List<Weapons>(){Weapons.Stone});
                stage.SetStageDescription("すべてのバルーンを撃ち落としてみよう");
                DataManager.currentStage = stage;
                nextScene = GameScenes.SeriousBalloon;
                break;
            case Weapons.Hammer:
                break;
            default:
                break;
        }
        // 他の画面は回転してもOK
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        _gameSceneMng.ChangeScene(nextScene);
    }

#endregion

}
