using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class HomeSceneController : MonoBehaviour
{
    private GameSceneManager _gameSceneMng;
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

        // Top画面は回転したくない
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
    }
    public void Update()
    {
        // not impl.
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
