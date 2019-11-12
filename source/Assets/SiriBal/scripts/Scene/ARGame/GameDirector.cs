using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    //Difine Parameters
    GameObject TimerText;
    GameObject ScoreText;
    float time = 60.0f;
    int score = 0;

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
    }

    // Update is called once per frame
    void Update()
    {
        this.time -= Time.deltaTime;
        this.TimerText.GetComponent<Text>().text= this.time.ToString("F1");//F1 は書式指定子
        this.ScoreText.GetComponent<Text>().text= this.score.ToString("F0");
    }
}
