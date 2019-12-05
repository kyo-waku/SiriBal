using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class ScorebordSceneController : MonoBehaviour
{
    private List<Record> _record;
    private bool _recordChanged;
    private GameSceneManager _gameSceneMng;

    private readonly string[] rankLabels = {"1st", "2nd", "3rd", "4th", "5th"};
    private readonly string[] objNames = {"RankName", "RankScore", "RankLabel"};


    // Start is called before the first frame update
    void Start()
    {
        _record = new List<Record>();
        _recordChanged = true;
        _gameSceneMng = new GameSceneManager();
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
        _gameSceneMng.ChangeScene(GameScenes.Top);
    }

    public void UpdateRecords()
    {
        // Initialzed All texts
        InitializeRecordTexts();

        _record.Sort();
        _record.Reverse();

        if (_record.Count > 0)
        {
            for (var i = 0; i < _record.Count ; ++i)
            {
                var score = _record[i].GameScore();
                var name = _record[i].UserName;
                
                SetTexts(rankLabels[i] + "/" + objNames[0], name);
                SetTexts(rankLabels[i] + "/" + objNames[1], score.ToString());
                SetTexts(rankLabels[i] + "/" + objNames[2], "No." + (i+1) );
            }
        }
    }


    public void InitializeRecordTexts()
    {
        foreach(var label in rankLabels)
        {
            foreach(var name in objNames)
            {
                SetTexts(label + "/" + name, "");
            }
        }
    }

    private void SetTexts(string objectName, string labelText)
    {
        var scoreLabel = GameObject.Find(objectName);
        scoreLabel.GetComponent<Text>().text = labelText;
    }
}
