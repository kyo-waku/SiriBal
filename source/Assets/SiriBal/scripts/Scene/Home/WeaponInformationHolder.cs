﻿using UnityEngine;
using Generic;

public class WeaponInformationHolder : MonoBehaviour
{
    public WeaponData StoneScriptableObject;
    public WeaponData ColaCanScriptableObject;
    public WeaponData ShoesScriptableObject;
    public WeaponData HammerScriptableObject;
    // 武器管理の参照先変更により削除してもよい？
    public WeaponData GetWeaponDataFromKey(Weapons weaponKey)
    {
        WeaponData data = null;
        switch(weaponKey)
        {
            case Weapons.Stone:
                data = StoneScriptableObject;
                break;
            case Weapons.ColaCan:
                data = ColaCanScriptableObject;
                break;
            case Weapons.Shoes:
                data = ShoesScriptableObject;
                break;
            case Weapons.Hammer:
                data = HammerScriptableObject;
                break;
            default:
                break;
        }
        return data;
    }
}