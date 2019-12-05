using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class ScorebordSceneController : MonoBehaviour
{
    private List<Record> record;
    private bool recordChanged;
    private GameSceneManager gameSceneMng;
    private ScoreManager scoreMng;
    private readonly string[] rankLabels = {"1st", "2nd", "3rd", "4th", "5th"};
    private readonly string[] objNames = {"RankName", "RankScore", "RankLabel"};


    // Start is called before the first frame update
    void Start()
    {
        record = new List<Record>();
        recordChanged = true;
        gameSceneMng = new GameSceneManager();
        scoreMng = new ScoreManager();
    }

    // Update is called once per frame
    void Update()
    {
        var result = scoreMng.GetAllRecords(out var records);
        if (result == DefinedErrors.Pass){
            if (records != null){
                if (records.Count != record.Count) // あとでちゃんと内容比較に変えること
                {
                    record = records;
                    recordChanged = true;
                }
            }
        }

        if (recordChanged)
        {
            UpdateRecords();
            recordChanged = false;
        }
    }

    public void BackButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.Top);
    }

    public void UpdateRecords()
    {
        // Initialzed All texts
        InitializeRecordTexts();
        if (record.Count > 0)
        {
            for (var i = 0; i < record.Count ; ++i)
            {
                var score = record[i].GameScore();
                var name = record[i].UserName;
                
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
