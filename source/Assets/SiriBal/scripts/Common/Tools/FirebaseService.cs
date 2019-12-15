using Firebase.Auth;
using Firebase.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generic;

namespace Generic.Firebase
{
    public class FirebaseService
    {
        private readonly FirebaseAuth auth;
        private readonly FirebaseFunctions functions;

        public FirebaseService()
        {
            auth = FirebaseAuth.DefaultInstance;
            functions = FirebaseFunctions.GetInstance("asia-northeast1");
        }

        public bool IsAuthenticated()
        {
            return auth.CurrentUser != null;
        }

        public async Task<FirebaseUser> SignInAnonymouslyAsync()
        {
            return await auth.SignInAnonymouslyAsync();
        }

        public Task SignOut()
        {
            return auth.CurrentUser.DeleteAsync().ContinueWith(task =>
            {
                auth.SignOut();
            });
        }

        public async Task<object> AddEntryAsync(Record entry)
        {
            object data = new Dictionary<object, object>
            {
                { "name", entry.UserName },
                { "time", entry.TimeScore },
                { "balloon", entry.BalloonScore},
                { "date", entry.PlayDateTime.ToString()}
            };

            return await functions.GetHttpsCallable("addEntry").CallAsync(data)
                .ContinueWith(task =>
                {
                    return task.Result.Data;
                });
        }

        public async Task<IEnumerable<Record>> GetTopEntriesAsync(int count)
        {
            object data = new Dictionary<object, object>
            {
                { "count", count },
            };

            return await functions.GetHttpsCallable("getTopEntries").CallAsync(data)
                .ContinueWith(task =>
                {
                    var result = (Dictionary<object, object>)task.Result.Data;
                    return ((List<object>)result["entries"])
                        .Select(e => CreateFromEntryObject(e));
                });
        }

        public static Record CreateFromEntryObject(object obj)
        {
            Dictionary<object, object> entry = (Dictionary<object, object>)obj;
            return new Record(
                name: (string)entry["name"],
                time: Convert.ToInt32(entry["time"]),
                balloon: Convert.ToInt32(entry["balloon"]),
                date: DateTime.Parse(Convert.ToString(entry["date"]))
                );
        }
    }
}