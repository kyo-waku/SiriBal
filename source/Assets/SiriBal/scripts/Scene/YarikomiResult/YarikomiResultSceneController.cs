using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;
public class YarikomiResultSceneController : MonoBehaviour
{
    private GameSceneManager gameSceneMng;
    private ScoreManager scoreMng;
    public StageData yarikomiStage_rank1;

    void Start()
    {
        gameSceneMng = new GameSceneManager();
        scoreMng = new ScoreManager(DataManager.service);
        // 回転したくない
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;

        ShowResult();
    }

    void Update()
    {
        
    }

    private void ShowResult()
    {
        var latestScore = scoreMng.LoadYarikomiLatestFromLocal();
        var bestScore = scoreMng.LoadYarikomiBestFromLocal();
        
        var resultObj = GameObject.Find("ResultScoreText");
        if (resultObj != null)
        {
            resultObj.GetComponent<Text>().text = latestScore.ToString();
        }

        var bestObj = GameObject.Find("BestScoreText");
        if (bestObj != null)
        {
            bestObj.GetComponent<Text>().text = bestScore.ToString();
        }
    }

    public void RestartButtonClicked()
    {
        var stage = new Stage(yarikomiStage_rank1);
        DataManager.currentStage = stage;

        // 回転の制御を戻す
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        // シーン遷移
        gameSceneMng.ChangeScene(GameScenes.SeriousBalloon);
    }

    public void ToTopButtonClicked()
    {
        // 回転の制御を戻す
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        // シーン遷移
        gameSceneMng.ChangeScene(GameScenes.Home);
    }
}
