using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Generic;

[CreateAssetMenu(menuName = "ScriptableObject/WeaponData2")]
public class WeaponData2 : ScriptableObject
{

    //PATHはWeaponDataで作成したAssetが保存してある場所のパス。
    //HACK:Resources.Load使用のため、Resourcesフォルダ内である必要がある。
    //Resourcesフォルダから移動させた場合はPATHを変更すること
    //例「Resources¥a.asset」の場合は「PATH = "a"」
    public const string PATH = "WeaponList";

    //WeaponDataを他の場所から参照できるように、ScriptableObjectの実体を定義。
    private static WeaponData2 _entity;
    public static WeaponData2 Entity
    {
        get
        {
            //初アクセス時にロードする
            if (_entity == null)
            {
                _entity = Resources.Load<WeaponData2>(PATH);
                //ロード出来なかった場合はエラーログを表示
                if (_entity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _entity;
        }
    }
    public List<HeroWeaponStatus> HeroWeaponList = new List<HeroWeaponStatus>();
    //todo:敵の武器の参照はまだ未実装。あとで実装する。
    public List<EnemyWeaponStatus> EnemyWeaponList = new List<EnemyWeaponStatus>();

    // ウェポン獲得状況の更新
    // スコアで決まるなら、スコアをもらうだけでいい。
    // 特別な条件をフォローするために、id指定でも対応できるようにしている
    public static void UpdateWeaponAcquiredStatus(int score, WeaponIds id = WeaponIds.None)
    {
        // Stone
        if (score > 0 || id == WeaponIds.Stone)
        {
            UpdateCache(WeaponIds.Stone);
        }
        // ColaCan
        if (id == WeaponIds.ColaCan)
        {
            UpdateCache(WeaponIds.ColaCan);
        }
        // Shoes
        if (score > 7000 || id == WeaponIds.Shoes)
        {
            UpdateCache(WeaponIds.Shoes);
        }
        // Hammer
        if (score > 15000 || id == WeaponIds.Hammer)
        {
            UpdateCache(WeaponIds.Hammer);
        }
    }

    private static void UpdateCache(WeaponIds id)
    {
        var weapon = WeaponData2.Entity.HeroWeaponList.Where(x=>x.WeaponID == id).First();
        if (weapon == null)
        {
            return;
        }
        if(!weapon.IsWeaponAcquired)
        {
            PlayerPrefs.SetInt(weapon.Name, 1);
        }
    }
}



//主人公の武器リスト
//System.Serializableを設定しないと、データを保持できない(シリアライズできない)
[System.Serializable]
public class HeroWeaponStatus
{
    //パラメーター定義
    public WeaponIds WeaponID; // ウェポン管理用のID
    public string Name; // 武器名称
    public string Explanation; // 武器説明
    public string WeaponGetCondition; // 武器獲得の条件説明文
    public GameObject WeaponPrefab; //武器のPrefab
    public Sprite SelectedIcon; //武器選択時のアイコン
    public Sprite ImageOn; // 武器の画像
    public Sprite ImageOff; // 武器のシルエット画像

    // Weapon Properties
    [Range(0, 5)]
    public int Attack;// 攻撃力

    [Range(0, 5)]
    public int Scale; // 大きさ

    [Range(0, 5)]
    public int Distance; // 飛距離

    [Range(0, 5)]
    public int Penetrate; // 貫通力

    [Range(0, 5)]
    public int Rapidfire; // 連射性能

    // -----ウェポン獲得関連--------
    // ウェポン獲得状態の確認(自分自身)
    // true(1): acquired, false(0): secret
    public bool IsWeaponAcquired
    {
        get{
            var cache = PlayerPrefs.GetInt(Name, 0);
            return (cache == 1);
        }
        set{
            ;
        }
    }
}

public enum WeaponIds
{
    None = 0,
    Stone = 1,
    ColaCan,
    Shoes,
    Hammer,
}

public class WeaponProperties : MonoBehaviour // ウェポンのPrefab側に提供したい情報だけはこっちにも実装する。
{
    public int Attack{get; private set;}

    // 初期化メソッド
    public void Initialize(HeroWeaponStatus src)
    {
        Attack = src.Attack;
    }
}

//敵の武器リスト
[System.Serializable]
public class EnemyWeaponStatus
{

    //パラメーター定義
    [SerializeField]
    private string _name;
    [SerializeField]
    private GameObject _weaponPrefab;



    //getter, setter
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public GameObject WeaponPrefab
    {
        get { return _weaponPrefab; }
        set { _weaponPrefab = value; }
    }
}


