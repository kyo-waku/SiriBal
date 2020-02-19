using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class WeaponResultSceneController : MonoBehaviour
{
    private GameSceneManager gameSceneMng;

    // Weapon UI
    [SerializeField]
    Sprite stone_on;
    [SerializeField]
    Sprite stone_off;
    [SerializeField]
    Sprite hammer_on;
    [SerializeField]
    Sprite hammer_off;
    //--------
    float TimeValue = 0;
    bool showWeaponFinished = false;
    bool weaponEffectFinished = false;


    // Start is called before the first frame update
    void Start()
    {
        gameSceneMng = new GameSceneManager();
        ShowResult();
    }

    // Update is called once per frame
    void Update()
    {
        if(DataManager.WResult == null){return;}
        // クリアしてなければ終了
        // if (DataManager.WResult.ClearFlag == false)
        // {
        //     return;
        // }

        // クリアした場合はくるくる回して結果表示
        TimeValue += Time.deltaTime;
        float Rotate = 0;
        const int rotateTime = 8;
        if (TimeValue < rotateTime)
        {
            if(TimeValue < rotateTime/2)
            {
                Rotate = TimeValue * 3;
            }
            else
            {
                Rotate = (rotateTime - TimeValue) * 3;

                if(showWeaponFinished == false) // Weaponの色付きを表示する
                {
                    // Weaponを表示する
                    ShowWeaponResult(true, DataManager.WResult.Weapon);
                    showWeaponFinished = true; // 一度セットしたら戻さない
                }
            }
            GameObject.Find("WeaponImage").transform.rotation *= Quaternion.AngleAxis(Rotate, Vector3.down);
        }
        else
        {
            if(weaponEffectFinished == false)
            {
                weaponEffectFinished = true;
                GameObject.Find("WeaponImage").transform.rotation = Quaternion.identity;
                GameObject.Find("WeaponMessage").GetComponent<Text>().text = "GET NEW WEAPON!!";
            }
        }
    }
    public void RestartButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.SeriousBalloon);
    }
    public void ToTopButtonClicked()
    {
        gameSceneMng.ChangeScene(GameScenes.Home);
    }

    private void ShowResult()
    {
        if(DataManager.WResult == null){return;}
        var resultText = DataManager.WResult.ClearFlag == true? "SUCCESS!!" : "FAILED";
        GameObject.Find("ResultText").GetComponent<Text>().text = resultText;
        // Weaponを表示する(一旦黒背景)
        ShowWeaponResult(false, DataManager.WResult.Weapon);
        // クリアした
        if (DataManager.WResult.ClearFlag == true)
        {
            // 結果をローカルに残しておく
            SaveWRToLocal(DataManager.WResult);
        }
    }

    private void ShowWeaponResult(bool result, Weapons weapon)
    {
        switch(weapon)
        {
            case Weapons.Stone:
                GameObject.Find("WeaponImage").GetComponent<Image>().sprite = (result == true)? stone_on: stone_off;
                break;
            case Weapons.Hammer:
                GameObject.Find("WeaponImage").GetComponent<Image>().sprite = (result == true)? hammer_on: hammer_off;
                break;
            default:
                break;
        }
    }

    // ローカルキャッシュに結果を登録
    internal static void SaveWRToLocal(WeaponResult WR){
        PlayerPrefs.SetInt(WR.Weapon.ToString(), WR.ClearFlag? 1: 0);
        PlayerPrefs.Save();
    }

}
