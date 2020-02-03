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
        GameDirector.LoadingBalloonPosMinY = GameDirector.LoadingBalloonPosMinYMinus;
        GameDirector.bJudgeGenerateLoadingBalloon = true;
        GameDirector.bJudgeUpdateLoadingBalloonPosMinY = false;
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
        float BasePosX = 0; //X方向の生成基準位置
        float BasePosY = -6400; //Y方向の生成基準位置
        float BasePosZ = 0; //Z方向の生成基準位置
        float IntervalX = 150; //X方向の生成間隔
        float IntervalY = 400; //Y方向の生成間隔

        while (count < TheNumberOfBalloons)
        {
            count++;
            GameObject LoadingBalloon = Instantiate(LoadingBalloonPrefab) as GameObject;
            //UIのImageはCanvasの子じゃないとGUI表示されないため
            var canvas = GameObject.FindObjectOfType<Canvas>();
            LoadingBalloon.transform.SetParent(canvas.transform, false);
            LoadingBalloon.transform.position = new Vector3(BasePosX + (Random.Range(MinColumn, MaxColumn) * IntervalX), + BasePosY + (LowNumber * IntervalY) + Random.Range(-IntervalY/2, IntervalY/2), BasePosZ);
        }
        count = 0;
    }
}
