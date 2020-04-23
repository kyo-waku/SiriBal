using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;
using System;
using Generic.Firebase;
using System.Threading.Tasks;

namespace Generic.Manager{
    public class ScoreManager
    {
        FirebaseService service;
        private List<Record> records;

        // Load Cached scores
        public ScoreManager(FirebaseService fb)
        {
            service = fb != null? fb : new FirebaseService();
            DataManager.RecordList = DataManager.RecordList != null? DataManager.RecordList: new List<Record>();
        }

        // すべてのレコードをデータベースから取得する
        public async void GetAllRecordsFromDatabase()
        {
            if (service.IsUserLoginned())
            {
                records = await service.GetRankingData();
            }
        }

        public List<Record> GetRecords()
        {
            if (records != null)
            {
                records.Sort((a,b) => b.TotalScore - a.TotalScore);
            }
            return records;
        }

        public void RegisterRecordToDatabase(Record record)
        {
            if (service.IsUserLoginned())
            {
                service.WriteNewScore(record);
            }
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
