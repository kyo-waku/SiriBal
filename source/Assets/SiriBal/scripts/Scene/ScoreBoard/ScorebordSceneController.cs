using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;
using Generic.Firebase;

public class ScorebordSceneController : MonoBehaviour
{
    private List<GameObject> scoreBoards;
    private List<Record> recordList;
    private bool recordChanged;
    private GameSceneManager gameSceneMng;
    private ScoreManager scoreMng;
    private readonly string[] rankLabels = {"1st", "2nd", "3rd", "4th", "5th"};
    private readonly string[] objNames = {"RankName", "RankScore", "RankLabel", "RankBalloon", "RankTime"};

    private GameObject baseScoreObject;

    // Start is called before the first frame update
    void Start()
    {
        recordList = new List<Record>();
        recordChanged = true;
        gameSceneMng = new GameSceneManager();
        scoreMng = new ScoreManager(DataManager.service);
        
        // 一旦非表示
        baseScoreObject = GameObject.Find("Score");
        baseScoreObject.SetActive(false);

        // Score画面は回転したくない
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
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
        // 他の画面は回転してもOK
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        gameSceneMng.ChangeScene(GameScenes.Top);
    }

    public void UpdateRecords()
    {
        if (recordList == null){return;}
        if (recordList.Count > 0)
        {
            for (var i = 0; i < recordList.Count; ++i)
            {
                var score = recordList[i].GameScore();
                var balloon = recordList[i].BalloonScore;
                var time = recordList[i].TimeScore;
                var name = recordList[i].UserName;

                var texts = new Dictionary<string, string>()
                            {
                                {"RankName" , name},
                                {"RankScore" , score.ToString()},
                                {"RankLabel" , (i+1).ToString()},
                                {"RankBalloon", balloon.ToString()},
                                {"RankTime" , time.ToString()}
                            };

                var cloneObject = CloneScoreBoard(i+1); //　i+1 番目のスコアボード
                
                for ( var j = 0 ; j < objNames.Length; ++j)
                {
                    var childObject = cloneObject.transform.GetChild(j).gameObject;
                    texts.TryGetValue(childObject.name , out var text);
                    SetTexts(childObject, text);
                }
            }
        }
    }

    public GameObject CloneScoreBoard(int boardCount)
    {
        scoreBoards = new List<GameObject>();
        var basePosition = baseScoreObject.transform.position;
        var baseHeight = baseScoreObject.GetComponent<RectTransform>().sizeDelta.y;
        var canvas = GameObject.Find("Canvas");
        
        if (boardCount == 1)
        {
            baseScoreObject.SetActive(true);
            return baseScoreObject;
        }
        else
        {
            var position = new Vector3(basePosition.x, basePosition.y - baseHeight * (boardCount - 1), basePosition.z);
            return Instantiate(baseScoreObject, position, Quaternion.identity, canvas.transform);
        }
    }

    private void SetTexts(GameObject objectArg, string labelText)
    {
        objectArg.GetComponent<Text>().text = labelText;
    }
}
