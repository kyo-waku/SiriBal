using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBalloonGenerator : MonoBehaviour
{
    private GameDirector GameDirector;
    private GameModeController gameMode;
    public GameObject LoadingBalloonPrefab;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        gameMode = GameObject.Find("ModeSwitcher").GetComponent<GameModeController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateLoadingBalloons()
    {
        //GameDirector.BalloonCounter = 0;
        GameDirector.MinLoadingBalloonPosY = -1.0f;
        GameDirector.bMinLoadingBalloonPosYJudge1 = true;
        GameDirector.bMinLoadingBalloonPosYJudge2 = false;
        GenerateLoadingBalloonLine(32, 1, 0, 8);
        GenerateLoadingBalloonLine(32, 2, 0, 8);
        GenerateLoadingBalloonLine(32, 3, 0, 8);
        GenerateLoadingBalloonLine(32, 4, 0, 8);
        GenerateLoadingBalloonLine(32, 5, 0, 8);
        GenerateLoadingBalloonLine(16, 6, 1, 7);
        GenerateLoadingBalloonLine(8, 7, 1, 7);
        GenerateLoadingBalloonLine(4, 8, 1, 7);
        GenerateLoadingBalloonLine(2, 11, 2, 6);
        GenerateLoadingBalloonLine(1, 14, 3, 5);
    }

    private void GenerateLoadingBalloonLine(int TheNumberOfBalloons, int LowNumber, int MinColumn, int MaxColumn)
    {
        while (count < TheNumberOfBalloons)
        {
            count++;
            GameObject LoadingBalloon = Instantiate(LoadingBalloonPrefab) as GameObject;
            //UIのImageはCanvasの子じゃないとGUI表示されないため
            var canvas = GameObject.FindObjectOfType<Canvas>();
            LoadingBalloon.transform.SetParent(canvas.transform, false);
            LoadingBalloon.transform.position = new Vector3((Random.Range(MinColumn, MaxColumn) * 150), (LowNumber * 400) - 6400 + Random.Range(-200, 200), 0);
        }
        count = 0;
    }
}
