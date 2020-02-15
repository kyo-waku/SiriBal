using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generic
{
    public class StageDefines
    {
        public static Stage easyStage = new Stage(
            5, // BalloonLimit
            3, // Balloon HP
            Weapons.missile, // Balloon Weapon
            true, // Balloon Action
            Stage.ArrangementMode.Random, // ランダム配置
            30, // 時間制限
            100 // 投げ数上限
        );

        public static Stage normalStage = new Stage(
            10, // BalloonLimit
            3, // Balloon HP
            Weapons.missile, // Balloon Weapon
            true, // Balloon Action
            Stage.ArrangementMode.Random, // ランダム配置
            30, // 時間制限
            100 // 投げ数上限
        );

        public static Stage hardStage = new Stage(
            15, // BalloonLimit
            5, // Balloon HP
            Weapons.missile, // Balloon Weapon
            true, // Balloon Action
            Stage.ArrangementMode.Random, // ランダム配置
            30, // 時間制限
            100 // 投げ数上限
        );
    }
}
