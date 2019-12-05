using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class ResultSceneController : MonoBehaviour
{
    public Record _myRecord;
    private GameSceneManager _gameSceneMng;
    private GameObject ResultScoreText;
    // Start is called before the first frame update
    void Start()
    {
        _gameSceneMng = new GameSceneManager();
        _myRecord = ScoreManager.MyLatestRecord;
        this.ResultScoreText = GameObject.Find("ResultScore");
        this.ResultScoreText.GetComponent<Text>().text= _myRecord.GameScore().ToString("F0")+" !!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartButtonClicked()
    {
        _gameSceneMng.ChangeScene(GameScenes.SeriousBalloon);
    }

    public void RankingButtonClicked()
    {
        _gameSceneMng.ChangeScene(GameScenes.ScoreBoard);
    }
    public void ToTopButtonClicked()
    {
        _gameSceneMng.ChangeScene(GameScenes.Top);
    }
}
