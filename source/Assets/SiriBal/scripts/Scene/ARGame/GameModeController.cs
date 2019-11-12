﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeController : MonoBehaviour
{
    // Defines
    public enum eGameMode{
        Shooting,
        Balloon,
        WaitTime,
        None
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModeSwitcherButton()
    {
        switch(GameMode)
        {
            case eGameMode.None:
                GameMode = eGameMode.Balloon;
                break;
            case eGameMode.Balloon:
                GameMode = eGameMode.WaitTime;
                break;
            case eGameMode.WaitTime:
                GameMode = eGameMode.Shooting;
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