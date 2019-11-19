using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    //Difine Parameters
    GameObject TimerText;
    GameObject ScoreText;
    float time;
    int score = 0;

    GameModeController gameMode; //for ReadGameMode

    //CountScore
    public void DestroyCount(){
        this.score += 70;
    }

    public void HitCount(){
        this.score += 10;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        this.TimerText = GameObject.Find("Timer");
        this.ScoreText = GameObject.Find("Score");
        gameMode = GameObject.Find ("ModeSwitcher").GetComponent<GameModeController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameMode.GameMode)
        {
            case GameModeController.eGameMode.None:
                time = 30.0f;
                break;
            case GameModeController.eGameMode.Balloon:
                this.time -= Time.deltaTime;
                this.TimerText.GetComponent<Text>().text= this.time.ToString("F1");//F1 は書式指定子
                if(this.time<0) gameMode.GameMode=GameModeController.eGameMode.WaitTime;
                break;
            case GameModeController.eGameMode.WaitTime:
                time = 30.0f;
                break;
            case GameModeController.eGameMode.Shooting:
                this.ScoreText.GetComponent<Text>().text= this.score.ToString("F0");
                this.time -= Time.deltaTime;
                this.TimerText.GetComponent<Text>().text= this.time.ToString("F1");//F1 は書式指定子
                if(this.time<0) gameMode.GameMode=GameModeController.eGameMode.None;
                break;
        }
    }
}
