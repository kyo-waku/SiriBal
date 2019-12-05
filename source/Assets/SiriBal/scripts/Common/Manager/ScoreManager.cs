using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;
using System;

namespace Generic.Manager{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] static List<Record> _recordList;
        static Record _myBestRecord;
        static Record _myLatestRecord;

        // property for my record
        public static Record MyBestRecord
        {
            get{
                if (_myBestRecord == null)
                {
                    _myBestRecord = LoadData();
                    if(_myBestRecord == null)
                    {
                        _myBestRecord = new Record("Dummy", 0, 0, DateTime.Now);
                    }
                    else
                    {
                        RegisterRecord(_myBestRecord);
                    }
                }

                return _myBestRecord;
            }
            internal set{
                _myBestRecord = value;
            }
        }

        public static Record MyLatestRecord
        {
            get{
                return _myLatestRecord;
            }
            internal set{
                _myLatestRecord = value;
            }
        }
        // Load Cached scores
        public DefinedErrors LoadRecords()
        {
            var result = DefinedErrors.Pass;
            
            // Load all records from server
            // not impl.
            
            return result;
        }

        // Register current score
        public static DefinedErrors RegisterRecord(Record currentScore)
        {
            if(_recordList == null){ _recordList = new List<Record>();}
            var result = DefinedErrors.Pass;

            MyLatestRecord = currentScore;
            _recordList.Add(currentScore);
            
            if(MyBestRecord.GameScore() < currentScore.GameScore())
            {
                MyBestRecord = currentScore;
            }

            SaveData();

            return result;
        }

        // Get records
        // Not sorted
        public static DefinedErrors GetAllRecords(out List<Record> Ranks)
        {
            
            var result = DefinedErrors.Pass;
            var myRecord = MyBestRecord;
            Ranks = new List<Record>();
            if(_recordList == null)
            {
                return result = DefinedErrors.E_Fail;
            }
            Ranks = _recordList;
            return result;
        }

        #region "Save/Load from local"
        internal static void SaveData(){
            PlayerPrefs.SetString("UserName", MyBestRecord.UserName);
            PlayerPrefs.SetString("PlayDateTime", MyBestRecord.PlayDateTime.ToString());
            PlayerPrefs.SetInt("TimeScore", MyBestRecord.TimeScore);
            PlayerPrefs.SetInt("BalloonScore", MyBestRecord.BalloonScore);
            PlayerPrefs.Save();
        }

        internal static Record LoadData(){
            var record = new Record("Dummy", 0, 0, DateTime.Now);
            
            var name = PlayerPrefs.GetString("UserName", "Dummy");
            var dateStr = PlayerPrefs.GetString("PlayDateTime", DateTime.Now.ToString());
            var time = PlayerPrefs.GetInt("TimeScore", 0);
            var balloon = PlayerPrefs.GetInt("BalloonScore", 0);

            record.UserName = name;
            record.PlayDateTime = DateTime.Parse(dateStr);
            record.TimeScore = time;
            record.BalloonScore = balloon;

            return record;
        }

        #endregion


    }
}
