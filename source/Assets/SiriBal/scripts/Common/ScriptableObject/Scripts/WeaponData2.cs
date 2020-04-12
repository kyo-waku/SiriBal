using System.Collections;
using System.Collections.Generic;
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
}



//主人公の武器リスト
//System.Serializableを設定しないと、データを保持できない(シリアライズできない)
[System.Serializable]
public class HeroWeaponStatus
{
    //パラメーター定義
    [SerializeField]
    private string _name; // 武器名称
    [SerializeField]
    private string _explanation; // 武器説明
    [SerializeField]
    [Range(0, 5)]
    private int _attack; // 攻撃力
    [SerializeField]
    [Range(0, 5)]
    private int _scale; // 大きさ
    [SerializeField]
    [Range(0, 5)]
    private int _distance; // 飛距離
    [SerializeField]
    [Range(0, 5)]
    private int _penetrate; // 貫通力
    [SerializeField]
    [Range(0, 5)]
    private int _rapidfire; // 連射性能
    [SerializeField]
    private GameObject _weaponPrefab; //武器のPrefab
    [SerializeField]
    private Sprite _selectedIcon; //武器選択時のアイコン

    [SerializeField]
    private Sprite _image_on; // 武器の画像
    [SerializeField]
    private Sprite _image_off; // 武器のシルエット画像



    //getter, setter
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public string Explanation
    {
        get { return _explanation; }
        set { _explanation = value; }
    }
    public int Attack
    {
        get { return _attack; }
        set { _attack = value; }
    }
    public int Scale
    {
        get { return _scale; }
        set { _scale = value; }
    }
    public int Distance
    {
        get { return _distance; }
        set { _distance = value; }
    }
    public int Penetrate
    {
        get { return _penetrate; }
        set { _penetrate = value; }
    }
    public int Rapidfire
    {
        get { return _rapidfire; }
        set { _rapidfire = value; }
    }
    public GameObject WeaponPrefab
    {
        get { return _weaponPrefab; }
        set { _weaponPrefab = value; }
    }
    public Sprite SelectedIcon
    {
        get { return _selectedIcon; }
        set { _selectedIcon = value; }
    }
    public Sprite ImageOn
    {
        get { return _image_on; }
        set { _image_on = value; }
    }
    public Sprite ImageOFF
    {
        get { return _image_off; }
        set { _image_off = value; }
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


