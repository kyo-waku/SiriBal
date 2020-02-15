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
        E_Duplicate, // duplicate registration.
        W_NotRequired, // in case the process not required. But that won't make any issues.
    }

    // 使用可能なウェポンのキー登録
    // ウェポンのゲームオブジェクト自体はゲームシーンで保持して管理すること
    public enum Weapons{
        Stone = 0,
        Hammer,
        missile,
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
        public int BalloonLimit{get; internal set;}
        public int BalloonHP{get; internal set;}
        public Weapons BalloonWeaponKey{get; internal set;} // バルーンが攻撃してくるときのウェポンのキー
        public bool IsBalloonAction{get; internal set;}
        public ArrangementMode BalloonArrangementMode{get; internal set;}
        public int TimeLimit{get; internal set;}
        public int ShootingLimit{get; internal set;}
        public string StageDescription{get; internal set;} = "Play Serious Balloon";
        
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
        public Stage(int bLim = 10, int bHP = 3, Weapons bWp = Weapons.missile, 
                    bool isAct = true, ArrangementMode bArr = ArrangementMode.Manual, int tLim = 30, int sLim = 100)
        {
            BalloonLimit = bLim;
            BalloonHP = bHP;
            BalloonWeaponKey = bWp;
            IsBalloonAction = isAct;
            BalloonArrangementMode = bArr;
            TimeLimit = tLim;
            ShootingLimit = sLim;
        }

        // Members ------
        public List<Vector3> BalloonPositions{get; internal set;}
        public List<Weapons> ShootingWeapons{get; internal set;}
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

        public DefinedErrors RegisterShootingWeapon(Weapons weapon)
        {
            if (ShootingWeapons == null)
            {
                ShootingWeapons = new List<Weapons>();
            }
            if (ShootingWeapons.Contains(weapon))
            {
                return DefinedErrors.E_Duplicate;
            }
            ShootingWeapons.Add(weapon);
            return DefinedErrors.Pass;
        }

        public DefinedErrors GetRegisteredPositions(out List<Vector3> positions)
        {
            positions = new List<Vector3>();
            if(BalloonPositions == null) // 登録なし
            {
                return DefinedErrors.E_Fail;
            }
            if(BalloonPositions.Count != BalloonLimit) // 登録数が足りない
            {
                return DefinedErrors.E_Fail;
            }
            positions = BalloonPositions;
            return DefinedErrors.Pass;
        }

        public DefinedErrors GetRegisteredShootingWeapons(out List<Weapons> weapons)
        {
            weapons = new List<Weapons>();
            if(ShootingWeapons == null) // 登録なし
            {
                return DefinedErrors.E_Fail;
            }
            if(ShootingWeapons.Count > 0) // 登録数が足りない
            {
                return DefinedErrors.E_Fail;
            }
            weapons = ShootingWeapons;
            return DefinedErrors.Pass;
        }

        // ステージの説明文を登録する
        public void SetStageDescription(string description)
        {
            if (description != null)
            {
                StageDescription = description;
            }
        }
    }

}