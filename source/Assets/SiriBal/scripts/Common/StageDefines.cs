using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generic
{
    public class StageDefines
    {

#region Predefined
        public static Stage easyStage = new Stage(
            5, // BalloonLimit
            3, // Balloon HP
            Weapons.None, // Balloon Weapon
            false, // Balloon Action
            Stage.ArrangementMode.Random, // ランダム配置
            30, // 時間制限
            100 // 投げ数上限
        );

        public static Stage normalStage = new Stage(
            10, // BalloonLimit
            3, // Balloon HP
            Weapons.Missile, // Balloon Weapon
            false, // Balloon Action
            Stage.ArrangementMode.Random, // ランダム配置
            30, // 時間制限
            100 // 投げ数上限
        );

        public static Stage hardStage = new Stage(
            15, // BalloonLimit
            5, // Balloon HP
            Weapons.Missile, // Balloon Weapon
            true, // Balloon Action
            Stage.ArrangementMode.Random, // ランダム配置
            30, // 時間制限
            100 // 投げ数上限
        );

#endregion

#region Yarikomi
        public static Stage yarikomiStage = new Stage(
            10, // BalloonLimit < 0 means YARIKOMI MODE
            1, // Balloon HP
            Weapons.None, // Balloon Weapon
            false, // Balloon Action
            Stage.ArrangementMode.Random, // ランダム配置
            -1, // 時間制限 < 0 YARIKOMI MODE
            -1, // 投げ数上限 < 0 YARIKOMI MODE
            Stage.ClearConditions.Yarikomi // やりこみモード
        );
        public static Stage yarikomi_rank2 = new Stage(
            15, 3, Weapons.None, false, Stage.ArrangementMode.Random, -1, -1, Stage.ClearConditions.Yarikomi
        );
        public static Stage yarikomi_rank3 = new Stage(
            20, 5, Weapons.Missile, true, Stage.ArrangementMode.Random, -1, -1, Stage.ClearConditions.Yarikomi
        );


#endregion

#region WeaponGames

        // Weapon系ゲーム
        public static Stage StoneStage = new Stage(
            3,
            1,
            Weapons.None,
            false,
            Stage.ArrangementMode.Preset,
            100,
            3,
            Stage.ClearConditions.DestroyAll
        );

        // CameraPositionに加えて使うこと
        public static List<Vector3> StoneStageArrangement = new List<Vector3>(){
            new Vector3(0,0,3),
            new Vector3(-1,0,3),
            new Vector3(1,0,3)
        };

#endregion

    }
}
