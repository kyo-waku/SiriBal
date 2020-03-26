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

    [SerializeField]
    private GameObject NameInput;
    [SerializeField]
    private GameObject UserNameInputTextObject;
    // Start is called before the first frame update
    void Start()
    {
        gameSceneMng = new GameSceneManager();
        
        _myRecord = DataManager.MyLatestRecord;
        NameInput.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.SeriousBalloon);
    }

    public void RegisterButtonClicked()
    {
        NameInput.SetActive(true);
    }
    public void ToTopButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.Home);
    }

    public void OkayButtonClicked()
    {
        var scoreMng = new ScoreManager(DataManager.service);
        var userName = UserNameInputTextObject.GetComponent<Text>().text;
        var score = DataManager.MyLatestRecord;
        if (score != null)
        {
            score.UserName = userName;
            //scoreMng.RegisterRecord(score); //最新の結果を名前付きで送る
        }
        NameInput.SetActive(false);
    }
}
