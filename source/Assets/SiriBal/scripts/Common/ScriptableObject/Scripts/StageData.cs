using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;

[CreateAssetMenu(menuName = "ScriptableObject/StageData")]
public class StageData : ScriptableObject {
    public int BalloonLimit;
    public int BalloonHP;
    public Weapons BalloonWeaponKey;
    public bool IsBalloonAction;
    public Stage.ArrangementMode BalloonArrangementMode;
    public int TimeLimit;
    public int ShootingLimit;
    public Stage.ClearCondition GameClearCondition;
}