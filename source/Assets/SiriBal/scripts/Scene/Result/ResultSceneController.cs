using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class ResultSceneController : MonoBehaviour
{
    private Record _myRecord;
    private GameSceneManager gameSceneMng;
    private GameObject resultScoreText;
    // Start is called before the first frame update
    void Start()
    {
        gameSceneMng = new GameSceneManager();
        
        _myRecord = DataManager.MyLatestRecord;

        resultScoreText = GameObject.Find("ResultScore");
        resultScoreText.GetComponent<Text>().text= _myRecord.GameScore().ToString("F0")+" !!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.SeriousBalloon);
    }

    public void RankingButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.ScoreBoard);
    }
    public void ToTopButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.Top);
    }
}
