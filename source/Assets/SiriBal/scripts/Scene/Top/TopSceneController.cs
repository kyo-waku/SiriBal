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

    }
    public void Update()
    {

    }
    public void PlayButtonClicked()
    {
        GameSceneManager.ChangeScene(GameScenes.SeriousBalloon);
    }

    public void LankingButtonClicked()
    {
        GameSceneManager.ChangeScene(GameScenes.ScoreBoard);
    }
    
    public void OptionsButtonClicked(){
        // Not impl
    }
}
