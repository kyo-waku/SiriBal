using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;
using Generic.Firebase;

public class ScorebordSceneController : MonoBehaviour
{
    private List<Record> recordList;
    private bool recordChanged;
    private GameSceneManager gameSceneMng;
    private ScoreManager scoreMng;
    private readonly string[] rankLabels = {"1st", "2nd", "3rd", "4th", "5th"};
    private readonly string[] objNames = {"RankName", "RankScore", "RankLabel"};


    // Start is called before the first frame update
    void Start()
    {
        recordList = new List<Record>();
        recordChanged = true;
        gameSceneMng = new GameSceneManager();
        scoreMng = new ScoreManager(DataManager.service);
    }

    // Update is called once per frame
    void Update()
    {
        var result = scoreMng.GetAllRecords(out var records);
        if (result == DefinedErrors.Pass){
            if (records != null){
                if (records.Count != recordList.Count) // あとでちゃんと内容比較に変えること
                {
                    recordList = records;
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
        if (recordList == null){return;}
        if (recordList.Count > 0)
        {
            for (var i = 0; i < recordList.Count && i < rankLabels.Length ; ++i)
            {
                var score = recordList[i].GameScore();
                var name = recordList[i].UserName;
                
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
