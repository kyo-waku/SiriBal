using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;

[CreateAssetMenu(menuName = "ScriptableObject/StageData2")]
public class StageData2 : ScriptableObject
{
    public const string PATH = "StageList";
    private static StageData2 _entity;
    public static StageData2 Entity
    {
        get
        {
            //初アクセス時にロードする
            if (_entity == null)
            {
                _entity = Resources.Load<StageData2>(PATH);
                //ロード出来なかった場合はエラーログを表示
                if (_entity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _entity;
        }
    }
    public List<StageProperties> StageList = new List<StageProperties>();
}

[System.Serializable]
public class StageProperties
{
    public int BalloonLimit;
    public int BalloonHP;
    public int AttackSpan;
    public Weapons BalloonWeaponKey;
    public bool IsBalloonAction;
    public Stage.ArrangementMode BalloonArrangementMode;
    public int TimeLimit;
    public int ShootingLimit;
    public Stage.ClearCondition GameClearCondition;
    public int RankUpThreshold;
}