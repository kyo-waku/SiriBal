using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;
using System;
using Generic.Firebase;
using System.Threading.Tasks;

namespace Generic.Manager{
    public class ScoreManager : MonoBehaviour
    {
        FirebaseService service;
        Boolean fetchComplete;
        int fetchCount = 20;
        List<Record> entries;

        // Load Cached scores
        public ScoreManager(FirebaseService fb)
        {
            fetchComplete = false;
            entries = new List<Record>();

            if (fb == null){
                fb = new FirebaseService();
            }
            service = fb;

            if(DataManager.RecordList == null)
            { 
                DataManager.RecordList = new List<Record>();
            }
        }

        // Register current score
        public DefinedErrors RegisterRecord(Record currentScore)
        {
            var result = DefinedErrors.Pass;

            if(DataManager.RecordList == null)
            { 
                DataManager.RecordList = new List<Record>();
            }
            
            DataManager.MyLatestRecord = currentScore;
            DataManager.RecordList.Add(currentScore);

            if (DataManager.MyBestRecord == null)
            {
                // 記録なし -> 現在のスコアがベストスコア
                DataManager.MyBestRecord = currentScore;
                // データ登録時にベストレコードだけローカルキャッシュに保存する
                SaveRecordToLocal(currentScore);
                // サーバーにも登録する
                SaveRecordToRemote(currentScore);
            }
            else
            {
                if (DataManager.MyBestRecord.GameScore() <= currentScore.GameScore())
                {
                    DataManager.MyBestRecord = currentScore;
                    // データ登録時にベストレコードだけローカルキャッシュに保存する
                    SaveRecordToLocal(currentScore);
                    // サーバーにも登録する
                    SaveRecordToRemote(currentScore);
                }
            }

            // 登録したなら再取得していいので
            // fetchComplete情報の初期化
            fetchComplete = false;
            return result;
        }
        // すべてのレコードを取得する（ソート済）
        public DefinedErrors GetAllRecords(out List<Record> Ranks)
        {
            var result = DefinedErrors.Pass;
            Ranks = new List<Record>();
            // サーバーにある０個以上のレコードを取得する
            if (!fetchComplete)
            {
                LoadRecordFromRemote();
                fetchComplete = true;
            }

            if (entries != null)
            {
                if(DataManager.RecordList != null)
                { 
                    DataManager.RecordList.Clear();
                    DataManager.RecordList.AddRange(entries);
                }
            }

            Ranks.AddRange(DataManager.RecordList);
            Ranks.Sort();
            Ranks.Reverse();
            
            return result;
        }

        // ローカルキャッシュに自己ベストを登録
        internal static void SaveRecordToLocal(Record score){
            PlayerPrefs.SetString("UserName", score.UserName);
            PlayerPrefs.SetString("PlayDateTime", score.PlayDateTime.ToString());
            PlayerPrefs.SetInt("TimeScore", score.TimeScore);
            PlayerPrefs.SetInt("BalloonScore", score.BalloonScore);
            PlayerPrefs.SetInt("HitScore", score.HitScore);
            PlayerPrefs.Save();
        }

        // ローカルキャッシュからレコードを取得（自己ベストだけ入っている）
        internal Record LoadRecordFromLocal(){
            var name = PlayerPrefs.GetString("UserName", "Dummy");
            var dateStr = PlayerPrefs.GetString("PlayDateTime", DateTime.Now.ToString());
            var time = PlayerPrefs.GetInt("TimeScore", 0);
            var balloon = PlayerPrefs.GetInt("BalloonScore", 0);
            var hit = PlayerPrefs.GetInt("HitScore", 0);

            return new Record(name, time, balloon, 0, DateTime.Parse(dateStr));
        }

        internal void SaveRecordToRemote(Record record)
        {
            
        }
        public void AddEntry(Record entry, Action onComplete, Action<AggregateException> onError)
        {
            
        }

        internal void LoadRecordFromRemote()
        {
            
        }


        public void UpdateEntries(Action onComplete, Action<AggregateException> onError)
        {
            
        }

        // やりこみモード用
        // ローカルキャッシュに登録
        public void SaveYarikomiScoreToLocal(int score){
            PlayerPrefs.SetInt("LatestScore", score);
            var bestScore = PlayerPrefs.GetInt("BestScore", 0);
            if (bestScore < score)
            {
                PlayerPrefs.SetInt("BestScore", score);
            }
            PlayerPrefs.Save();
        }

        // ローカルキャッシュから最新の結果を取得
        public int LoadYarikomiLatestFromLocal(){
            return PlayerPrefs.GetInt("LatestScore", 0);
        }
        
        // ローカルキャッシュからベストの結果を取得
        public int LoadYarikomiBestFromLocal(){
            return PlayerPrefs.GetInt("BestScore", 0);
        }
    }
}
