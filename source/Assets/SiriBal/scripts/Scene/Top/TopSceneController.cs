using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class TopSceneController : MonoBehaviour
{
    public void Start()
    {
        // Top画面は回転したくない
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
    }
    public void Update()
    {

    }
    public void PlayButtonClicked()
    {
        // 他の画面は回転してもOK
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        GameSceneManager.ChangeScene(GameScenes.SeriousBalloon);
    }

    public void LankingButtonClicked()
    {
        // 他の画面は回転してもOK
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        GameSceneManager.ChangeScene(GameScenes.ScoreBoard);
    }
    
    public void OptionsButtonClicked(){
        // Not impl
    }
}
