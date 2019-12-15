using System.Collections;
using System.Collections.Generic;
using Generic;
using Generic.Firebase;

namespace Generic.Manager{
    class DataManager
    {
        #region GameSceneManager
        // Current Scene
        public static GameScenes CurrentScene = GameScenes.None;
        // Previous Scene
        public static GameScenes PrevScene = GameScenes.None;
        #endregion

        #region ScoreManager
        public static List<Record> RecordList;
        public static Record MyBestRecord;
        public static Record MyLatestRecord;

        #endregion

        #region Firebase
        public static FirebaseService service;

        #endregion
    }
}