﻿using System.Collections;
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
        if (records.Count > 0)
        {
            // 基本、固定で8個なのでベタ書きする
            var objName = "";
            for( var count = 0; count < records.Count && count < 8; ++count)
            {
                objName = "Score"+ (count+1);
                // 0個目:Label, 1個目:Score, 2個目:Name　の順にScoreにぶら下がっている。（これ崩さないでね・・・）
                var obj1 = GameObject.Find(objName).transform.GetChild(1).gameObject.GetComponent<Text>().text = records[count].GameScore().ToString();
                var obj2 = GameObject.Find(objName).transform.GetChild(2).gameObject.GetComponent<Text>().text = records[count].UserName;
            }
        }
    }

    public void GameStartButtonClicked()
    {
        var nextScene = GameScenes.Home;

        switch (GetActiveStageIndex())
        {
            case StageIndices.easy:
            case StageIndices.normal:
            case StageIndices.hard:
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
