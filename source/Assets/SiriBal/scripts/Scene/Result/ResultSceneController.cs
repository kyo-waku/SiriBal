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
    private GameObject Result_DestroyRateText;
    private GameObject Result_TimeScoreText;
    private GameObject Result_HitPropabilityText;
    // Start is called before the first frame update
    void Start()
    {
        gameSceneMng = new GameSceneManager();
        
        _myRecord = DataManager.MyLatestRecord;

        resultScoreText = GameObject.Find("ResultScore");
        resultScoreText.GetComponent<Text>().text = _myRecord.GameScore().ToString("F0")+" !!";
        Result_DestroyRateText = GameObject.Find("Result_DestroyRate");
        Result_DestroyRateText.GetComponent<Text>().text = ((_myRecord.BalloonScore)/10).ToString("F0")+" %";
        Result_TimeScoreText = GameObject.Find("Result_TimeScore");
        Result_TimeScoreText.GetComponent<Text>().text = ((_myRecord.TimeScore)/10).ToString("F0")+" %";
        Result_HitPropabilityText = GameObject.Find("Result_HitPropability");
        Result_HitPropabilityText.GetComponent<Text>().text = ((_myRecord.HitScore)/10).ToString("F0")+" %";

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
        gameSceneMng.ChangeScene(GameScenes.Home);
    }
    public void ToTopButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.Home);
    }
}
