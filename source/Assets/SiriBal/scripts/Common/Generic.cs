using System.Collections;
using UnityEngine;
using System;

namespace Generic
{
    // GameScenes
    public enum GameScenes{
        None = 0,
        Top,
        SeriousBalloon,
        Result,
        ScoreBoard,
        Login,
        UserRegistered,
    }

    // Error Handling
    public enum DefinedErrors{

        Pass = 0, // success.
        E_Fail, // generic fails. Not grouped.
        E_InvalidArg, // invalid argument.
    }

    // Score Management
    public class Record{
        // Members
        string UserName{get;set;}
        
        //-- Time result
        int TimeScore{get;set;}

        //-- Balloon result
        int BalloonScore{get;set;}

        // Date and Time
        DateTime PlayDateTime{get;set;}

        // Methods
        //-- Get/Set
        public int GameScore(int alpha = 100, int beta = 10){
            return TimeScore * alpha + BalloonScore * beta;
        }

        public Record(string name, int time, int balloon, DateTime date)
        {
            UserName = name;
            TimeScore = time;
            BalloonScore = balloon;
            PlayDateTime = date;
        }
    }
}