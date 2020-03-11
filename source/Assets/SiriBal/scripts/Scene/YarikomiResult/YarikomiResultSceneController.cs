using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;
using Generic.Manager;
public class YarikomiResultSceneController : MonoBehaviour
{
    private GameSceneManager gameSceneMng;
    public StageData yarikomiStage_rank1;

    void Start()
    {
        gameSceneMng = new GameSceneManager();
        // 回転したくない
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
    }

    void Update()
    {
        
    }


    public void RestartButtonClicked()
    {
        var stage = new Stage(yarikomiStage_rank1);
        DataManager.currentStage = stage;

        // 回転の制御を戻す
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        // シーン遷移
        gameSceneMng.ChangeScene(GameScenes.SeriousBalloon);
    }

    public void ToTopButtonClicked()
    {
        // 回転の制御を戻す
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        // シーン遷移
        gameSceneMng.ChangeScene(GameScenes.Home);
    }
}
