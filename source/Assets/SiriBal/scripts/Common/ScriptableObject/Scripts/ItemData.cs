using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "ScriptableObject/ItemData")]
public class ItemData : ScriptableObject
{

    //PATHはItemDataで作成したAssetが保存してある場所のパス。
    //HACK:Resources.Load使用のため、Resourcesフォルダ内である必要がある。
    //Resourcesフォルダから移動させた場合はPATHを変更すること
    //例「Resources¥a.asset」の場合は「PATH = "a"」
    public const string PATH = "ItemList";

    //ItemDataを他の場所から参照できるように、ScriptableObjectの実体を定義。
    private static ItemData _entity;
    public static ItemData Entity
    {
        get
        {
            //初アクセス時にロードする
            if (_entity == null)
            {
                _entity = Resources.Load<ItemData>(PATH);
                //ロード出来なかった場合はエラーログを表示
                if (_entity == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }
            return _entity;
        }
    }
    public List<ItemStatus> ItemList = new List<ItemStatus>();
}



//アイテムリスト
//System.Serializableを設定しないと、データを保持できない(シリアライズできない)
[System.Serializable]
public class ItemStatus
{
    //パラメーター定義
    public string Name; // アイテム名
    public string Explanation; // アイテム説明
    public GameObject ItemPrefab; //アイテムのPrefab
    public GameObject DisappearGraphic; //アイテム取得時の消滅グラフィック

    // Item Properties
    public float RecoveryHP;// 回復量

}
