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
        Home,
        WeaponChallenge,
    }

    // Error Handling
    public enum DefinedErrors{

        Pass = 0, // success.
        E_Fail, // generic fails. Not grouped.
        E_InvalidArg, // invalid argument.
        E_DuplicateKeys, // duplicate dictionary key.
        W_NotRequired, // in case the process not required. But that won't make any issues.
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
        public int GameScore(int alpha = 20, int beta = 10, int ganma = 5 ){
            return TimeScore * alpha + BalloonScore * beta + HitScore* ganma ;
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

    // ゲームシーンのステージ定義に使う
    public class Stage
    {
        // Defines ------

        // Balloon Arrangement Mode
        public enum ArrangementMode
        {
            Preset = 0, // Preset Arrangement
            Random, // Random Arrangement
            Manual, // User Arrangement 
        }

        // Properties ------
        public int BalloonLimit{get; set;}
        public int BalloonHP{get; set;}
        public GameObject BalloonWeapon{get; set;} // バルーンが攻撃してくるときのウェポン
        public bool IsBalloonAction{get; set;}
        public ArrangementMode BalloonArrangementMode{get; set;}
        public int TimeLimit{get; set;}
        public int ShootingLimit{get; set;}
        
        public bool isAvailable{
            get{
                // PresetとRandomは事前登録が必須
                if (BalloonArrangementMode == ArrangementMode.Preset || BalloonArrangementMode == ArrangementMode.Random)
                {
                    if (BalloonPositions.Count < BalloonLimit) //登録が足りない
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        // Constructor ------
        public Stage(int bLim = 10, int bHP = 3, GameObject bWp = null, 
                    bool isAct = true, ArrangementMode bArr = ArrangementMode.Manual, int tLim = 30, int sLim = 100)
        {
            BalloonLimit = bLim;
            BalloonHP = bHP;
            BalloonWeapon = bWp;
            IsBalloonAction = isAct;
            BalloonArrangementMode = bArr;
            TimeLimit = tLim;
            ShootingLimit = sLim;
        }

        // Members ------
        public List<Vector3> BalloonPositions{get; internal set;}
        public Dictionary<int, GameObject> ShootingWeaponDic{get; internal set;}
        public DefinedErrors RegisterBalloonPosition(Vector3 position)
        {
            // Manual Mode : Position not required (User will arrange balloons by their hand)
            if (BalloonArrangementMode == ArrangementMode.Manual) 
            {
                return DefinedErrors.W_NotRequired;
            }
            if (BalloonPositions == null)
            {
                BalloonPositions = new List<Vector3>();
            }
            if (BalloonLimit <= BalloonPositions.Count)
            {
                return DefinedErrors.W_NotRequired;
            }
            // Random Mode : Position will be decided randamly.
            if (BalloonArrangementMode == ArrangementMode.Random) 
            {
                // Make Random Position
            }

            if (position == null)
            {
                return DefinedErrors.E_InvalidArg;
            }

            BalloonPositions.Add(position);
            return DefinedErrors.Pass;
        }

        public DefinedErrors RegisterShootingWeapon(int key, GameObject weapon)
        {
            if (ShootingWeaponDic == null)
            {
                ShootingWeaponDic = new Dictionary<int, GameObject>();
            }
            if (ShootingWeaponDic.ContainsKey(key))
            {
                return DefinedErrors.E_DuplicateKeys;
            }
            ShootingWeaponDic.Add(key, weapon);
            return DefinedErrors.Pass;
        }
    }

}