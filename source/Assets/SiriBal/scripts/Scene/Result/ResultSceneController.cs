using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class ResultSceneController : MonoBehaviour
{
    public Record _myRecord;
    GameObject ResultScoreText;
    // Start is called before the first frame update
    void Start()
    {
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
        GameSceneManager.ChangeScene(GameScenes.SeriousBalloon);
    }

    public void RankingButtonClicked()
    {
        GameSceneManager.ChangeScene(GameScenes.ScoreBoard);
    }
    public void ToTopButtonClicked()
    {
        GameSceneManager.ChangeScene(GameScenes.Top);
    }
}
