using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;
using System;

namespace Generic.Manager{
    public class ScoreManager : MonoBehaviour
    {
        // Load Cached scores
        public DefinedErrors LoadRecords()
        {
            var result = DefinedErrors.Pass;
            
            // Load all records from server
            // not impl.
            
            return result;
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
            }
            else
            {
                if (DataManager.MyBestRecord.GameScore() < currentScore.GameScore())
                {
                    DataManager.MyBestRecord = currentScore;
                    // データ登録時にベストレコードだけローカルキャッシュに保存する
                    SaveRecordToLocal(currentScore);
                }
            }
            return result;
        }

        // すべてのレコードを取得する（ソート済）
        public DefinedErrors GetAllRecords(out List<Record> Ranks)
        {
            var result = DefinedErrors.Pass;
            Ranks = new List<Record>();
            if(DataManager.RecordList == null)
            {
                var record = LoadRecordFromLocal();
                if (record == null)
                {
                    // ローカルキャッシュにもない
                    return result = DefinedErrors.E_Fail;
                }
                // ロードできたので登録する
                result = RegisterRecord(record);
            }
            Ranks = DataManager.RecordList;
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
            PlayerPrefs.Save();
        }

        // ローカルキャッシュからレコードを取得（自己ベストだけ入っている）
        internal Record LoadRecordFromLocal(){
            var name = PlayerPrefs.GetString("UserName", "Dummy");
            var dateStr = PlayerPrefs.GetString("PlayDateTime", DateTime.Now.ToString());
            var time = PlayerPrefs.GetInt("TimeScore", 0);
            var balloon = PlayerPrefs.GetInt("BalloonScore", 0);

            return new Record(name, time, balloon, DateTime.Parse(dateStr));
        }
    }
}
