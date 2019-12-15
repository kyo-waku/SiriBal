using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBalloonGenerator : MonoBehaviour
{
    private GameDirector GameDirector;
    private GameModeController gameMode;
    public GameObject LoadingBalloonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        gameMode = GameObject.Find("ModeSwitcher").GetComponent<GameModeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMode.GameMode == GameModeController.eGameMode.Balloon || gameMode.GameMode == GameModeController.eGameMode.WaitTime)
        {
            //GameDirector.BalloonLimitはPublicにして良いか確認してから反映
            //if (GameDirector.BalloonCounter >= GameDirector.BalloonLimit || GameDirector.TimeValue <= 0 || Input.GetKeyDown(KeyCode.L))
            //Lキーはdebug用。
            if (GameDirector.BalloonCounter >= 30 || GameDirector.TimeValue <= 0 || Input.GetKeyDown(KeyCode.L))
            {
                GameDirector.BalloonCounter = 0;
                gameMode.GameMode = GameModeController.eGameMode.WaitTime;

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        GameObject LoadingBalloon = Instantiate(LoadingBalloonPrefab) as GameObject;
                        //UIのImageはCanvasの子じゃないとGUI表示されないため
                        var canvas = GameObject.FindObjectOfType<Canvas>();
                        LoadingBalloon.transform.SetParent(canvas.transform, false);

                        LoadingBalloon.transform.position = new Vector3(j * 150, (i * 150) - 2000, 0);
                    }
                }
            }
        }
    }
}
