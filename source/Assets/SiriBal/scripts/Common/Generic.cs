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
        WeaponResult,
        YarikomiResult,
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
    // GetRegisteredShootingWeaponsで全ウェポンを返すところがあるのでWeaponを増やしたらそこも増やすこと
    // バルーンが使うWeaponはちょっと挙動が違うのであとで別の定義に分けるかも・・・
    // 武器管理の参照先変更により削除してもよい？
    public enum Weapons{
        Missile = -1, // (プレイヤーの武器じゃないので別にしておきたい)
        None = 0,
        Stone,
        ColaCan,
        Shoes,
        Hammer,
    }

    //--------------
    // スコアの管理用クラス
    //--------------
    public class Record
    {
        // Members
        public string UserName{get;set;}
        public int TotalScore{get;set;}
        public Record(string name, int score)
        {
            UserName = name;
            TotalScore = score;
        }
    }

    //-------------
    // ゲームシーンのステージ定義用クラス
    //-------------
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

        // Game Clear Conditions
        public enum ClearCondition
        {
            None = 0,
            DestroyAll, 
            Yarikomi,
            // 他になんかあんのかな・・・
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
        public ClearCondition GameClearCondition{get; internal set;} = ClearCondition.None;
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
        public Stage(int bLim = 10, int bHP = 3, Weapons bWp = Weapons.Missile, 
                    bool isAct = true, ArrangementMode bArr = ArrangementMode.Manual, int tLim = 30, int sLim = 100,
                    ClearCondition condition = ClearCondition.None) // 互換性のために一旦おいておくけど本当は消したい！！また今度やる
        {
            BalloonLimit = bLim;
            BalloonHP = bHP;
            BalloonWeaponKey = bWp;
            IsBalloonAction = isAct;
            BalloonArrangementMode = bArr;
            TimeLimit = tLim;
            ShootingLimit = sLim;
            GameClearCondition = condition;
        }

        public Stage(StageProperties stage) // Scriptable Data (本当はメモリコピーせず参照だけ持っておきたい！！また今度やる。。。)
        {
            BalloonLimit = stage.BalloonLimit;
            BalloonHP = stage.BalloonHP;
            BalloonWeaponKey = stage.BalloonWeaponKey;
            IsBalloonAction = stage.IsBalloonAction;
            BalloonArrangementMode = stage.BalloonArrangementMode;
            TimeLimit = stage.TimeLimit;
            ShootingLimit = stage.ShootingLimit;
            GameClearCondition = stage.GameClearCondition;
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

        public DefinedErrors RegisterShootingWeaponKeys(List<Weapons> weapons)
        {
            if (ShootingWeapons == null)
            {
                ShootingWeapons = new List<Weapons>();
            }
            ShootingWeapons = weapons;
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
            if(ShootingWeapons == null || ShootingWeapons.Count <= 0)
            {
                // 登録がなければ全部使えるものとする
                // Weapon追加時は忘れずここにも追加してね
                // 武器管理の参照先変更により削除してもよい？
                weapons.Add(Weapons.Stone);
                weapons.Add(Weapons.ColaCan);
                weapons.Add(Weapons.Shoes);
                weapons.Add(Weapons.Hammer);
            }
            else
            {
                weapons = ShootingWeapons;
            }
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

    //--------------
    // ウェポン獲得ゲームの結果管理クラス
    //--------------
    public class WeaponResult
    {
        public bool ClearFlag {get; set;} = false;
        public Weapons Weapon{get; set;} = Weapons.None;
    }

    //--------------
    // オプション情報管理クラス
    //--------------
    public class GameOptions
    {
        // プロパティ
        public bool IsVibration{get; internal set;} = true; // Default is ON
    }

}