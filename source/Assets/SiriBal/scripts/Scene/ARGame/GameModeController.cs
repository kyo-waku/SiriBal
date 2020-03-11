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
    private GameObject ShadeUI;

    // Start is called before the first frame update
    void Start()
    {
        GameMode = new eGameMode();
        GameMode = eGameMode.None;

        ShadeUI = GameObject.Find("Shade");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ModeSwitcherButton()
    {
        switch (GameMode)
        {
            case eGameMode.None:
                GameMode = eGameMode.Balloon;
                break;

            case eGameMode.Balloon:
                GameMode = eGameMode.WaitTime;
                break;

            case eGameMode.WaitTime:
                GameMode = eGameMode.Shooting;
                //float ShadeUIRed = ShadeUI.GetComponent<Image>().color.r;
                //float ShadeUIGreen = ShadeUI.GetComponent<Image>().color.g;
                //float ShadeUIBlue = ShadeUI.GetComponent<Image>().color.b;
                //ShadeUI.GetComponent<Image>().color = new Color(ShadeUIRed, ShadeUIGreen, ShadeUIBlue, 0.0f);
                break;

            case eGameMode.Shooting:
                GameMode = eGameMode.None;
                break;

            default:
                GameMode = eGameMode.None;
                break;
        }
    }
}
