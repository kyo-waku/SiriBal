using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeController : MonoBehaviour
{
    // Defines
    public enum eGameMode
    {
        Shooting,  // 投げる
        Balloon,  // バルーン配置
        WaitTime,  // 投げ前の待機
        BeforeResult, // 結果表示前の待機
        None  // 起点
    }

    // Values
    private eGameMode _gameMode;

    // Properties
    public eGameMode GameMode { get => _gameMode; internal set => _gameMode = value; }

    // Start is called before the first frame update
    void Start()
    {
        GameMode = new eGameMode();
        GameMode = eGameMode.None;
    }
}
