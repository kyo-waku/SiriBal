using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class HomeSceneController : MonoBehaviour
{
    private GameSceneManager _gameSceneMng;
    private ScoreManager _scoreManager;
    private List<Record> _records;

    private bool updateFlag;
    private enum StageIndices
    {
        easy = 1,
        normal,
        hard,
    }
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
    }

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

    public void GameStartButtonClicked()
    {
        var nextScene = GameScenes.Home;

        switch (GetActiveStageIndex())
        {
            case StageIndices.easy:
                DataManager.currentStage = StageDefines.easyStage;
                nextScene = GameScenes.SeriousBalloon;
                break;
            case StageIndices.normal:
                DataManager.currentStage = StageDefines.normalStage;
                nextScene = GameScenes.SeriousBalloon;
                break;
            case StageIndices.hard:
                DataManager.currentStage = StageDefines.hardStage;
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
}
