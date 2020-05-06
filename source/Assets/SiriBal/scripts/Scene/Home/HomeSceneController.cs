using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class HomeSceneController : MonoBehaviour
{
    // COMMON
    private GameSceneManager _gameSceneMng;

    [SerializeField]
    Fade FadeObject;

    // STAGE DATA (Ref for scriptable objects)
    // NOTE: easy,normal,hard は現在使用していません
    // 
    // public StageData easyStage;
    // public StageData normalStage;
    // public StageData hardStage;

    //---------

    // RANK UI
    private ScoreManager _scoreManager;
    private bool rankUpdateFlag;
    //---------

    // WEAPON UI
    [SerializeField]
    GameObject WeaponPropertyDialog;
    private bool weaponLoadFlag;
    //--------


    // OPTIONS UI
    [SerializeField]
    GameObject VibrationToggleObject;

    //--------

    // GAME UI

    // NOTE: 現在使用していません
    // private enum StageIndices
    // {
    //     easy = 1,
    //     normal,
    //     hard,
    // }
    // NOTE: DescriptiopnUIはeasy, normal, hardの説明用だったため、一旦不要
    // [SerializeField]
    // GameObject DescriptionUI;
    [SerializeField]
    GameObject bestScoreValueText;
    // NOTE: 画面動作制御用フラグ ( 現在使用していません )
    // bool IsSwipeOutPlayMode = false;
    // bool IsSwipeInStages = false;
    //------------

#region Start-Update
    // MAIN
    public void Start()
    {
        // マネージャー系の委譲
        _gameSceneMng = new GameSceneManager();
        _scoreManager = new ScoreManager(DataManager.service);

        // Top画面は回転したくない
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;

        // 情報更新フラグ
        rankUpdateFlag = true;
        weaponLoadFlag = true;
        // ベストスコアを表示
        InitialzeBestScoreUI();
        // オプション画面の設定をキャッシュから取得して初期化する
        InitializeOptionsUI();
        // ランキング情報をサーバーから取得しておく（非同期）
        _scoreManager.GetAllRecordsFromDatabase();
    }
    public void Update()
    {
        if(GameObject.Find("RankToggle").GetComponent<Toggle>().isOn && rankUpdateFlag == true)
        {
            // ランキング画面の更新はUpdate処理で実施する。更新完了時は内部でフラグを立てて処理はスキップさせる。
            // NOTE: 1回きりのコールにしないのは、サーバーからの再取得処理を実装するとき、フラグの切り替えだけで完結するから。
            UpdateRanks(_scoreManager.GetRecords());
        }
        else if (GameObject.Find("WeaponToggle").GetComponent<Toggle>().isOn && weaponLoadFlag == true)
        {
            LoadWeaponResultFromCache();
        }
        // UIの更新(SWIPE): やりこみモード以外は一旦隠しているので、スワイプの動作も不要
        // if(IsSwipeOutPlayMode) SwipeOutPlayModeUI();
        // if(IsSwipeInStages) SwipeInStageUI();
    }
    //-----------
#endregion

#region GAME-UI

    // NOTE: お一人様モード用の処理なので、一旦コメントアウト
    // public void GameStartButtonClicked()
    // {
    //     var sceneChangeFlag = false;
    //     switch (GetActiveStageIndex())
    //     {
    //         case StageIndices.easy:
    //             DataManager.currentStage = new Stage(easyStage);
    //             sceneChangeFlag = true;
    //             break;
    //         case StageIndices.normal:
    //             DataManager.currentStage = new Stage(normalStage);
    //             sceneChangeFlag = true;
    //             break;
    //         case StageIndices.hard:
    //             DataManager.currentStage = new Stage(hardStage);
    //             sceneChangeFlag = true;
    //             break;
    //         default:
    //             break;
    //     }
        
    //     if(sceneChangeFlag)
    //     {
    //         GameStart();
    //     }
    // }

    // NOTE: お一人様モードの難易度チェックですが、現在使用していません
    // private StageIndices GetActiveStageIndex()
    // {
    //     // Stages の toggle の状態を取得する
    //     var stg_easy = GameObject.Find("Easy").GetComponent<Toggle>();
    //     var stg_normal = GameObject.Find("Normal").GetComponent<Toggle>();
    //     var stg_hard = GameObject.Find("Hard").GetComponent<Toggle>();
        
    //     // なんもとれないとき
    //     var value = StageIndices.easy;

    //     if (stg_easy.isOn)
    //     {
    //         value = StageIndices.easy;
    //     }
    //     else if (stg_normal.isOn)
    //     {
    //         value = StageIndices.normal;
    //     }
    //     else if (stg_hard.isOn)
    //     {
    //         value = StageIndices.hard;
    //     }
    //     return value;
    // }

    // NOTE: おひとり様モード（かんたん・ふつう・むずかしい）は現在使用していません
    // public void SingleModeButtonClicked()
    // {
    //     // PlayModeのUIを消し、StageのUIを出す
    //     IsSwipeOutPlayMode = true;
    //     IsSwipeInStages = true;
    // }

    // NOTE: おふたりさまモードも現在使用していません
    // public void PairModeButtonClicked()
    // {
    //     var stage = new Stage(normalStage);
    //     stage.BalloonArrangementMode = Stage.ArrangementMode.Manual;
    //     DataManager.currentStage = stage;
    //     // 他の画面は回転してもOK
    //     GameStart();
    // }

    public void YarikomiModeButtonClicked()
    {
        var stage = new Stage(StageData2.Entity.StageList[0]);
        DataManager.currentStage = stage;
        GameStart();
    }

    // NOTE: スワイプアウトはおひとりさまモード用で、現在は使用していません
    // private void SwipeOutPlayModeUI()
    // {
    //     var playModeUI = GameObject.Find("PlayModes").GetComponent<RectTransform>();
    //     var left = playModeUI.offsetMin.x - 100;
    //     var right = playModeUI.offsetMax.x - 100;
    //     playModeUI.offsetMin = new Vector2(left, playModeUI.offsetMin.y);
    //     playModeUI.offsetMax = new Vector2(right, playModeUI.offsetMax.y);

    //     if (playModeUI.offsetMin.x < -1500 )
    //     {
    //         IsSwipeOutPlayMode = false;
    //     }
    // }

    // NOTE: スワイプインはおひとりさまモード用で、現在は使用していません
    // private void SwipeInStageUI()
    // {
    //     var stageUI = GameObject.Find("Stages").GetComponent<RectTransform>();
    //     var left = stageUI.offsetMin.x - 100 < 0? 0 : stageUI.offsetMin.x - 100;
    //     var right = stageUI.offsetMax.x - 100;

    //     stageUI.offsetMin = new Vector2(left, stageUI.offsetMin.y);
    //     stageUI.offsetMax = new Vector2(right, stageUI.offsetMax.y);

    //     if (stageUI.offsetMin.x == 0 )
    //     {
    //         IsSwipeInStages = false;
    //         DescriptionUI.SetActive(true);
    //     }
    // }


    // NOTE: スワイプ動作は現在使用していません
    // Swipe動作で移動した画面をもとに戻す
    // public void InitializeSwipedUIs()
    // {
    //     var playModeObj = GameObject.Find("PlayModes");
    //     if(playModeObj == null) return;
    //     var playModeUI = playModeObj.GetComponent<RectTransform>();
    //     playModeUI.offsetMin = new Vector2(0, playModeUI.offsetMin.y);
    //     playModeUI.offsetMax = new Vector2(0, playModeUI.offsetMax.y);
    //     IsSwipeOutPlayMode = false;

    //     var stageObj = GameObject.Find("Stages").GetComponent<RectTransform>();
    //     if (stageObj == null) return;
    //     var stageUI = stageObj.GetComponent<RectTransform>();
    //     stageUI.offsetMin = new Vector2(1500, stageUI.offsetMin.y);
    //     stageUI.offsetMax = new Vector2(1500, stageUI.offsetMax.y);
    //     IsSwipeInStages = false;

    //     DescriptionUI.SetActive(false);
    // }

    private void GameStart()
    {
        UpdateCurrentVibrationOption();
        // 他の画面は回転してもOK
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        FadeWithChangeScene();
    }

    private void FadeWithChangeScene()
    {
		FadeObject.FadeIn (1, () => {
			Invoke("GameSceneStart",0.1f);
		});
	}

    // NOTE: 参照先が無い関数に見えますが、FadeWIthChangeSceneから時差呼び出しされています。
	public void GameSceneStart()
    {
		_gameSceneMng.ChangeScene(GameScenes.SeriousBalloon);
    }

    private void InitialzeBestScoreUI()
    {
        if(bestScoreValueText != null)
        {
            var bestScore = _scoreManager.LoadYarikomiBestFromLocal();
            bestScoreValueText.GetComponent<Text>().text = bestScore.ToString();
        }
    }
#endregion

#region RANK-UI
    private void UpdateRanks(List<Record> records)
    {
        if (records == null)
        {
            return ;
        }
        if (records.Count > 0)
        {
            // 基本、固定で8個なのでベタ書きする
            var objName = "";
            for( var count = 0; count < records.Count && count < 8; ++count)
            {
                objName = "Score"+ (count+1);
                // 0個目:Label, 1個目:Score, 2個目:Name　の順にScoreにぶら下がっている。（これ崩さないでね・・・）
                GameObject.Find(objName).transform.GetChild(0).gameObject.GetComponent<Text>().text = (count+1).ToString();
                GameObject.Find(objName).transform.GetChild(1).gameObject.GetComponent<Text>().text = records[count].TotalScore.ToString();
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
            rankUpdateFlag = false; // 更新完了
        }
    }

#endregion

#region WEAPON-UI
    private void LoadWeaponResultFromCache()
    {
        foreach(var weapon in WeaponData.Entity.HeroWeaponList)
        {
            var isWeaponAcquired = weapon.IsWeaponAcquired;
            var objName = "Weapon" + (int)weapon.WeaponID;
            var obj = GameObject.Find(objName);
            if (obj != null)
            {
                if (weapon != null)
                {
                    obj.GetComponent<Image>().sprite = (isWeaponAcquired)? weapon.ImageOn: weapon.ImageOff;
                }
            }
        }
        // 読み込み完了
        weaponLoadFlag = false;
    }

    // Weapon の詳細を表示
    // WeaponDataで管理されているウェポンIDをセットする
    public void ShowWeaponPropertyDialog(int weaponId)
    {
        WeaponPropertyDialog.SetActive(true);
        ShowCurrentWeaponInformation(weaponId);
    }

    private void ShowCurrentWeaponInformation(int weaponId)
    {
        var weapon = WeaponData.Entity.HeroWeaponList.Where(x=>x.WeaponID == (WeaponIds)weaponId).First();
        if (weapon == null)
        {
            // こんなことはないはずだが、壊れると嫌なので、1つめのweaponをセットしておく
            weapon = WeaponData.Entity.HeroWeaponList[0];
        }
        // 獲得済みかどうかで、表示は切り替えるべき
        SetWeaponInformationToObject(weapon, weapon.IsWeaponAcquired);
    }

    private void SetWeaponInformationToObject(HeroWeaponStatus weapon, bool isWeaponAcquired)
    {
        // ウェポン名
        var weaponName = GameObject.Find("WeaponName");
        if (weaponName != null)
        {
            weaponName.GetComponent<Text>().text = isWeaponAcquired? weapon.Name: "????";
        }
        // ウェポン画像
        var weaponImage = GameObject.Find("WeaponImage");
        if (weaponImage != null)
        {
            weaponImage.GetComponent<Image>().sprite = isWeaponAcquired? weapon.ImageOn: weapon.ImageOff;
        }
        // ウェポンの説明
        var weaponExplanation = GameObject.Find("WeaponExplanation");
        if (weaponExplanation != null)
        {
            weaponExplanation.GetComponent<Text>().text = isWeaponAcquired? weapon.Explanation: weapon.WeaponGetCondition;
        }
        // レーダーチャート
        var radarPoly = GameObject.Find("RadarPoly");
        if (radarPoly != null)
        {
            var radar = radarPoly.GetComponent<RadarChartController>();
            if (radar != null)
            {
                radar.Volumes = isWeaponAcquired?
                                new float[]{
                                            (float)weapon.Attack/5,
                                            (float)weapon.Scale/5,
                                            (float)weapon.Distance/5,
                                            (float)weapon.Penetrate/5,
                                            (float)weapon.Rapidfire/5,
                                            }
                                :
                                new float[]{0f,0f,0f,0f,0f};
            }
        }
        var notFound = GameObject.Find("NotAcquiredImage");
        if (notFound != null)
        {
            notFound.GetComponent<Text>().text = isWeaponAcquired? "": "?";
        }
    }

    public void CloseWeaponPropertyButtonClicked()
    {
        Invoke("CloseWeaponPropertyDialog",0.1f);
    }
    private void CloseWeaponPropertyDialog()
    {
        WeaponPropertyDialog.SetActive(false);
    }
#endregion

#region OPTION-UI
    private void InitializeOptionsUI()
    {
        var VibrationOnToggle = VibrationToggleObject.GetComponent<Toggle>();
        var VibrationCache = PlayerPrefs.GetInt("Vibration", 1); // 0:OFF , 1:ON
        if (VibrationCache == 0) // OFF
        {
            VibrationOnToggle.isOn = false;
        }
        else
        {
            VibrationOnToggle.isOn = true;
        }
    }

    public void UpdateCurrentVibrationOption()
    {
        var VibrationOnToggle = VibrationToggleObject.GetComponent<Toggle>();
        if (VibrationOnToggle != null)
        {
            if (DataManager.options == null)
            {
                DataManager.options = new GameOptions();
            }
            DataManager.options.IsVibration = VibrationOnToggle.isOn;

            if (DataManager.options.IsVibration)
            {
                PlayerPrefs.SetInt("Vibration",1);
            }
            else
            {
                PlayerPrefs.SetInt("Vibration",0);
            }
            PlayerPrefs.Save();
        }
    }


#endregion

}