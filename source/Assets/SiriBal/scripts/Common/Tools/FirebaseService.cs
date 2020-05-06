using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Generic.Firebase
{
    public class FirebaseService
    {
        private FirebaseAuth _auth;
        private DatabaseReference _database;

        // Defines
        private const string FIREBASE_REALTIMEDATABASE_URL= "https://seriousballoon-kyowaku-82546.firebaseio.com/";
        private const string FIREBASE_REALTIMEDATABASE_URL_DEV = "https://seriousballoon-kyo-waku-dev.firebaseio.com/";
        private const string DATABASE_KEY = "ScoreBoard";
        private const string ENTRY_SCORE = "score";
        private const string ENTRY_NAME = "name";
        private const string ENTRY_ID = "id";
        private const string ENTRY_DATE = "date";

        private string FirebaseUrl{
            get{
                return Debug.isDebugBuild? FIREBASE_REALTIMEDATABASE_URL_DEV: FIREBASE_REALTIMEDATABASE_URL;
            }
        }

        public FirebaseService()
        {
            // Initialize Firebase Instances
            _auth = FirebaseAuth.DefaultInstance;
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(FirebaseUrl);
            _database = FirebaseDatabase.DefaultInstance.GetReference(DATABASE_KEY);
            // 匿名認証
            AnonymousLogin();
        }

        public bool IsUserLoginned()
        {
            return (_auth.CurrentUser != null);
        }

        private void AnonymousLogin()
        {
            _auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            });
        }
        // Firebase Realtime Database から 登録されているレコードを取得する
        public async Task<List<Record>> GetRankingData(int fetchCount = 8)
        {
            return await _database.OrderByChild(ENTRY_SCORE).LimitToLast(fetchCount).GetValueAsync().ContinueWith(task => {
                var records = new List<Record>();
                if (task.IsFaulted) {
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    IEnumerator<DataSnapshot> result = snapshot.Children.GetEnumerator();
                    while (result.MoveNext()) {
                        DataSnapshot data = result.Current;
                        string name = (string)data.Child(ENTRY_NAME).Value;
                        // Firebaseの数値データはLong型となっているので、一度longで受け取った後にintにキャスト
                        int score   = (int)(long)data.Child(ENTRY_SCORE).Value;
                        records.Add(new Record(name, score));
                    }
                }
                return records;
            });
        }

        // Firebase Realtime Database に書き込む
        public void WriteNewScore(Record record) {
            string key = FirebaseDatabase.DefaultInstance.GetReference(DATABASE_KEY).Push().Key;
            Guid guid = Guid.NewGuid();
            Dictionary<string, object> itemMap = new Dictionary<string, object>();
            itemMap.Add("name", record.UserName);
            itemMap.Add("id", guid.ToString());
            itemMap.Add("score", record.TotalScore);
            itemMap.Add("updatedate", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add(guid.ToString(), itemMap);
            _database.UpdateChildrenAsync(map);
        }
    }
}