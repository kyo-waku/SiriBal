using System.Collections.Generic;
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
    public class Record: IComparable
    {
        // Members
        public string UserName{get;set;}
        
        //-- Time result
        public int TimeScore{get;set;}

        //-- Balloon result
        public int BalloonScore{get;set;}

        public int HitScore{get;set;}

        // Date and Time
        public DateTime PlayDateTime{get;set;}

        // Methods
        //-- Get/Set
        public int GameScore(int alpha = 10, int beta = 10){
            return TimeScore * alpha + BalloonScore * beta;
        }

        public Record(string name, int time, int balloon, int hit, DateTime date)
        {
            UserName = name;
            TimeScore = time;
            BalloonScore = balloon;
            HitScore = hit;
            PlayDateTime = date;
        }

            
        public int CompareTo(object obj) 
        {
            Record i = obj as Record;
            return this.GameScore().CompareTo(i.GameScore());
        }
    }
}