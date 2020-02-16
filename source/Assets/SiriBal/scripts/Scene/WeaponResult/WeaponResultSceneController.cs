using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Generic;
using Generic.Manager;

public class WeaponResultSceneController : MonoBehaviour
{
    private GameSceneManager gameSceneMng;

    // Start is called before the first frame update
    void Start()
    {
        gameSceneMng = new GameSceneManager();
        ShowResult();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        var resultText = DataManager.WResult.ClearFlag == true? "SUCCESS!!" : "FAILED";
        GameObject.Find("ResultText").GetComponent<Text>().text = resultText;

        // クリアしたなら、その結果をキャッシュに取っておく
        if (DataManager.WResult.ClearFlag == true)
        {
            SaveWRToLocal(DataManager.WResult);
        }
    }

    // ローカルキャッシュに結果を登録
    internal static void SaveWRToLocal(WeaponResult WR){
        foreach (var weapon in WR.Weapons)
        {
            PlayerPrefs.SetInt(weapon.ToString(), WR.ClearFlag? 1: 0);
        }
        PlayerPrefs.Save();
    }

}
