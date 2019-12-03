using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class ScorebordSceneController : MonoBehaviour
{
    List<Record> _record;
    bool _recordChanged;
    // Start is called before the first frame update
    void Start()
    {
        _record = new List<Record>();
        _recordChanged = true;
    }

    // Update is called once per frame
    void Update()
    {
        var result = ScoreManager.GetAllRecords(out var records);
        if (result == DefinedErrors.Pass){
            if (records != null){
                if (records.Count != _record.Count) // あとでちゃんと内容比較に変えること
                {
                    _record = records;
                    _recordChanged = true;
                }
            }
        }

        if (_recordChanged)
        {
            UpdateRecords();
            _recordChanged = false;
        }
    }

    public void BackButtonClicked()
    {
        GameSceneManager.ChangeScene(GameScenes.Top);
    }

    public void UpdateRecords()
    {
        // Initialzed All texts
        InitializeRecordTexts();

        _record.Sort();
        _record.Reverse();

        var score1 = _record[0].GameScore();
        var name1 = _record[0].UserName;

        //1st
        var FirstRankName = GameObject.Find("1st/RankName");
        FirstRankName.GetComponent<Text>().text = name1.ToString();
        var FirstRankScore = GameObject.Find("1st/RankScore");
        FirstRankScore.GetComponent<Text>().text = score1.ToString();
        var FirstRankLabel = GameObject.Find("1st/RankLabel");
        FirstRankLabel.GetComponent<Text>().text = "No.1";

        var score2 = _record[1].GameScore();
        var name2 = _record[1].UserName;
        //2nd
        var SecondRankName = GameObject.Find("2nd/RankName");
        SecondRankName.GetComponent<Text>().text = name2.ToString();
        var SecondRankScore = GameObject.Find("2nd/RankScore");
        SecondRankScore.GetComponent<Text>().text = score2.ToString();
        var SecondRankLabel = GameObject.Find("2nd/RankLabel");
        SecondRankLabel.GetComponent<Text>().text = "No.2";
    }


    public void InitializeRecordTexts()
    {
        //1st
        var FirstRankName = GameObject.Find("1st/RankName");
        FirstRankName.GetComponent<Text>().text = "";
        var FirstRankScore = GameObject.Find("1st/RankScore");
        FirstRankScore.GetComponent<Text>().text = "";
        var FirstRankLabel = GameObject.Find("1st/RankLabel");
        FirstRankLabel.GetComponent<Text>().text = "";

        //2nd
        var SecondRankName = GameObject.Find("2nd/RankName");
        SecondRankName.GetComponent<Text>().text = "";
        var SecondRankScore = GameObject.Find("2nd/RankScore");
        SecondRankScore.GetComponent<Text>().text = "";
        var SecondRankLabel = GameObject.Find("2nd/RankLabel");
        SecondRankLabel.GetComponent<Text>().text = "";

        //3rd
        var ThirdRankName = GameObject.Find("3rd/RankName");
        ThirdRankName.GetComponent<Text>().text = "";
        var ThirdRankScore = GameObject.Find("3rd/RankScore");
        ThirdRankScore.GetComponent<Text>().text = "";
        var ThirdRankLabel = GameObject.Find("3rd/RankLabel");
        ThirdRankLabel.GetComponent<Text>().text = "";

        //4th
        var ForthRankName = GameObject.Find("4th/RankName");
        ForthRankName.GetComponent<Text>().text = "";
        var ForthRankScore = GameObject.Find("4th/RankScore");
        ForthRankScore.GetComponent<Text>().text = "";
        var ForthRankLabel = GameObject.Find("4th/RankLabel");
        ForthRankLabel.GetComponent<Text>().text = "";

        //5th
        var FifthRankName = GameObject.Find("5th/RankName");
        FifthRankName.GetComponent<Text>().text = "";
        var FifthRankScore = GameObject.Find("5th/RankScore");
        FifthRankScore.GetComponent<Text>().text = "";
        var FifthRankLabel = GameObject.Find("5th/RankLabel");
        FifthRankLabel.GetComponent<Text>().text = "";

    }
}
