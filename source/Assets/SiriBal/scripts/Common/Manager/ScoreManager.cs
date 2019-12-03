using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic;

namespace Generic.Manager{
    public class ScoreManager : MonoBehaviour
    {
        static List<Record> _recordList;
        static Record _myLatestRecord;

        // property for my record
        public static Record MyRecord
        {
            get{
                return _myLatestRecord;
            }
            internal set{
                _myLatestRecord = value;
            }
        }

        // Load Cached scores
        public DefinedErrors LoadCachedRecords()
        {
            var result = DefinedErrors.Pass;
            
            // not impl.
            
            return result;
        }

        // Register current score
        public static DefinedErrors RegisterRecord(Record currentScore)
        {
            if(_recordList == null){ _recordList = new List<Record>();}
            var result = DefinedErrors.Pass;
            _recordList.Add(currentScore);
            MyRecord = currentScore;
            return result;
        }

        // Get records
        // Not sorted
        public static DefinedErrors GetAllRecords(out List<Record> Ranks)
        {
            var result = DefinedErrors.Pass;
            Ranks = new List<Record>();
            if(_recordList == null)
            {
                return result = DefinedErrors.E_Fail;
            }
            Ranks = _recordList;
            return result;
        }
    }
}
