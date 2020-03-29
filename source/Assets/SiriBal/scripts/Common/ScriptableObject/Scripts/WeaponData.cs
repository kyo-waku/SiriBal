using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;

[CreateAssetMenu(menuName = "ScriptableObject/WeaponData")]
public class WeaponData : ScriptableObject {
    public Weapons weaponKey;


	[Range (0, 5)]
    public int attack; // 攻撃力
	[Range (0, 5)]
    public int size; // 大きさ
	[Range (0, 5)]
    public int distance; // 飛距離
	[Range (0, 5)]
    public int penetrate; // 貫通力
	[Range (0, 5)]
    public int rapidfire; // 連射性能
    public string explanation; // ウェポンの説明
    public Sprite image_on; // ウエポンの画像
    public Sprite image_off; // ウエポンのシルエット画像
    
}