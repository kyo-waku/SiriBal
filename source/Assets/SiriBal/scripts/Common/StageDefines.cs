using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generic
{
    public class StageDefines
    {
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
            Stage.ClearCondition.DestroyAll
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
