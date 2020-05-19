using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Generic;
using Generic.Manager;
public class YarikomiResultSceneController : MonoBehaviour
{
    private GameSceneManager gameSceneMng;
    private GameObject mainCamera;
    private Transform cameraTransform;
    private ScoreManager scoreMng;

    // Status
    private bool IsNewRecord = false;
    private float Timer;
    // UI references
    [SerializeField]
    private GameObject NotNewResultObject;
    [SerializeField]
    private GameObject NewResultObject;
    [SerializeField]
    private GameObject ParticleEffect;
    [SerializeField]
    private GameObject NameInput;
    [SerializeField]
    private GameObject NameInputTextFiled;
    [SerializeField]
    private GameObject RegisterButton;
    // --- Processes ---
    void Start()
    {
        gameSceneMng = new GameSceneManager();
        scoreMng = new ScoreManager(DataManager.service);
        mainCamera = GameObject.Find("AR Camera");
        // 回転したくない
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        ShowResult();
    }

    void Update()
    {
        cameraTransform = mainCamera.transform;
        if (IsNewRecord)
        {
            // 0.5~2秒に 1回 花火を打ち上げる (ただの爆発エフェクト)
            Timer += Time.deltaTime;
            if (Timer > Random.Range(1,4)/2f)
            {
                CreateParticle();
                Timer = 0f;
            }
        }
    }

    private void CreateParticle()
    {
        var RandomPosition = new Vector3(Random.Range(-5,5), Random.Range(-5,5), Random.Range(10,30));
        Instantiate(ParticleEffect, RandomPosition + cameraTransform.position, Quaternion.identity);
    }
    private void ShowResult()
    {
        var latestScore = scoreMng.LoadYarikomiLatestFromLocal();
        var bestScore = scoreMng.LoadYarikomiBestFromLocal();

        if (latestScore >= bestScore)
        {
            ShowNewRecord(latestScore);
            IsNewRecord = true;
        }
        else
        {
            ShowNotNewRecord(latestScore, bestScore);
        }
    }

    private void ShowNewRecord(int newRecord)
    {
        // UI コンポーネントのアクティブステータス更新
        NotNewResultObject.SetActive(false);
        NewResultObject.SetActive(true);

        var resultObj = GameObject.Find("NewRecordText");
        if (resultObj != null)
        {
            resultObj.GetComponent<Text>().text = newRecord.ToString();
        }

        // Weapon獲得したかどうか確認する
        WeaponData.UpdateWeaponAcquiredStatus(newRecord);
    }

    private void ShowNotNewRecord(int latest, int best)
    {
        // UI コンポーネントのアクティブステータス更新
        NotNewResultObject.SetActive(true);
        NewResultObject.SetActive(false);

        var resultObj = GameObject.Find("ResultScoreText");
        if (resultObj != null)
        {
            resultObj.GetComponent<Text>().text = latest.ToString();
        }

        var bestObj = GameObject.Find("BestScoreText");
        if (bestObj != null)
        {
            bestObj.GetComponent<Text>().text = best.ToString();
        }
    }
    public void RestartButtonClicked()
    {
        var stage = new Stage(StageData2.Entity.StageList[0]);
        DataManager.currentStage = stage;

        // 回転の制御を戻す
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        // 一応戻す
        RegisterButton.SetActive(true);
        // シーン遷移
        gameSceneMng.ChangeScene(GameScenes.SeriousBalloon);
    }

    public void ToTopButtonClicked()
    {
        // 回転の制御を戻す
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        // 一応戻す
        RegisterButton.SetActive(true);
        // シーン遷移
        gameSceneMng.ChangeScene(GameScenes.Home);
    }

    public void RegisterButtonClicked()
    {
        NameInput.SetActive(true);
    }
    public void OkayButtonClicked()
    {
        var scoreMng = new ScoreManager(DataManager.service);
        var userName = NameInputTextFiled.GetComponent<Text>().text;
        var score = scoreMng.LoadYarikomiLatestFromLocal();
        scoreMng.RegisterRecordToDatabase(new Record(userName, score));
        NameInput.SetActive(false);
        RegisterButton.SetActive(false);

        // 特殊条件によりWeapon獲得 (ColaCan)
        WeaponData.UpdateWeaponAcquiredStatus(0, WeaponIds.ColaCan);
    }
    public void CancelButtonClicked()
    {
        NameInput.SetActive(false);
    }

    public void OnClickTwitterButton()
    {
        string tweet = "スコアは" + scoreMng.LoadYarikomiLatestFromLocal() + "だったよ";
        //urlの作成
        string esctext = UnityWebRequest.EscapeURL(tweet + "\n みんなも遊んでみよう！！\n");
        string esctag = UnityWebRequest.EscapeURL("SeriousBalloon, きょうわく");
        string url = "https://twitter.com/intent/tweet?text=" + esctext + "&hashtags=" + esctag;

        //Twitter投稿画面の起動
        Application.OpenURL(url);
    }
}
